using fluxiolib.Internal;
using System.Reflection;

namespace fluxiolib;

/// <summary>
/// 타입 제약을 포함하여 보다 안전하게 사용가능한 FieldAccessor.
/// </summary>
#if DEBUG
[System.Diagnostics.DebuggerDisplay("{_type.FullName} #{_fdAccess.getDebugString()}")]
#endif
[StructLayout(LayoutKind.Sequential, Pack = sizeof(long))]
public readonly unsafe struct TypedFieldAccessor : IEquatable<TypedFieldAccessor>
{
    private readonly Type _type;
    private readonly UnsafeFieldAccessor _fdAccess;

    #region constructor
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private TypedFieldAccessor(int offset)
    {
        _type = null!;
        _fdAccess = new(offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private TypedFieldAccessor(Type declaringType, FieldDesc* fdDesc)
    {
        _type = declaringType;
        _fdAccess = new((int)fdDesc->Offset);
    }
    #endregion

    #region property
    internal bool IsInvalid => _fdAccess.Offset < 0;
    #endregion

    #region value
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/valueDirect_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ref TField ValueDirect<TStruct, TField>(scoped ref readonly TStruct reference) where TStruct : struct
    {
        ThrowIf(typeof(TStruct));

        return ref _fdAccess.Value<TField>(in Unsafe.As<TStruct, byte>(ref Unsafe.AsRef(in reference)));
    }

    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ref TField Value<TField>(object reference)
    {
        ArgumentNullException.ThrowIfNull(reference);

        ThrowIf(reference.GetType());

        return ref _fdAccess.Value<TField>(reference);
    }

    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_summary/*'/>
    /// <include file='0docs/FieldAccessor.Doc.xml' path='docs/value_common/*'/>
    [CLSCompliant(false), MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public ref TField Value<TField>(TypedReference reference)
    {
        ThrowIf(__reftype(reference));

#pragma warning disable CS8500 // Takes a pointer to a managed type
        ref byte pTypedRef = ref **(byte**)&reference;
#pragma warning restore CS8500

        return
            ref __reftype(reference).IsValueType
            ? ref _fdAccess.Value<TField>(in pTypedRef)
            : ref _fdAccess.Value<TField>(Unsafe.As<byte, object>(ref pTypedRef));
    }
    #endregion

    #region methods
    /// <summary>
    /// <see cref="UnsafeFieldAccessor"/>로 변환합니다.
    /// </summary>
    /// <returns>타입 제약 및 유효성 검사가 포함되지 않는 고속 FieldAccessor인 <see cref="UnsafeFieldAccessor"/> 입니다.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public UnsafeFieldAccessor AsUnsafeAccessor() => _fdAccess;

    public bool Equals(TypedFieldAccessor other) => _type == other._type && _fdAccess == other._fdAccess;
    #endregion

    #region helper methods
    private void ThrowIf(Type otherType)
    {
        System.Diagnostics.Debug.Assert(_fdAccess.Offset >= 0);

        if (_type.IsAssignableFrom(otherType) == false)
            throw new ArgumentException($"TypeMismatch/InvalidCast: current: '{_type.FullName}', other: '{otherType.FullName}'");
    }
    #endregion

    #region override
    public override int GetHashCode() => HashCode.Combine(_type, _fdAccess);
    public override bool Equals(object? obj) => obj is TypedFieldAccessor other && Equals(other);
    #endregion

    #region static methods (Create)
    /// <summary>
    /// 새로운 <see cref="TypedFieldAccessor"/>를 만듭니다.
    /// </summary>
    /// <param name="fdHandle">접근하고자하는 필드의 핸들입니다.</param>
    /// <returns>만들어진 <see cref="TypedFieldAccessor"/> 객체</returns>
    /// <exception cref="ArgumentException"><paramref name="fdHandle"/>가 속한 타입을 알 수 없습니다.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TypedFieldAccessor Create(RuntimeFieldHandle fdHandle)
    {
        var fdDesc = (FieldDesc*)fdHandle.Value;

        ArgumentNullException.ThrowIfNull(fdDesc, nameof(fdHandle));

        var v = Create(fdDesc);
        if (v.IsInvalid)
            throw new ArgumentException("The type with which the field was declared could not be read.", nameof(fdHandle));

        return v;
    }

    /// <summary>
    /// 새로운 <see cref="TypedFieldAccessor"/>를 만듭니다.
    /// </summary>
    /// <param name="fdInfo">접근하고자하는 필드의 정보입니다.</param>
    /// <returns>만들어진 <see cref="TypedFieldAccessor"/> 객체</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TypedFieldAccessor Create(FieldInfo fdInfo)
    {
        ArgumentNullException.ThrowIfNull(fdInfo);
        return Create(fdInfo.FieldHandle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    internal static TypedFieldAccessor Create(FieldDesc* fdDesc)
    {
        var declaringType = FluxRuntimeFieldDesc.GetApproxDeclaringType((nint)fdDesc->pMethodTable);
        return
            declaringType == null
            ? new(-1)
            : new(declaringType, fdDesc);
    }
    #endregion

    #region operator
    public static bool operator ==(TypedFieldAccessor left, TypedFieldAccessor right) => left.Equals(right);
    public static bool operator !=(TypedFieldAccessor left, TypedFieldAccessor right) => !(left == right);
    #endregion
}