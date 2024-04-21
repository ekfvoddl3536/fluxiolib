#if NET8_0_OR_GREATER
using fluxiolib.Internal;
using System.Diagnostics.CodeAnalysis;

namespace fluxiolib;

static unsafe partial class FluxTool
{
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/fieldNames_summary/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/extra_g1/*'/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string[] GetFieldNames<[DynamicallyAccessedMembers(ACCESS_FIELD)] T>() => GetFieldNames_Unsafe(typeof(T));

    /// <include file='0docs/FluxTool.Doc.xml' path='docs/fieldNames_summary/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/param_tDesc/*'/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string[] GetFieldNames([DynamicallyAccessedMembers(ACCESS_FIELD)] Type t)
    {
        ArgumentNullException.ThrowIfNull(t);
        return GetFieldNames_Unsafe(t);
    }

    /// <include file='0docs/FluxTool.Doc.xml' path='docs/fieldNames_summary/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/paradesc/*'/>
    /// <include file='0docs/FluxTool.Doc.xml' path='docs/param_tDesc/*'/>
    [SkipLocalsInit]
    public static string[] GetFieldNames_Unsafe(
        [DynamicallyAccessedMembers(ACCESS_FIELD)] Type t,
        ProtFlags protection = ProtFlags.Public,
        TypeFlags cortype = TypeFlags.ALL,
        FluxSearchSpace space = default)
    {
        ref readonly MethodTable pMT = ref GetMethodTable(t);

        var fields = FieldDescList.GetList(in pMT);
        if (fields.Length == 0)
            return [];

        string[] result = new string[fields.Length];

        FieldDesc* pFields = (FieldDesc*)Unsafe.AsPointer(ref MemoryMarshal.GetArrayDataReference(fields));
        for (int i = 0; i < result.Length; ++i)
        {
            string temp = FieldDescList.GetName(pFields + i);

            if (temp.Length == 0) goto _ERROR;

            result[i] = temp;
        }

        return result;

    _ERROR:
        throw new InvalidOperationException("HRESULT -> 0x80131124 // at GetFieldName<T>");
    }
}
#endif