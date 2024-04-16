using fluxiolib.Internal;
using System.Diagnostics.CodeAnalysis;

namespace fluxiolib;

static unsafe partial class FluxTool
{
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/paradesc/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/extra_g1/*'/>
    [SkipLocalsInit, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FluxRuntimeFieldDesc GetInstanceField<[DynamicallyAccessedMembers(ACCESS_FIELD)] T>(
        SStringUtf8 utf8Name,
        ProtFlags protection = ProtFlags.Public,
        TypeFlags cortype = TypeFlags.ALL,
        FluxSearchSpace space = default)
    {
        ref readonly MethodTable pMT = ref GetMethodTable(typeof(T));

        return GetInstanceField_OffLen(in pMT, utf8Name, protection, cortype, space);
    }

    /// <include file='0docs/FluxTool.Doc.xml' path='docs/common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/paradesc/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/extra_g2/*'/>
    [SkipLocalsInit, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FluxRuntimeFieldDesc GetInstanceField(
        [DynamicallyAccessedMembers(ACCESS_FIELD)] Type t,
        SStringUtf8 utf8Name,
        ProtFlags protection = ProtFlags.Public,
        TypeFlags cortype = TypeFlags.ALL,
        FluxSearchSpace space = default)
    {
        ArgumentNullException.ThrowIfNull(t);

        if (t.IsGenericTypeDefinition) return default;

        ref readonly MethodTable pMT = ref GetMethodTable(t);

        return GetInstanceField_OffLen(in pMT, utf8Name, protection, cortype, space);
    }

    /// <include file='0docs/FluxTool.Doc.xml' path='docs/common/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/paradesc/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/extra_g3/*'/>
    [SkipLocalsInit, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FluxRuntimeFieldDesc GetInstanceField_Unsafe(
        [DynamicallyAccessedMembers(ACCESS_FIELD)] Type t,
        SStringUtf8 utf8Name,
        ProtFlags protection = ProtFlags.Public,
        TypeFlags cortype = TypeFlags.ALL,
        FluxSearchSpace space = default)
    {
        ref readonly MethodTable pMT = ref GetMethodTable(t);

        (uint index, uint count) = space.GetOffsetAndLength(in pMT);

        return GetInstanceField_Unsafe(in pMT, utf8Name, protection, cortype, index, count);
    }
}
