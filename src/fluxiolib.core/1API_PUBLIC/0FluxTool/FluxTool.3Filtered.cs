#if NET8_0_OR_GREATER
using fluxiolib.Internal;
using System.Diagnostics.CodeAnalysis;

namespace fluxiolib;

static unsafe partial class FluxTool
{
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/fieldList_summary/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/paradesc/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/extra_g1/*'/>
    public static IEnumerable<FluxRuntimeFieldDesc> GetInstanceFieldList<[DynamicallyAccessedMembers(ACCESS_FIELD)] T>(
        ProtFlags protection = ProtFlags.Public,
        TypeFlags cortype = TypeFlags.ALL,
        FluxSearchSpace space = default)
    {
        ref readonly MethodTable pMT = ref GetMethodTable(typeof(T));

        return GetInstanceFieldList_OffLen(in pMT, protection, cortype, space);
    }

    /// <include file='0docs/FluxTool.Doc.xml' path='docs/fieldList_summary/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/paradesc/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/extra_g2/*'/>
    public static IEnumerable<FluxRuntimeFieldDesc> GetInstanceFieldList(
        [DynamicallyAccessedMembers(ACCESS_FIELD)] Type t,
        ProtFlags protection = ProtFlags.Public,
        TypeFlags cortype = TypeFlags.ALL,
        FluxSearchSpace space = default)
    {
        ArgumentNullException.ThrowIfNull(t);

        if (t.IsGenericTypeDefinition) return [];

        ref readonly MethodTable pMT = ref GetMethodTable(t);

        return GetInstanceFieldList_OffLen(in pMT, protection, cortype, space);
    }

    /// <include file='0docs/FluxTool.Doc.xml' path='docs/fieldList_summary/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/paradesc/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/extra_g3/*'/>
    public static IEnumerable<FluxRuntimeFieldDesc> GetInstanceFieldList_Unsafe(
       [DynamicallyAccessedMembers(ACCESS_FIELD)] Type t,
       ProtFlags protection = ProtFlags.Public,
       TypeFlags cortype = TypeFlags.ALL,
       FluxSearchSpace space = default)
    {
        ref readonly MethodTable pMT = ref GetMethodTable(t);

        (uint index, uint count) = space.GetOffsetAndLength(in pMT);

        return GetInstanceFieldList_Unsafe(in pMT, protection, cortype, index, count);
    }
}
#endif