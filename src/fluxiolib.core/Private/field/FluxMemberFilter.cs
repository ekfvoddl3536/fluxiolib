namespace fluxiolib.Internal;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal struct FluxMemberFilter
{
    public uint bitMask_prot; // Access modifier

    // 부모 클래스에서 시작하여, 스킵할 필드 개수.
    // A(3)->B(5)->C(5) 로 총 필드 13개가 있을 때,
    // skip = 8을 지정하면 A와 B의 필드를 검색하지 않는다.
    public uint skipFields;

    public uint bitMask_type; // CorElementType

    // 검색할 필드의 최대 개수
    public uint maxCount;

    public readonly bool isZero
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveInlining)]
        get => (bitMask_prot | bitMask_type) == 0;
    }
}
