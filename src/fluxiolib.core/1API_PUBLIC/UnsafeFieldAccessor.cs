namespace fluxiolib;

/// <summary>
/// 타입 제약을 포함하지 않는 고속 필드 접근자(FieldAccessor) 객체입니다.
/// </summary>
/// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
#if DEBUG
[System.Diagnostics.DebuggerDisplay("{getDebugString()}")]
#endif
[StructLayout(LayoutKind.Sequential, Pack = sizeof(int))]
public readonly struct UnsafeFieldAccessor : IEquatable<UnsafeFieldAccessor>
{
    private readonly int _fieldOffset;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    internal UnsafeFieldAccessor(int fieldOffset) => _fieldOffset = fieldOffset;

    /// <summary>
    /// 이 필드 접근자가 가진 필드 오프셋입니다.
    /// </summary>
    /// <returns>
    /// 필드 오프셋.
    /// 이 필드 접근자가 유효하지 않을 때, 반환 값은 음수입니다.
    /// </returns>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
    public int Offset
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => _fieldOffset;
    }

    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/valueDirect_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ref TField ValueDirect<TStruct, TField>(scoped ref readonly TStruct reference) where TStruct : struct =>
        ref Value<TField>(in Unsafe.As<TStruct, byte>(ref Unsafe.AsRef(in reference)));

    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ref TField Value<TField>(object reference) =>
        ref Value<TField>(in Unsafe.As<ObjectRawData>(reference).Data);

    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ref TField Value<TField>(scoped ref readonly byte reference) =>
        ref Unsafe.As<byte, TField>(ref Unsafe.Add(ref Unsafe.AsRef(in reference), _fieldOffset));

    public bool Equals(UnsafeFieldAccessor other) => this == other;

    public override bool Equals(object? obj) => obj is UnsafeFieldAccessor other && Equals(other);
    public override int GetHashCode() => _fieldOffset;

    public static bool operator ==(UnsafeFieldAccessor left, UnsafeFieldAccessor right) => left._fieldOffset == right._fieldOffset;
    public static bool operator !=(UnsafeFieldAccessor left, UnsafeFieldAccessor right) => left._fieldOffset != right._fieldOffset;

#if DEBUG
    internal string getDebugString()
    {
        string hex =
            _fieldOffset > ushort.MaxValue
            ? _fieldOffset.ToString("X6")
            : _fieldOffset.ToString("X4");

        return $"0x{hex} ({_fieldOffset})";
    }
#endif
}
