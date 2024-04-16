namespace fluxiolib.Internal;

[SkipLocalsInit]
internal static unsafe class SpanCreateHelper
{
    #region Span
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Span<T> _S<T>(T* p, int length) where T : unmanaged =>
        MemoryMarshal.CreateSpan(ref *p, length);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Span<T> _S<T>(ref T p, int length) =>
        MemoryMarshal.CreateSpan(ref p, length);
    #endregion

    #region ReadOnlySpan
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ReadOnlySpan<T> _ROS<T>(T[] array, int length) where T : unmanaged =>
        MemoryMarshal.CreateReadOnlySpan(in MemoryMarshal.GetArrayDataReference(array), length);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ReadOnlySpan<T> _ROS<T>(T* p, int length) where T : unmanaged =>
        MemoryMarshal.CreateReadOnlySpan(in *p, length);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ReadOnlySpan<T> _ROS<T>(scoped ref readonly T p, int length) where T : unmanaged =>
        MemoryMarshal.CreateReadOnlySpan(in p, length);
    #endregion
}
