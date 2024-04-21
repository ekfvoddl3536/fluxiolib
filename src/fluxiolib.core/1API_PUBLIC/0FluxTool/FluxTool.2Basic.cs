#if NET8_0_OR_GREATER
using fluxiolib.Internal;
using System.Diagnostics.CodeAnalysis;

namespace fluxiolib;

static unsafe partial class FluxTool
{
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/getNumFds/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para_t1/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
    [SkipLocalsInit, MethodImpl(HOME.__inline)]
    public static int GetNumInstanceFields_Unsafe(Type t) => (int)MethodTable.GetNumInstanceFields(in GetMethodTable(t));

    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/getNumFds/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para_tG/*'/>
    [MethodImpl(HOME.__inline)]
    public static int GetNumInstanceFields<T>() => GetNumInstanceFields_Unsafe(typeof(T));

    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/getNumFds/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para_t1/*'/>
    [MethodImpl(HOME.__inline)]
    public static int GetNumInstanceFields(Type t)
    {
        ArgumentNullException.ThrowIfNull(t);
        return GetNumInstanceFields_Unsafe(t);
    }

    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/getInstFdList/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para_t1/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/unsafeAPI/*'/>
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

    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/getInstFdList/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para_t1/*'/>
    public static FluxRuntimeFieldDesc[] GetInstanceFieldList(
        [DynamicallyAccessedMembers(ACCESS_FIELD)] Type t)
    {
        ArgumentNullException.ThrowIfNull(t);

        return GetInstanceFieldList_Unsafe(t);
    }
}
#endif