#if NET8_0_OR_GREATER
using System.Diagnostics;

namespace fluxiolib.Internal;

using static SpanCreateHelper;
internal static unsafe partial class FieldDescList
{
    /// <summary>
    /// 필드 목록을 가져온다
    /// </summary>
    /// <param name="pMT">읽어들일 타입의 메소드 테이블 포인터</param>
    /// <returns>결과 배열이다. Pinned된 배열이므로 포인터로 접근해도 된다.</returns>
    public static FieldDesc[] GetList(scoped ref readonly MethodTable pMT)
    {
        int count = 0;
        if (GetAllFields(in pMT, null, &count))
            return []; // 아무런 필드도 없다

        FieldDesc[] result = GC.AllocateUninitializedArray<FieldDesc>(count, true);

        ref FieldDesc pFirst = ref MemoryMarshal.GetArrayDataReference(result);
        GetAllFields(in pMT, (FieldDesc*)Unsafe.AsPointer(ref pFirst), &count);

        return result;
    }

    /// <summary>
    /// 모든 필드 정보를 읽는다
    /// </summary>
    /// <param name="pMT">읽어들일 타입의 메소드 테이블 포인터</param>
    /// <param name="pBuffer_dst">결과를 저장할 대상 버퍼의 포인터</param>
    /// <param name="pCount_inout">결과가 저장될 버퍼의 크기, 이 메소드가 실패할 경우 이 값은 요구되는 최소 버퍼 크기가 기록된다</param>
    /// <returns>
    /// 성공하면 true이다.
    /// 실패한 경우, <paramref name="pCount_inout"/>에 필요한 버퍼 크기가 기록된다.
    /// </returns>
    public static bool GetAllFields(scoped ref readonly MethodTable pMT, FieldDesc* pBuffer_dst, int* pCount_inout)
    {
        Debug.Assert(pCount_inout != null);

        uint numFDs = MethodTable.GetNumInstanceFields(in pMT);
        if ((uint)*pCount_inout < numFDs)
        {
            *pCount_inout = (int)numFDs;
            return false;
        }

        Debug.Assert(pBuffer_dst != null);

        FieldDesc* pbuffer = pBuffer_dst;
        int bufLength = *pCount_inout;

        // 읽은 개수
        *pCount_inout = (int)numFDs;

        for (ref readonly MethodTable pThisMT = ref pMT; numFDs != 0;)
        {
            FieldDesc* pFDList = (FieldDesc*)pThisMT.pEEClass->pFieldDescList;

            // 필드가 1개 이상이면 object는 제외된다.
            //  -> 반드시 부모가 존재한다
            //  -> 부모 클래스 중 누군가 필드를 갖고 있다(반드시)
            while (pFDList == null)
            {
                pThisMT = ref *pThisMT.pParentMT;
                pFDList = (FieldDesc*)pThisMT.pEEClass->pFieldDescList;
            }

            // 현재 클래스의 필드 개수를 알아내기 위해, 부모 클래스에 기록된 총 필드 개수를 가져온다
            // * object의 필드 개수는 0개이므로, 이 과정에 참여할 수 없다.
            MethodTable* parentMT = pThisMT.pParentMT;

            uint parentNumInstanceFields = EEClass.getValue(&parentMT->pEEClass->numInstanceFields);
            uint adjustFieldCount = numFDs - parentNumInstanceFields;

            // 현재 클래스의 필드는 처리됐다
            numFDs = parentNumInstanceFields;

            _ROS(pFDList, (int)adjustFieldCount).CopyTo(_S(pbuffer, bufLength));

            // 버퍼 길이는 검증돼있으므로, 길이를 줄이지 않는다
            pbuffer += adjustFieldCount;

            // 다음으로
            pThisMT = ref *parentMT;
        }

        return true;
    }
}
#endif