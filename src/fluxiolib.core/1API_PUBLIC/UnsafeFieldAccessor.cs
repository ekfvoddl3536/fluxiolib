#if NET8_0_OR_GREATER
using XUnsafe = System.Runtime.CompilerServices.Unsafe;

namespace fluxiolib;
#else
using SuperComicLib.Runtime;
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace fluxiolib {
#endif

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

    [MethodImpl(HOME.__inline)]
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
        [MethodImpl(HOME.__inline)]
        get => _fieldOffset;
    }

    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/valueDirect_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
    [MethodImpl(HOME.__inline)]
#if NET8_0_OR_GREATER
    public ref TField ValueDirect<TStruct, TField>(scoped ref readonly TStruct reference) where TStruct : struct =>
#else
    public ref TField ValueDirect<TStruct, TField>(in TStruct reference) where TStruct : struct =>
#endif
        ref Value<TField>(in XUnsafe.As<TStruct, byte>(ref XUnsafe.AsRef(in reference)));

    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
    [MethodImpl(HOME.__inline)]
    public ref TField Value<TField>(object reference) =>
        ref Value<TField>(in XUnsafe.As<ObjectRawData>(reference).Data);

    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
    [MethodImpl(HOME.__inline)]
#if NET8_0_OR_GREATER
    public ref TField Value<TField>(scoped ref readonly byte reference) =>
#else
    public ref TField Value<TField>(in byte reference) =>
#endif
        ref XUnsafe.As<byte, TField>(ref XUnsafe.Add(ref XUnsafe.AsRef(in reference), _fieldOffset));

#pragma warning disable CS1591
    public bool Equals(UnsafeFieldAccessor other) => this == other;

#if NETCOREAPP3_0_OR_GREATER
    public override bool Equals(object? obj) => obj is UnsafeFieldAccessor other && Equals(other);
#else
    public override bool Equals(object obj) => obj is UnsafeFieldAccessor other && Equals(other);
#endif
    public override int GetHashCode() => _fieldOffset;

    public static bool operator ==(UnsafeFieldAccessor left, UnsafeFieldAccessor right) => left._fieldOffset == right._fieldOffset;
    public static bool operator !=(UnsafeFieldAccessor left, UnsafeFieldAccessor right) => left._fieldOffset != right._fieldOffset;
#pragma warning restore CS1591

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

#if !NET8_0_OR_GREATER
} // namespace 'fluxiolib'
#endif