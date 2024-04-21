#if NET8_0_OR_GREATER
#pragma warning disable FLXLIB0201
using fluxiolib.Internal;

namespace fluxiolib;

internal static unsafe partial class StringMatchHelper
{
    public static readonly FNPTR_BUFFER buffer = new([
        _V(&Default),
        _V(&LengthAbove),
        _V(&LengthBelow),
        _V(&LengthEqual),
        _V(&Contains),
        _V(&StartsWith),
        _V(&EndsWith),
        0L // AnyAccept => NULL
    ]);

#if DEBUG
    public static StringMatchType ToEnum(delegate* managed<in ReadOnlySpan<byte>, in ReadOnlySpan<byte>, bool> v) =>
        (StringMatchType)SpanCreateHelper._ROS(in buffer.first, (int)StringMatchType.Reserved).IndexOf((long)v);
#endif

    // macro
    private static long _V(delegate* managed<in ReadOnlySpan<byte>, in ReadOnlySpan<byte>, bool> v) => (long)v;

    [StructLayout(LayoutKind.Sequential, Pack = 16, Size = sizeof(long) * (int)StringMatchType.Reserved)]
    public readonly struct FNPTR_BUFFER
    {
        public readonly long first;

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public FNPTR_BUFFER(ReadOnlySpan<long> items)
        {
            var span = SpanCreateHelper._S(ref first, (int)StringMatchType.Reserved);
            items.CopyTo(span);
        }

        public delegate* managed<in ReadOnlySpan<byte>, in ReadOnlySpan<byte>, bool> this[int index]
        {
            [MethodImpl(HOME.__inline)]
            get => (delegate* managed<in ReadOnlySpan<byte>, in ReadOnlySpan<byte>, bool>)Unsafe.Add(ref Unsafe.AsRef(in first), index);
        }
    }
}
#endif