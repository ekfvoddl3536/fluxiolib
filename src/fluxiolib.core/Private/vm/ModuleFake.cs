namespace fluxiolib.Internal;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal readonly unsafe struct ModuleFake
{
    public readonly LookupMap _dummyA, _dummyB, _dummyC;

    // vm/crst.h#L108, "class CrstBase"
    // T_CRITICAL_SECTION m_criticalsection;
    // DWORD m_dwFlags;
    // NOTE:: "T_CRITICAL_SECTION" has different type sizes across platforms. (platform-dependent type size)

    // where's 12 or 8-bytes?
    // DWORD m_dwFlags; -> PADDING -> 8-bytes left.
    // where?
    // 
    // unknown (still unknown this field. but, is requires.)
    public readonly nuint UNKNOWN_PTR;

    // PTR_LoaderAllocator     m_loaderAllocator;
    public readonly nuint m_loaderAllocater;

    // const ReadyToRun_EnclosingTypeMap *m_pEnclosingTypeMap 
    public readonly nuint m_pEnclosingTypeMap;

    public readonly nuint pSimpleName;
    public readonly PEAssemblyFake* pPEAssembly; // Win32NT Offset-> 0xB0

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static PEAssemblyFake* getPEAssembly(ModuleFake* pThis) =>
        *(PEAssemblyFake**)((byte*)&pThis->pPEAssembly + PlatformHelper.CSRTBASE_SIZE);
}
