using fluxiolib.Internal;
using System.Diagnostics.CodeAnalysis;

namespace fluxiolib;

static unsafe partial class FluxTool
{
    /// <summary>
    /// 필드 개수를 가져옵니다.
    /// </summary>
    /// <remarks>
    /// <see cref="Unsafe">이 API는 매개변수의 유효성 검사를 수행하지 않습니다.</see>
    /// </remarks>
    /// <param name="t">정보를 읽을 타입입니다.</param>
    /// <returns><paramref name="t"/> 형식이 갖고 있는 필드의 개수입니다.</returns>
    [SkipLocalsInit, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int GetNumInstanceFields_Unsafe(Type t) => (int)MethodTable.GetNumInstanceFields(in GetMethodTable(t));

    /// <summary>
    /// 필드 개수를 가져옵니다.
    /// </summary>
    /// <typeparam name="T">정보를 읽을 타입입니다.</typeparam>
    /// <returns><typeparamref name="T"/> 형식이 갖고 있는 필드의 개수입니다.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int GetNumInstanceFields<T>() => GetNumInstanceFields_Unsafe(typeof(T));

    /// <summary>
    /// 필드 개수를 가져옵니다.
    /// </summary>
    /// <param name="t">정보를 읽을 타입 객체입니다.</param>
    /// <returns><paramref name="t"/> 형식이 갖고 있는 필드의 개수입니다.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int GetNumInstanceFields(Type t)
    {
        ArgumentNullException.ThrowIfNull(t);
        return GetNumInstanceFields_Unsafe(t);
    }

    public static FluxRuntimeFieldDesc[] GetInstanceFieldList_Unsafe(
        [DynamicallyAccessedMembers(ACCESS_FIELD)] Type t)
    {
        ref readonly MethodTable pMT = ref GetMethodTable(t);

        var buffer = FieldDescList.GetList(in pMT);
        if (buffer.Length == 0)
            return [];

        var result = GC.AllocateUninitializedArray<FluxRuntimeFieldDesc>(buffer.Length, true);

        var pBuffer = (FieldDesc*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(buffer));
        for (int i = 0; i < result.Length; ++i)
            result[i] = new(pBuffer + i);

        return result;
    }

    public static FluxRuntimeFieldDesc[] GetInstanceFieldList(
        [DynamicallyAccessedMembers(ACCESS_FIELD)] Type t)
    {
        ArgumentNullException.ThrowIfNull(t);

        return GetInstanceFieldList_Unsafe(t);
    }
}
