#if NET8_0_OR_GREATER
#pragma warning disable FLXLIB0201
using System.Text;

namespace fluxiolib;

/// <summary>
/// 간단한 UTF8 문자열.
/// </summary>
/// <remarks>
/// 이 구조체는 다른 UTF8 문자열들에 대하여 비교를 수행하기 위한 목적으로 사용됩니다.
/// </remarks>
#if DEBUG
[System.Diagnostics.DebuggerTypeProxy(typeof(SStringUtf8DebugDisplay))]
[System.Diagnostics.DebuggerDisplay("{ToString()}")]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 16)]
public readonly unsafe ref struct SStringUtf8
{
    #region static property
    /// <summary>
    /// 모든 문자열을 수락하는 <see cref="SStringUtf8"/>을(를) 반환합니다.
    /// </summary>
    public static SStringUtf8 AnyAccept => new(""u8, StringMatchType.AnyAccept);
    #endregion

    internal readonly ReadOnlySpan<byte> utf8String;
    internal readonly delegate* managed<in ReadOnlySpan<byte>, in ReadOnlySpan<byte>, bool> fpMatch;

    /// <include file='0docs/SStringUtf8.xml' path='docs/ctor_1/*'/>
    /// <include file='0docs/SStringUtf8.xml' path='docs/ctor_arg1/*'/>
    /// <include file='0docs/SStringUtf8.xml' path='docs/ctor_example1/*'/>
    [MethodImpl(HOME.__inline)]
    public SStringUtf8(ReadOnlySpan<byte> utf8String, StringMatchType type = StringMatchType.Default)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)type, (uint)StringMatchType.Reserved, nameof(type));

        this.utf8String = utf8String;

        fpMatch = StringMatchHelper.buffer[(int)type];
    }

    /// <include file='0docs/SStringUtf8.xml' path='docs/ctor_2/*'/>
    /// <include file='0docs/SStringUtf8.xml' path='docs/ctor_example2/*'/>
    [MethodImpl(HOME.__inline)]
    public SStringUtf8(ReadOnlySpan<char> utf16String, StringMatchType type = StringMatchType.Default)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)type, (uint)StringMatchType.Reserved, nameof(type));

        utf8String = ToUtf8(utf16String);

        fpMatch = StringMatchHelper.buffer[(int)type];
    }

    /// <include file='0docs/SStringUtf8.xml' path='docs/ctor_1/*'/>
    /// <param name="fpMatch">
    /// 두 문자열의 비교에 사용할 함수 포인터 콜백입니다. 
    /// <para/>
    /// 시그니처: <see langword="bool"/> fpMatch(<see langword="in"/> <see cref="ReadOnlySpan{T}"/> other, <see langword="in"/> <see cref="ReadOnlySpan{T}"/> <paramref name="utf8String"/>);
    /// </param>
    /// <include file='0docs/SStringUtf8.xml' path='docs/ctor_exmaple3/*'/>
    [CLSCompliant(false), MethodImpl(HOME.__inline)]
    public SStringUtf8(ReadOnlySpan<byte> utf8String, delegate* managed<in ReadOnlySpan<byte>, in ReadOnlySpan<byte>, bool> fpMatch)
    {
        this.utf8String = utf8String;
        this.fpMatch = fpMatch;
    }

    /// <summary>
    /// UTF8 인코딩된 문자열의 길이입니다.
    /// </summary>
    public int Length
    {
        [MethodImpl(HOME.__inline)]
        get => utf8String.Length;
    }

    /// <summary>
    /// UTF8 인코딩된 문자열에서 지정된 위치의 하나의 문자를 읽습니다.
    /// </summary>
    /// <param name="index">읽을 문자의 위치</param>
    /// <returns>읽은 문자입니다.</returns>
    public byte this[int index]
    {
        [MethodImpl(HOME.__inline)]
        get => utf8String[index];
    }

    /// <summary>
    /// 이 <see cref="SStringUtf8"/> 개체가 표현하는 utf8 문자열을 <see cref="ReadOnlySpan{T}"/>으로 반환합니다.
    /// </summary>
    /// <returns>UTF8 문자열 데이터</returns>
    [MethodImpl(HOME.__inline)]
    public ReadOnlySpan<byte> AsSpan() => utf8String;

    /// <summary>
    /// 다른 문자열에 대하여 현재 문자열 비교를 수행합니다.
    /// </summary>
    /// <param name="otherUtf8String">다른 문자열</param>
    /// <returns>비교에 성공하였는지 여부</returns>
    [MethodImpl(HOME.__inline)]
    public bool Match(ReadOnlySpan<byte> otherUtf8String) => fpMatch(otherUtf8String, utf8String);

    /// <summary>
    /// 이 <see cref="SStringUtf8"/>가 표현하는 UTF8 문자열을 <see cref="string"/>으로 변환합니다.
    /// </summary>
    /// <returns>문자열</returns>
    public override string ToString() => Encoding.UTF8.GetString(utf8String);

#pragma warning disable CS1591
    [MethodImpl(HOME.__inline)]
    public static implicit operator SStringUtf8(ReadOnlySpan<byte> utf8) => new(utf8, StringMatchType.Default);
    [MethodImpl(HOME.__inline)]
    public static implicit operator SStringUtf8(ReadOnlySpan<char> utf16) => new(utf16, StringMatchType.Default);
    [MethodImpl(HOME.__inline)]
    public static implicit operator SStringUtf8(string utf16) => new(utf16.AsSpan(), StringMatchType.Default);

    [MethodImpl(HOME.__inline)]
    public static implicit operator ReadOnlySpan<byte>(SStringUtf8 utf8) => utf8.utf8String;
    // mov rax, [rdx+0x10]
    // jmp rax
    [MethodImpl(HOME.__inline)]
    public static bool Match(ReadOnlySpan<byte> other, SStringUtf8 @this) => @this.fpMatch(in other, in @this.utf8String);
#pragma warning restore CS1591

    [MethodImpl(HOME.__inline)]
    private static ReadOnlySpan<byte> ToUtf8(ReadOnlySpan<char> utf16)
    {
        var encoding = (UTF8Encoding)Encoding.UTF8;

        int byteCount = encoding.GetByteCount(utf16);

        // align 8-bytes
        byteCount = (byteCount + 7) & -8;

        byte[] buffer = GC.AllocateUninitializedArray<byte>(byteCount, true);

        byte* pFirst = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(buffer));

        fixed (char* charsPtr = &MemoryMarshal.GetReference(utf16))
            byteCount = encoding.GetBytes(charsPtr, utf16.Length, pFirst, byteCount);

        return MemoryMarshal.CreateReadOnlySpan(in *pFirst, byteCount);
    }

#if DEBUG
    internal StringMatchType DebugMatchType => StringMatchHelper.ToEnum(fpMatch);

    internal nuint DebugFNPTR => (nuint)fpMatch;
#endif

    /// <summary>
    /// 아무런 기능이 없는 정적 메소드. 오로지 디버깅을 위해 사용됩니다. (변수가 사용되는 것으로 위장하기 위함)
    /// </summary>
    [System.Diagnostics.Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static void DebugView(scoped ref SStringUtf8 v) { }
}
#endif