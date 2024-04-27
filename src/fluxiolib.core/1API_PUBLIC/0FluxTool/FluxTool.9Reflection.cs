using fluxiolib.Internal;
using System.Reflection;

#if NET8_0_OR_GREATER
namespace fluxiolib;
#else
using System;
using System.Runtime.CompilerServices;

namespace fluxiolib {
#endif

static unsafe partial class FluxTool
{
#if NET8_0_OR_GREATER
    /// <summary>
    /// <paramref name="fdHandle"/>를 필드 설명자로 변환합니다.
    /// </summary>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para1/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/toRFD_ret/*'/>
    [MethodImpl(HOME.__inline)]
    public static FluxRuntimeFieldDesc ToRuntimeFieldDesc(RuntimeFieldHandle fdHandle) => new(fdHandle);

    /// <summary>
    /// <paramref name="fdInfo"/>를 필드 설명자로 변환합니다.
    /// </summary>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para2/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/toRFD_ret/*'/>
    [MethodImpl(HOME.__inline)]
    public static FluxRuntimeFieldDesc ToRuntimeFieldDesc(FieldInfo fdInfo)
    {
        ArgumentNullException.ThrowIfNull(fdInfo);
        return ToRuntimeFieldDesc(fdInfo.FieldHandle);
    }
#endif

    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/gFA/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para1/*'/>
    [MethodImpl(HOME.__inline)]
    public static UnsafeFieldAccessor GetFieldAccessor(RuntimeFieldHandle fdHandle)
    {
#pragma warning disable
        uint offset = 
            PlatformHelper.isRunningOnMono
            ? ((MonoFDesc*)fdHandle.Value)->Offset
            : ((FieldDesc*)fdHandle.Value)->Offset;

        return new UnsafeFieldAccessor((int)offset);
#pragma warning restore
    }

    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/gFA/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para2/*'/>
    [MethodImpl(HOME.__inline)]
    public static UnsafeFieldAccessor GetFieldAccessor(FieldInfo fdInfo)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(fdInfo);
#else
        if (fdInfo == null) throw new ArgumentNullException(nameof(fdInfo));
#endif
        return GetFieldAccessor(fdInfo.FieldHandle);
    }

#if NET8_0_OR_GREATER
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/gFA/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para1/*'/>
    [MethodImpl(HOME.__inline)]
    public static TypedFieldAccessor GetSafeFieldAccessor(RuntimeFieldHandle fdHandle) => TypedFieldAccessor.Create(fdHandle);

    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/gFA/*'/>
    /// <include file='0docs/FluxTool.Doc_2.xml' path='docs/para2/*'/>
    [MethodImpl(HOME.__inline)]
    public static TypedFieldAccessor GetSafeFieldAccessor(FieldInfo fdInfo) => TypedFieldAccessor.Create(fdInfo);
#endif
}

#if !NET8_0_OR_GREATER
} // namespace 'fluxiolib'
#endif