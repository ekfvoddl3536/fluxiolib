using fluxiolib.Internal;
using System.Reflection;

namespace fluxiolib;

static unsafe partial class FluxTool
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static FluxRuntimeFieldDesc ToRuntimeFieldDesc(RuntimeFieldHandle fdHandle) => new(fdHandle);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static FluxRuntimeFieldDesc ToRuntimeFieldDesc(FieldInfo fdInfo)
    {
        ArgumentNullException.ThrowIfNull(fdInfo);
        return ToRuntimeFieldDesc(fdInfo.FieldHandle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static UnsafeFieldAccessor GetFieldAccessor(RuntimeFieldHandle fdHandle)
    {
        uint offset = ((FieldDesc*)fdHandle.Value)->Offset;
        return new((int)offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static UnsafeFieldAccessor GetFieldAccessor(FieldInfo fdInfo)
    {
        ArgumentNullException.ThrowIfNull(fdInfo);
        return GetFieldAccessor(fdInfo.FieldHandle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TypedFieldAccessor GetSafeFieldAccessor(RuntimeFieldHandle fdHandle) => TypedFieldAccessor.Create(fdHandle);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TypedFieldAccessor GetSafeFieldAccessor(FieldInfo fdInfo) => TypedFieldAccessor.Create(fdInfo);
}
