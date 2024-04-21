#if NET8_0_OR_GREATER
namespace fluxiolib.Internal;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal readonly unsafe struct PEAssemblyFake
{
    public readonly nint p1; // unknown pointer
    public readonly nint p2; // unknown pointer
    public readonly nint p3; // unknown pointer

    // MD = Metadata
    public readonly nint pMDImport;
}
#endif