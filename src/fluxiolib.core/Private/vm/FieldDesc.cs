namespace fluxiolib.Internal;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal readonly unsafe struct FieldDesc
{
    #region constants
    public const int FIELD_OFFSET_MAX = (1 << 27) - 1;
    public const int FIELD_OFFSET_UNPLACED = FIELD_OFFSET_MAX;

    public const int FIELD_OFFSET_UNPLACED_GC_PTR = FIELD_OFFSET_MAX - 1;
    public const int FIELD_OFFSET_VALUE_CLASS = FIELD_OFFSET_MAX - 2;
    public const int FIELD_OFFSET_NOT_REAL_FIELD = FIELD_OFFSET_MAX - 3;

    public const int FIELD_OFFSET_NEW_ENC = FIELD_OFFSET_MAX - 4;
    public const int FIELD_OFFSET_BIG_RVA = FIELD_OFFSET_MAX - 5;
    public const int FIELD_OFFSET_LAST_REAL_OFFSET = FIELD_OFFSET_MAX - 6;
    #endregion
    
    #region fields
    // PTR_MethodTable  m_pMTOfEnclosingClass
    public readonly MethodTable* pMethodTable;

    // m_mb                 : 24
    // m_isStatic           : 1
    // m_isThreadLocal      : 1
    // m_isRVA              : 1
    // m_prot               : 3
    public readonly uint mb;

    // m_dwOffset           : 27
    // m_type               : 5
    public readonly uint dwOffset;
    #endregion

    #region property
    public uint Offset
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => dwOffset & FIELD_OFFSET_MAX;
    }

    public uint Prot
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => mb >> 27;
    }

    public uint Type
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => dwOffset >> 27;
    }
    #endregion

    #region static methods
    // is instance member?
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool IsInstance(FieldDesc* pThis) => (pThis->mb & (1u << (24 + 1))) == 0;

    // attribute fast match
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool AttrMatch(FieldDesc* pThis, uint prot, uint type)
    {
        int s1 = (int)pThis->Prot;
        int s2 = (int)pThis->Type;

        return
            ((prot >> s1) & 1) != 0 &
            ((type >> s2) & 1) != 0;
    }
    
    // name match
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool NameMatch(FieldDesc* pThis, SStringUtf8 name)
    {
        // fpMatch가 null이면 AnyAccept 모드로 동작하도록 합니다.
        if (name.fpMatch == null)
            return true;

        var thisFieldName = FieldDescList.GetNameAsSpan(pThis);

#if DEBUG
        SStringUtf8 debugThisFieldName = FieldDescList.GetNameAsSpan(pThis);
        SStringUtf8.DebugView(ref debugThisFieldName);
#endif

        return SStringUtf8.Match(thisFieldName, name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool Equals(scoped ref readonly FieldDesc a, scoped ref readonly FieldDesc b)
    {
        ref long pa = ref Unsafe.As<FieldDesc, long>(ref Unsafe.AsRef(in a));
        ref long pb = ref Unsafe.As<FieldDesc, long>(ref Unsafe.AsRef(in b));

        return
            ((pa ^ pb) | (Unsafe.Add(ref pa, 1) ^ Unsafe.Add(ref pb, 1))) == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int GetHashCode(scoped ref readonly FieldDesc a) =>
        Unsafe.As<FieldDesc, UInt128>(ref Unsafe.AsRef(in a)).GetHashCode();
    #endregion
}
