#if NET8_0_OR_GREATER
namespace fluxiolib.Internal;

unsafe partial class FieldDescList
{
    /// <summary>
    /// 필드를 특징 필터를 사용하여, 고속으로 검색합니다.
    /// </summary>
    /// <param name="pMT">읽어들일 타입의 메소드 테이블 포인터</param>
    /// <param name="pFilter_in">필드 필터를 수행할 필터 정보를 담은 객체</param>
    /// <param name="name">필드 이름으로 매칭하기 위한 이름</param>
    /// <returns>필드를 찾은 경우 null이 아님</returns>
    /// <remarks>
    /// 이 API는 이름으로 매칭하는 것을 최저 우선순위로 하여, 접근제한자와 타입으로 먼저 매칭을 수행합니다.
    /// 만약, 접근제한자와 타입 매칭이 ALL로 표시된 경우 이 API는 이름으로 매칭하는 것과 같으며, 단순히 이름으로 매칭하는것보다 더 느려집니다.
    /// </remarks>
    [SkipLocalsInit]
    public static FieldDesc* GetInstanceFieldQuick(
        scoped ref readonly MethodTable pMT,
        scoped ref readonly FluxMemberFilter pFilter_in,
        SStringUtf8 name)
    {
        // NOTE:
        //  실행 성능을 위해 이 코드를 합치지 마세요.
        //  여기 코드를 수정했다면 이 코드와 동일한 역할을 수행하는 다른 코드도 수정해야 합니다.

        var numFDs = MethodTable.GetNumInstanceFields(in pMT);
        if (numFDs < pFilter_in.skipFields + (ulong)pFilter_in.maxCount || pFilter_in.maxCount == 0)
            return null;

        uint bitProt = pFilter_in.bitMask_prot;
        uint bitType = pFilter_in.bitMask_type;

        uint numSkip = pFilter_in.skipFields;
        uint maxFDs =
            Math.Min(
                numSkip + pFilter_in.maxCount,
                numFDs);

        for (ref readonly MethodTable pThisMT = ref pMT; numFDs > numSkip;)
        {
            FieldDesc* pFDList = (FieldDesc*)pThisMT.pEEClass->pFieldDescList;

            while (pFDList == null)
            {
                pThisMT = ref *pThisMT.pParentMT;
                pFDList = (FieldDesc*)pThisMT.pEEClass->pFieldDescList;
            }
            
            MethodTable* parentMT = pThisMT.pParentMT;

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

            for (; pFDList != pFDListEnd; ++pFDList)
                if (FieldDesc.IsInstance(pFDList) &&
                    FieldDesc.AttrMatch(pFDList, bitProt, bitType) &&
                    FieldDesc.NameMatch(pFDList, name))
                    return pFDList;

            pThisMT = ref *parentMT;
        }

        return null;
    }
}
#endif