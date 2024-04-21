#if NET8_0_OR_GREATER
using System.Diagnostics;

namespace fluxiolib.Internal;

internal static unsafe class TypeHandleFake
{
    [MethodImpl(HOME.__inline)]
    public static bool IsTypeDesc(nint tyHnd) => (tyHnd & 2) != 0;

    [MethodImpl(HOME.__inline)]
    public static MethodTable* AsMethodTable(nint tyHnd)
    {
        Debug.Assert(!IsTypeDesc(tyHnd));

        return (MethodTable*)tyHnd;
    }
}
#endif