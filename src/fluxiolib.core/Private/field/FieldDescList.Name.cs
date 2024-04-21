#if NET8_0_OR_GREATER
using System.Text;

namespace fluxiolib.Internal;

static unsafe partial class FieldDescList
{
    internal static ReadOnlySpan<byte> GetNameAsSpan(FieldDesc* pFieldDesc)
    {
        nint pImport = ModuleFake.getPEAssembly(pFieldDesc->pMethodTable->pModule)->pMDImport;

        nint szName = 0;

        if (GetNameFieldDescDef(pImport, pFieldDesc->mb, &szName) < 0)
            return default;

        return MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)szName);
    }

    internal static string GetName(FieldDesc* pFieldDesc)
    {
        var text = GetNameAsSpan(pFieldDesc);
        return Encoding.UTF8.GetString(text);
    }

    // NOTE: To make tracking issues simpler, we mark it as NoInline.
    [SkipLocalsInit, MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    internal static int GetNameFieldDescDef(nint pImport, uint mb, nint* pFieldName)
    {
        const int HRESULT_FAIL = unchecked((int)0x80131124);

        uint metaV1 = *(ushort*)(pImport + 0x13A);

        if ((mb &= 0x00FF_FFFF) == 0) goto _ERROR;

        if (mb > *(uint*)(pImport + 0x40)) goto _ERROR;

        ulong metaV2 = *(ulong*)(pImport + 0x130);

        metaV1 *= mb - 1;

        uint indexF2 = *(uint*)(pImport + 0x3C0);

        uint indexL0 = *(byte*)(metaV2 + 0x04);

        ulong indexR1 = metaV1 + *(ulong*)(pImport + 0x3F0);

        indexF2 &= *(uint*)(indexL0 + indexR1);

        if (indexF2 >= *(uint*)(pImport + 0x550)) goto _ERROR;

        ulong result = indexF2 + *(ulong*)(pImport + 0x540);

        *pFieldName = (nint)result;

        return 0;

    _ERROR:
        return HRESULT_FAIL;
    }
}
#endif