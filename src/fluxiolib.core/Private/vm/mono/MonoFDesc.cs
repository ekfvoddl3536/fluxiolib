#if NET8_0_OR_GREATER
namespace fluxiolib.Internal;
#else
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace fluxiolib.Internal {
#endif

// Mono FieldDesc
[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal readonly unsafe struct MonoFDesc
{
    // https://github.com/mono/mono/blob/9bb01f57a126dab35f070ce238457931e9814c33/mono/metadata/class-internals.h#L146
    // MonoType*
    public readonly IntPtr m_pType;
    // const char*
    public readonly IntPtr m_pName;
    // MonoClass*
    public readonly IntPtr m_pParent;

    public readonly uint m_dwOffset;

    // NOTE:
    //  모노 런타임의 offset은 첫번째 필드가 기준이 아니라 객체 인스턴스의 처음부터이다.
    //  그래서, MethodTable 포인터와 SyncBlock을 건너뛰어야한다
    public uint Offset => m_dwOffset - (uint)(IntPtr.Size + IntPtr.Size);
}

#if !NET8_0_OR_GREATER
} // namespace 'fluxiolib.Internal'
#endif