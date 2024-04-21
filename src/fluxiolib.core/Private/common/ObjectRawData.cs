#if NET8_0_OR_GREATER
namespace fluxiolib;
#else
namespace fluxiolib {
#endif

internal sealed class ObjectRawData
{
    public byte Data;
}

#if !NET8_0_OR_GREATER
} // namespace 'fluxiolib'
#endif