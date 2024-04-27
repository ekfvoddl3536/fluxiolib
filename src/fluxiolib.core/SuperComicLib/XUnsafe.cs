// .NET Standard 2.0 Features
#if !NET8_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace SuperComicLib.Runtime
{
    /// <summary>
    /// The built-in 
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe">System.Runtime.CompilerServices.Unsafe</see> class 
    /// includes only some of the functionalities of 
    /// <see href="https://github.com/ekfvoddl3536/0SuperComicLibs/blob/x-amd64/1SuperComicLib.Runtime/src/Unsafe/ILUnsafe.cs">SuperComicLib.Runtime.ILUnsafe</see>. 
    /// </summary>
    /// <remarks>
    /// If possible, use APIs that support more features like 
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe">System.Runtime.CompilerServices.Unsafe</see>
    /// or 
    /// <see href="https://github.com/ekfvoddl3536/0SuperComicLibs/blob/x-amd64/1SuperComicLib.Runtime/src/Unsafe/ILUnsafe.cs">SuperComicLib.Runtime.ILUnsafe</see>.
    /// </remarks>
    public static unsafe class XUnsafe
    {
        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe.asref?view=net-8.0#system-runtime-compilerservices-unsafe-asref-1(-0@)"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining, MethodCodeType = MethodCodeType.Runtime)]
        public static extern ref T AsRef<T>(in T reference);

        /// <summary>
        /// <see href=" https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe.as?view=net-8.0"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining, MethodCodeType = MethodCodeType.Runtime)]
        public static extern T As<T>(object reference) where T : class;

        /// <summary>
        /// <see href=" https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe.as?view=net-8.0"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining, MethodCodeType = MethodCodeType.Runtime)]
        public static extern ref TTo As<TFrom, TTo>(ref TFrom reference);

        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe.add?view=net-8.0#system-runtime-compilerservices-unsafe-add-1(-0@-system-int32)"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining, MethodCodeType = MethodCodeType.Runtime)]
        public static extern ref T Add<T>(ref T reference, int offset);
    }
}
#endif
