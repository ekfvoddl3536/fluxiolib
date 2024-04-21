#if NET8_0_OR_GREATER
using System.ComponentModel;

namespace fluxiolib;

/// <summary>
/// 문자열 매칭의 유형을 나타냅니다
/// </summary>
/// <remarks>
/// 이 값은 Flag하지 않으므로, 반드시 하나의 값만 사용하십시오.
/// </remarks>
public enum StringMatchType
{
    /// <summary>
    /// 두 문자열을 기본 비교합니다. 이 방식은 left == right 또는, left.SequentialEqual(right)와 같습니다.
    /// </summary>
    Default,

    /// <summary>
    /// 입력 문자열의 길이가 지정된 문자열의 길이를 초과하는지 검사합니다.
    /// </summary>
    LengthGreaterThan,

    /// <summary>
    /// 입력 문자열의 길이가 지정된 문자열의 길이보다 더 짧은지 검사합니다.
    /// </summary>
    LengthLessThan,

    /// <summary>
    /// 입력 문자열의 길이가 지정된 문자열의 길이와 완전히 동일한 지 검사합니다.
    /// </summary>
    LengthEqual,

    /// <summary>
    /// 입력 문자열에 지정된 문자열이 포함되는지 검사합니다.
    /// </summary>
    Contains,

    /// <summary>
    /// 입력 문자열이 지정된 문자열로 시작되는지 검사합니다.
    /// </summary>
    StartsWith,

    /// <summary>
    /// 입력 문자열이 지정된 문자열로 끝나는지 검사합니다.
    /// </summary>
    EndsWith,
    
    /// <summary>
    /// 지정된 문자열을 무시하고, 모든 입력 문자열을 수락합니다.
    /// </summary>
    AnyAccept,

    /// <summary>
    /// 이 값은 다른 용도로 사용이 예약돼있습니다.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), Obsolete("This value has other uses.", false, DiagnosticId = "FLXLIB0201")]
    Reserved
}
#endif