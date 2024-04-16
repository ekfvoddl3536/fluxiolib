using System.Diagnostics;

namespace fluxiolib.Internal;

internal static unsafe class TypeHandleFake
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool IsTypeDesc(nint tyHnd) => (tyHnd & 2) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static MethodTable* AsMethodTable(nint tyHnd)
    {
        Debug.Assert(!IsTypeDesc(tyHnd));

        return (MethodTable*)tyHnd;
    }
}
