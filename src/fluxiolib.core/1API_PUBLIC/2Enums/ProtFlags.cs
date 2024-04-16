namespace fluxiolib;

/// <summary>
/// 필드의 접근 제한자 수준을 나타냅니다.
/// </summary>
[Flags]
public enum ProtFlags
{
    None = 0,

    /// <summary>
    /// 잘못된 접근제한자
    /// </summary>
    Invalid = 1,

    /// <summary>
    /// <see langword="private"/>
    /// </summary>
    Private = 1 << 1,

    /// <summary>
    /// <see langword="private protected"/> -OR- <see langword="protected private"/>
    /// </summary>
    FamilyANDAssembly = 1 << 2,

    /// <summary>
    /// <see langword="internal"/>
    /// </summary>
    Assembly = 1 << 3,

    /// <summary>
    /// <see langword="protected"/>
    /// </summary>
    Family = 1 << 4,

    /// <summary>
    /// <see langword="protected internal"/> -OR- <see langword="internal protected"/>
    /// </summary>
    FamilyORAssembly = 1 << 5,

    /// <summary>
    /// <see langword="public"/>
    /// </summary>
    Public = 1 << 6,

    /// <summary>
    /// Select all
    /// </summary>
    All = Private | FamilyANDAssembly | Assembly | Family | FamilyORAssembly | Public,

    /// <summary>
    /// Select all that is not public.
    /// </summary>
    NonPublic = All & ~Public,

    /// <summary>
    /// Select all that can be accessed in a legal way from outside the assembly.
    /// </summary>
    /// <remarks>
    /// Same as: <see cref="Public"/> | <see cref="FamilyORAssembly"/> | <see cref="Family"/>
    /// </remarks>
    ExternAccessible = Public | FamilyORAssembly | Family,

    /// <summary>
    /// Select all that cannot be accessed in a legal way from outside the assembly.
    /// </summary>
    /// <remarks>
    /// Same as: <see cref="Private"/> | <see cref="FamilyANDAssembly"/> | <see cref="Assembly"/>
    /// </remarks>
    NonExternAccessible = All & ~ExternAccessible
}
