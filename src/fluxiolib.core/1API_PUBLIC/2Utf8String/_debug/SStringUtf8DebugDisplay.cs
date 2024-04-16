#if DEBUG
namespace fluxiolib;

internal sealed class SStringUtf8DebugDisplay
{
    private readonly nuint _fpMatch;
    private readonly StringMatchType _type;

    public SStringUtf8DebugDisplay(SStringUtf8 utf8)
    {
        _fpMatch = utf8.DebugFNPTR;

        _type = utf8.DebugMatchType;
    }

    public StringMatchType MatchType => _type;

    public nuint FPMatch => _fpMatch;
}
#endif