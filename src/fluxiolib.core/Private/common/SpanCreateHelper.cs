#if NET8_0_OR_GREATER
namespace fluxiolib.Internal;

[SkipLocalsInit]
internal static unsafe class SpanCreateHelper
{
    #region Span
    [MethodImpl(HOME.__inline)]
    public static Span<T> _S<T>(T* p, int length) where T : unmanaged =>
        MemoryMarshal.CreateSpan(ref *p, length);

    [MethodImpl(HOME.__inline)]
    public static Span<T> _S<T>(ref T p, int length) =>
        MemoryMarshal.CreateSpan(ref p, length);
    #endregion

    #region ReadOnlySpan
    [MethodImpl(HOME.__inline)]
    public static ReadOnlySpan<T> _ROS<T>(T[] array, int length) where T : unmanaged =>
        MemoryMarshal.CreateReadOnlySpan(in MemoryMarshal.GetArrayDataReference(array), length);

    [MethodImpl(HOME.__inline)]
    public static ReadOnlySpan<T> _ROS<T>(T* p, int length) where T : unmanaged =>
        MemoryMarshal.CreateReadOnlySpan(in *p, length);

    [MethodImpl(HOME.__inline)]
    public static ReadOnlySpan<T> _ROS<T>(scoped ref readonly T p, int length) where T : unmanaged =>
        MemoryMarshal.CreateReadOnlySpan(in p, length);
    #endregion
}
#endif