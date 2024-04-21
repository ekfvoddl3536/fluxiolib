#if NET8_0_OR_GREATER
namespace fluxiolib.Internal;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal readonly unsafe struct LookupMap
{
    public readonly nint a, b, c, d;
}
#endif