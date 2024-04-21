#if NET8_0_OR_GREATER
namespace fluxiolib;
#else
using System.Runtime.CompilerServices;

namespace fluxiolib {
#endif

internal static class HOME
{
    public const MethodImplOptions __inline =
#if NET8_0_OR_GREATER
        MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;
#else
        MethodImplOptions.AggressiveInlining;
#endif
}

#if !NET8_0_OR_GREATER
} // namespace 'fluxiolib'
#endif