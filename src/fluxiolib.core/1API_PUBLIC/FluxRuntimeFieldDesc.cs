#if NET8_0_OR_GREATER
using fluxiolib.Internal;
using System.Text;

namespace fluxiolib;

/// <summary>
/// <see cref="RuntimeFieldHandle"/>의 핸들로 부터 필드 정보를 간단하게 서술합니다.
/// </summary>
#if DEBUG
[System.Diagnostics.DebuggerDisplay("{getDebugString()}")]
#endif
public readonly unsafe struct FluxRuntimeFieldDesc : IEquatable<FluxRuntimeFieldDesc>
{
    private readonly FieldDesc m_fieldDesc;

    /// <summary>
    /// 새로운 <see cref="FluxRuntimeFieldDesc"/> 구조체를 초기화합니다.
    /// </summary>
    /// <param name="fdHandle">이 구조체를 초기화할 때 사용할 필드 핸들</param>
    public FluxRuntimeFieldDesc(RuntimeFieldHandle fdHandle)
    {
        nint ptr = fdHandle.Value;
        ArgumentNullException.ThrowIfNull((void*)ptr, nameof(fdHandle));

        m_fieldDesc = *(FieldDesc*)ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FluxRuntimeFieldDesc(FieldDesc* pFieldDesc)
    {
        System.Diagnostics.Debug.Assert(pFieldDesc != null);

        m_fieldDesc = *pFieldDesc;
    }

    /// <summary>
    /// 이 구조체가 <see langword="null"/>을 나타내는지 여부를 가져옵니다.
    /// </summary>
    public bool IsNull
    {
        [MethodImpl(HOME.__inline)]
        get => m_fieldDesc.pMethodTable == null;
    }

    /// <summary>
    /// 이 필드의 CorElementType을 가져옵니다.
    /// </summary>
    public TypeFlags FieldCorType => (TypeFlags)(1u << (int)m_fieldDesc.Type);

    /// <summary>
    /// 이 필드의 접근제한자 수준을 가져옵니다.
    /// </summary>
    public ProtFlags FieldAccessModifier => (ProtFlags)(1u << (int)m_fieldDesc.Prot);

    /// <summary>
    /// 이 필드의 오프셋을 가져옵니다. <see cref="IsNull"/> = <see langword="true"/> 일 경우, <c>-1</c> 입니다.
    /// </summary>
    public int FieldOffset
    {
        [MethodImpl(HOME.__inline)]
        get
        {
            int x = -1;
            int y = (int)m_fieldDesc.Offset;

            return
                IsNull
                ? x
                : y;
        }
    }

    /// <summary>
    /// 이 필드에 대한 <see cref="UnsafeFieldAccessor"/>를 가져옵니다.
    /// <see cref="IsNull"/> = <see langword="true"/> 일 경우, 오프셋은 <c>-1</c> 입니다.
    /// </summary>
    public UnsafeFieldAccessor FieldAccessor => new(FieldOffset);

    /// <summary>
    /// 이 필드의 메소드 테이블 포인터를 가져옵니다.
    /// </summary>
    public nint ApproxEnclosingMethodTable => (nint)m_fieldDesc.pMethodTable;

    /// <summary>
    /// 이 필드가 선언된 유형(Type)을 가져옵니다.
    /// </summary>
    public Type? ApproxDeclaringType => GetApproxDeclaringType(ApproxEnclosingMethodTable);

    /// <summary>
    /// 이 필드의 이름을 캐싱하지 않고, <see cref="ReadOnlySpan{T}"/> 유형으로 읽습니다.
    /// </summary>
    /// <returns>UTF8 인코딩된 필드 이름입니다. 이름을 가져오는데에 실패한 경우 길이는 0입니다.</returns>
    public ReadOnlySpan<byte> GetUtf8NameAsSpanWithoutCaching()
    {
        ArgumentNullException.ThrowIfNull(m_fieldDesc.pMethodTable, "this");

        FieldDesc fdDesc = m_fieldDesc;

        return FieldDescList.GetNameAsSpan(&fdDesc);
    }

    /// <summary>
    /// 이 필드의 이름을 캐싱하지 않고, UTF8 인코딩을 사용하여 <see cref="string"/>으로 변환하여 읽습니다.
    /// </summary>
    /// <returns>이 필드의 이름입니다. 이름을 가져오는데에 실패한 경우 길이는 0입니다.</returns>
    public string GetNameWithoutCaching() => Encoding.UTF8.GetString(GetUtf8NameAsSpanWithoutCaching());

    /// <summary>
    /// 이 필드에 대한 타입 제약이 포함된 데이터 접근자를 가져옵니다.
    /// </summary>
    /// <returns>타입 제약이 포함된 데이터 접근자 입니다.</returns>
    /// <exception cref="InvalidOperationException"><see cref="ApproxDeclaringType"/>이 null이거나 잘못된 형식(Type)이면 이 메소드가 실패하고, 예외가 발생합니다.</exception>
    public TypedFieldAccessor GetSafeFieldAccessor()
    {
        ArgumentNullException.ThrowIfNull(m_fieldDesc.pMethodTable, "this");

        FieldDesc fdDesc = m_fieldDesc;

        var v = TypedFieldAccessor.Create(&fdDesc);
        if (v.IsInvalid)
            throw new InvalidOperationException($"{nameof(ApproxDeclaringType)} is null.");

        return v;
    }

#pragma warning disable CS1591
    public bool Equals(FluxRuntimeFieldDesc other) => FieldDesc.Equals(in m_fieldDesc, in other.m_fieldDesc);

    public override bool Equals(object? obj) => obj is FluxRuntimeFieldDesc other && Equals(other);
    public override int GetHashCode() => FieldDesc.GetHashCode(in m_fieldDesc);

    public static bool operator ==(FluxRuntimeFieldDesc left, FluxRuntimeFieldDesc right) => left.Equals(right);
    public static bool operator !=(FluxRuntimeFieldDesc left, FluxRuntimeFieldDesc right) => !(left == right);
#pragma warning restore CS1591

    [MethodImpl(HOME.__inline)]
    internal static Type? GetApproxDeclaringType(nint pMethodTable) =>
        Type.GetTypeFromHandle(RuntimeTypeHandle.FromIntPtr(pMethodTable));

#if DEBUG
    internal string getDebugString() =>
        $"Type: {FieldCorType}, Offset: {FieldAccessor.getDebugString()}, Name: {GetNameWithoutCaching()}";
#endif
}
#endif