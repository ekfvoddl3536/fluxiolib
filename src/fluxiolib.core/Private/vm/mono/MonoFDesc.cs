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
    public readonly IntPtr m_ptr1, m_ptr2, m_ptr3;

    public readonly uint m_dwOffset;
}

#if !NET8_0_OR_GREATER
} // namespace 'fluxiolib.Internal'
#endif