using System.ComponentModel;

namespace fluxiolib;

/// <summary>
/// 필드의 CorElementType 유형을 나타냅니다.
/// </summary>
[Flags]
public enum TypeFlags
{
    None = 0,

    /// <summary>
    /// 잘못된 유형
    /// </summary>
    INVALID = 1,

    /// <summary>
    /// <see cref="bool"/>. "System.Boolean"
    /// </summary>
    BOOLEAN = 1 << 0x02,

    /// <summary>
    /// <see cref="char"/>. "System.Char"
    /// </summary>
    CHAR = 1 << 0x03,

    /// <summary>
    /// <see cref="sbyte"/>. "System.SByte"
    /// </summary>
    I1 = 1 << 0x04,
    /// <summary>
    /// <see cref="byte"/>. "System.Byte"
    /// </summary>
    U1 = 1 << 0x05,

    /// <summary>
    /// <see cref="short"/>. "System.Int16"
    /// </summary>
    I2 = 1 << 0x06,
    /// <summary>
    /// <see cref="ushort"/>. "System.Int16"
    /// </summary>
    U2 = 1 << 0x07,

    /// <summary>
    /// <see cref="int"/>. "System.Int32"
    /// </summary>
    I4 = 1 << 0x08,
    /// <summary>
    /// <see cref="uint"/>. "System.Int32"
    /// </summary>
    U4 = 1 << 0x09,

    /// <summary>
    /// <see cref="long"/>. "System.Int64"
    /// </summary>
    I8 = 1 << 0x0A,
    /// <summary>
    /// <see cref="ulong"/>. "System.UInt64"
    /// </summary>
    U8 = 1 << 0x0B,

    /// <summary>
    /// <see cref="float"/>. "System.Single"
    /// </summary>
    R4 = 1 << 0x0C,
    /// <summary>
    /// <see cref="double"/>. "System.Double"
    /// </summary>
    R8 = 1 << 0x0D,

    /// <summary>
    /// pointer
    /// </summary>
    PTR = 1 << 0x0F,

    /// <summary>
    /// <see langword="struct"/> types
    /// </summary>
    VALUETYPE = 1 << 0x11,
    /// <summary>
    /// <see langword="class"/> types.
    /// </summary>
    CLASS = 1 << 0x12,

    /// <summary>
    /// <see cref="nint"/>. native int. "System.IntPtr"
    /// </summary>
    I = 1 << 0x18,
    /// <summary>
    /// <see cref="nuint"/>. native uint. "System.UIntPtr"
    /// </summary>
    U = 1 << 0x19,
    /// <summary>
    /// <see langword="delegate"/>*. delegate pointer (function pointer)
    /// </summary>
    FNPTR = 1 << 0x1B,

    /// <summary>
    /// All types that satisfy '<see langword="sizeof"/>(T) == 1'
    /// </summary>
    AllSizeOf1 = I1 | U1 | BOOLEAN,

    /// <summary>
    /// All types that satisfy '<see langword="sizeof"/>(T) == 2'
    /// </summary>
    AllSizeOf2 = I2 | U2 | CHAR,

    /// <summary>
    /// All types that satisfy '<see langword="sizeof"/>(T) == 4'
    /// </summary>
    AllSizeOf4 = I4 | U4 | R4,

    /// <summary>
    /// All types that satisfy '<see langword="sizeof"/>(T) == 8'
    /// </summary>
    AllSizeOf8 = I8 | U8 | R8,

    /// <summary>
    /// All types that satisfy '<see langword="sizeof"/>(T) == <see langword="sizeof"/>(<see cref="nint"/>)'
    /// </summary>
    AllSizeOfPTR = I | U | FNPTR | PTR,

    /// <summary>
    /// All primitive types. (e..g: byte, char, int, long, float, double, nint...)
    /// </summary>
    /// <remarks>
    /// The .NET built-in reference types(<see cref="string"/> and <see cref="object"/>) are not primitive.
    /// </remarks>
    Primitive = 
        AllSizeOf1 | AllSizeOf2 | AllSizeOf4 | AllSizeOf8 |
        I | U,

    /// <summary>
    /// All unmanaged types.
    /// </summary>
    /// <remarks>
    /// Includes all primitive types and pointer types.
    /// </remarks>
    AllUnmanaged = Primitive | FNPTR | PTR,

    /// <summary>
    /// All managed types.
    /// </summary>
    /// <remarks>
    /// Same as: '<see cref="CLASS"/> | <see cref="VALUETYPE"/>'
    /// </remarks>
    AllManaged = CLASS | VALUETYPE,

    /// <summary>
    /// All value types.
    /// </summary>
    /// <remarks>
    /// Same as: '<see cref="AllUnmanaged"/> | <see cref="VALUETYPE"/>'
    /// </remarks>
    AllValuetype = AllUnmanaged | VALUETYPE,

    /// <summary>
    /// All types
    /// </summary>
    ALL = AllUnmanaged | AllManaged,

    /// <summary>
    /// Selects any type, including hidden types. (Not a recommended option)
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), Obsolete("not recommended use", false, DiagnosticId = "FLXLIB0001")]
    CorElementType_TYPE_ANY = -1,
}
