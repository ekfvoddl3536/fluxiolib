using System.Collections;

namespace fluxiolib.Internal;

internal readonly unsafe struct FluxFilteredEnumerable : IEnumerable<FluxRuntimeFieldDesc>
{
    private readonly MethodTable* pTable;
    private readonly FluxMemberFilter filter;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public FluxFilteredEnumerable(
        scoped ref readonly MethodTable pMT, 
        scoped ref readonly FluxMemberFilter pFilter)
    {
        pTable = (MethodTable*)Unsafe.AsPointer(ref Unsafe.AsRef(in pMT));
        filter = pFilter;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Enumerator GetEnumerator() => new(in this);

    IEnumerator<FluxRuntimeFieldDesc> IEnumerable<FluxRuntimeFieldDesc>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<FluxRuntimeFieldDesc>
    {
        private readonly MethodTable* pMT;
        private readonly FluxMemberFilter filter;

        private uint numFDs;

        private uint maxFDs;

        private FieldDesc* current;
        private FieldDesc* pListEnd;
        private MethodTable* pNextMT;

        private delegate* managed<in Enumerator, bool> fpMoveNext;

        public Enumerator(in FluxFilteredEnumerable item)
        {
            pMT = item.pTable;
            filter = item.filter;

            numFDs = 0;

            fpMoveNext = &STEP1_ReadyForEnumerator;
        }

        public readonly FluxRuntimeFieldDesc Current
        {
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            get => new(current);
        }
        readonly object IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public readonly bool MoveNext() => fpMoveNext(in this);

        public void Reset() => fpMoveNext = &STEP1_ReadyForEnumerator;

        public readonly void Dispose() { }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static bool STEP1_ReadyForEnumerator(in Enumerator me)
        {
            ref readonly FluxMemberFilter filter = ref me.filter;

            var numFDs = MethodTable.GetNumInstanceFields(in *me.pMT);
            if (numFDs < filter.skipFields + (ulong)filter.maxCount || filter.maxCount == 0)
                return false;

            ref var _this = ref Unsafe.AsRef(in me);

            _this.numFDs = numFDs;
            _this.maxFDs =
                Math.Min(
                    filter.skipFields + filter.maxCount,
                    numFDs);

            _this.fpMoveNext = &STEP2_MoveNext;
            _this.pNextMT = me.pMT;

            return STEP2_MoveNext(in me);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static bool STEP2_MoveNext(in Enumerator me)
        {
            ref readonly FluxMemberFilter filter = ref me.filter;

            uint bitProt = filter.bitMask_prot;
            uint bitType = filter.bitMask_type;

            uint numSkip = filter.skipFields;
            uint maxFDs = me.maxFDs;

            uint numFDs = me.numFDs;

            for (MethodTable* pThisMT = me.pNextMT; numFDs > numSkip;)
            {
                FieldDesc* pFDList = (FieldDesc*)pThisMT->pEEClass->pFieldDescList;

                while (pFDList == null)
                {
                    pThisMT = pThisMT->pParentMT;
                    pFDList = (FieldDesc*)pThisMT->pEEClass->pFieldDescList;
                }

                MethodTable* parentMT = pThisMT->pParentMT;

                uint parentNumInstanceFields = EEClass.getValue(&parentMT->pEEClass->numInstanceFields);

                uint adjustFieldCount =
                    Math.Min(
                        (uint)Math.Max(
                            (int)(maxFDs - parentNumInstanceFields),
                            0),
                        numFDs - parentNumInstanceFields);

                uint offset =
                    (uint)Math.Max(
                        (int)(numSkip - parentNumInstanceFields),
                        0);

                FieldDesc* pFDListEnd = pFDList + adjustFieldCount;
                pFDList += offset;

                numFDs = parentNumInstanceFields;

                pThisMT = parentMT;

                for (; pFDList != pFDListEnd; ++pFDList)
                    if (FieldDesc.IsInstance(pFDList) && FieldDesc.AttrMatch(pFDList, bitProt, bitType))
                    {
                        ref var _this = ref Unsafe.AsRef(in me);

                        _this.current = pFDList;
                        _this.pListEnd = pFDListEnd;
                        _this.pNextMT = pThisMT;
                        _this.numFDs = numFDs;

                        _this.fpMoveNext = &STEP3_QueryToEnd;

                        return true;
                    }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static bool STEP3_QueryToEnd(in Enumerator me)
        {
            ref readonly FluxMemberFilter filter = ref me.filter;

            uint bitProt = filter.bitMask_prot;
            uint bitType = filter.bitMask_type;

            var pFDList = me.current + 1;
            var pFDListEnd = me.pListEnd;

            for (; pFDList != pFDListEnd; ++pFDList)
                if (FieldDesc.IsInstance(pFDList) && FieldDesc.AttrMatch(pFDList, bitProt, bitType))
                {
                    Unsafe.AsRef(in me).current = pFDList;

                    return true;
                }

            // tail call
            return STEP2_MoveNext(in me);
        }
    }
}
