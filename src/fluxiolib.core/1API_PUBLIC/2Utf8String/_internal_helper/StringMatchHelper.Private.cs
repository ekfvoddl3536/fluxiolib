#if NET8_0_OR_GREATER
namespace fluxiolib;

static unsafe partial class StringMatchHelper
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool Default(in ReadOnlySpan<byte> l, in ReadOnlySpan<byte> r) => l.SequenceEqual(r);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool LengthAbove(in ReadOnlySpan<byte> l, in ReadOnlySpan<byte> r) => l.Length > r.Length;
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool LengthBelow(in ReadOnlySpan<byte> l, in ReadOnlySpan<byte> r) => l.Length < r.Length;
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool LengthEqual(in ReadOnlySpan<byte> l, in ReadOnlySpan<byte> r) => l.Length == r.Length;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool Contains(in ReadOnlySpan<byte> l, in ReadOnlySpan<byte> r) => l.IndexOf(r) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool StartsWith(in ReadOnlySpan<byte> l, in ReadOnlySpan<byte> r) => l.StartsWith(r);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool EndsWith(in ReadOnlySpan<byte> l, in ReadOnlySpan<byte> r) => l.EndsWith(r);
}
#endif