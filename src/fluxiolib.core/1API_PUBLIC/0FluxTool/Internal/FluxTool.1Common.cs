using fluxiolib.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace fluxiolib;

/// <summary>
/// Provides all the features provided by <see cref="fluxiolib"/>. 
/// </summary>
public static unsafe partial class FluxTool
{
    private const BindingFlags INSTANCE_ALL = 
        BindingFlags.Public | 
        BindingFlags.NonPublic | 
        BindingFlags.Instance;

    private const DynamicallyAccessedMemberTypes ACCESS_FIELD = 
        DynamicallyAccessedMemberTypes.PublicFields | 
        DynamicallyAccessedMemberTypes.NonPublicFields;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    internal static ref readonly MethodTable GetMethodTable(Type t)
    {
        var tyHnd = t.TypeHandle.Value;

        if (TypeHandleFake.IsTypeDesc(tyHnd))
            tyHnd = RuntimeTypeHandle.ToIntPtr(t.TypeHandle);

        ref var result = ref *TypeHandleFake.AsMethodTable(tyHnd);

        var pCannonMT = (long)result.pEEClass & -2L;

        // cmov
        return
            ref ((nint)result.pEEClass & 1) == 0
            ? ref result
            : ref *(MethodTable*)pCannonMT;
    }
}
