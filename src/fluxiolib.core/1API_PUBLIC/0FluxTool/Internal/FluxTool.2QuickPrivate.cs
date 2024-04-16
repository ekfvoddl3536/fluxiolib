using fluxiolib.Internal;

namespace fluxiolib;

static unsafe partial class FluxTool
{
    [SkipLocalsInit, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static FluxRuntimeFieldDesc GetInstanceField_Unsafe(
        scoped ref readonly MethodTable pMT,
        SStringUtf8 utf8Name,
        ProtFlags protection,
        TypeFlags cortype,
        uint index,
        uint count)
    {
        FluxMemberFilter filter;

        filter.bitMask_prot = (uint)protection;
        filter.bitMask_type = (uint)cortype;
        filter.skipFields = index;
        filter.maxCount = count;

        var result = FieldDescList.GetInstanceFieldQuick(in pMT, in filter, utf8Name);

        return
            result == null
            ? default
            : new(result);
    }

    [SkipLocalsInit, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static FluxRuntimeFieldDesc GetInstanceField_OffLen(
        scoped ref readonly MethodTable pMT,
        SStringUtf8 utf8Name,
        ProtFlags protection,
        TypeFlags cortype,
        FluxSearchSpace space)
    {
        ArgumentOutOfRangeException.ThrowIfZero((int)protection, nameof(protection));
        ArgumentOutOfRangeException.ThrowIfZero((int)cortype, nameof(cortype));

        (uint index, uint count) = space.GetOffsetAndLength(in pMT);
        ArgumentOutOfRangeException.ThrowIfNegative((int)index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfNegative((int)count, nameof(count));

        ArgumentOutOfRangeException.ThrowIfLessThan((int)(MethodTable.GetNumInstanceFields(in pMT) - index), (int)count, nameof(count));

        return GetInstanceField_Unsafe(in pMT, utf8Name, protection, cortype, index, count);
    }
}
