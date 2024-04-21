#if NET8_0_OR_GREATER
namespace fluxiolib.Internal;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal readonly unsafe struct MethodTable
{
    // DWORD m_dwFlags
    public readonly uint dwFlags;

    // DWORD m_BaseSize
    public readonly uint baseSize;

    // DWORD m_dwFlags2
    public readonly uint dwFlags2;

    // WORD m_wNumVirtuals
    public readonly ushort numVirtuals;
    // WORD m_wNumInterfaces;
    public readonly ushort numInterfaces;

    //
    // == 16 bytes ==
    //

    // PTR_MethodTable m_pParentMethodTable
    public readonly MethodTable* pParentMT;

    // PTR_Module m_pModule
    public readonly ModuleFake* pModule;

    // PTR_MethodTableAuxiliaryData m_pAuxiliaryData
    public readonly void* pAuxiliaryData;

    // union {
    //   DPTR(EEClass) m_pEEClass;
    //   TADDR m_pCanonMT;
    // };
    public readonly EEClass* pEEClass;

    //
    // == 48 bytes ==                   END OF FIELDS
    //

    [MethodImpl(HOME.__inline)]
    public static uint GetNumInstanceFields(scoped ref readonly MethodTable pMT)
    {
        var pClass = pMT.pEEClass;

        uint v1 = EEClass.getValue(&pClass->numThreadStaticFields);
        uint v2 = EEClass.getValue(&pClass->numInstanceFields);

        v1 =
            (nint)pMT.pParentMT == Unsafe.As<RtType>(typeof(ValueType)).m_handle
            ? v1
            : v2;

        return v1;
    }
}
#endif