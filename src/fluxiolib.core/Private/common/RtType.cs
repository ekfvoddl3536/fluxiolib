#if NET8_0_OR_GREATER
#pragma warning disable
namespace fluxiolib;

internal sealed class RtType
{
    // ref.: https://source.dot.net/#System.Private.CoreLib/src/System/RuntimeType.CoreCLR.cs,2377
    public readonly object? keepAlive;
    public readonly nint m_cache;
    public readonly nint m_handle;
}
#endif