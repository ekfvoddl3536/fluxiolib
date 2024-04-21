#if NET8_0_OR_GREATER
namespace fluxiolib.Internal;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal readonly unsafe struct EEClass
{
    // !=== Using void* to eliminate type reference loops ===!

    // PTR_GuidInfo m_pGuidInfo
    public readonly void* pGuidInfo;

    // PTR_EEClassOptionalFields m_rpOptionalFields
    public readonly void* pOptionalFields;

    // PTR_MethodTable m_pMethodTable
    public readonly void* pMethodTable;

    // PTR_FieldDesc m_pFieldDescList
    public readonly void* pFieldDescList;

    // PTR_MethodDescChunk m_pChunks
    public readonly void* pChunks;

    // union {
    //   OBJECTHANDLE    m_ohDelegate;
    //   CorIfaceAttr    m_ComInterfaceType;
    // }
    public readonly void* m_COMINTEROP;     // Unix:: uint32* ([0]: AttrClass, [1]: VMFlags)

    public readonly void* m_UNKNOWN_PTR;    // Unix:: NormType

    //
    // == 48 bytes ==
    //

    // DWORD m_dwAttrClass
    public readonly uint AttrClass;         // Unix:: numInstanceFields
    // DWORD m_VMFlags
    public readonly uint VMFlags;           // Unix:: numMethods

    // NOTE: Following BYTE fields are laid out together so they'll fit within the same DWORD for efficient structure packing.
    // BYTE m_NormType
    public readonly uint NormType;          // Unix:: numStaticFields
    // BYTE m_cbBaseSizePadding
    public readonly uint cbBaseSizePadding;

    //
    // == 64 bytes ==
    //

    // WORD m_NumInstanceFields
    public readonly uint numInstanceFields;
    // WORD m_NumMethods
    public readonly uint numMethods;
    // WORD m_NumStaticFields
    public readonly uint numStaticFields;   // Unix:: numThreadStaticFields, Win32NT:: numTotalStaticFields(staticfields + threadstaticfields)
    // WORD m_NumHandleStatics
    public readonly uint numHandleStatics;

    //
    // == 80 bytes ==
    //

    // WORD m_NumThreadStaticFields
    public readonly uint numThreadStaticFields; // Win32NT:: ??? always zero
    // WORD m_NumHandleThreadStatics
    public readonly uint numHandleThreadStatics;
    // WORD m_NumNonVirtualSlots
    public readonly uint numNonVirtualSlots;
    // DWORD m_NonGCStaticFieldBytes
    public readonly uint nonGCStaticFieldBytes;

    //
    // == 96 bytes ==
    //

    // DWORD m_NonGCThreadStaticFieldBytes
    public readonly uint nonGCThreadStaticFieldBytes;

    //
    // == 128 bytes == (16 PADDING) NOTE: 16바이트 패딩은 원래 코드에서 없는 것임, 임의로 지정한 값
    //

    /// <summary>
    /// <see cref="AttrClass"/> 이상의 필드에 액세스할 때, OS에 종속적인 오프셋 접근을 계산하여 값을 읽습니다
    /// </summary>
    [MethodImpl(HOME.__inline)]
    public static uint getValue(uint* pFieldValueRef) =>
        PlatformHelper.isWinNT
        ? *pFieldValueRef
            // unix adjust offset
        : *(uint*)((byte*)pFieldValueRef - (sizeof(nuint) + sizeof(nuint)));
}
#endif