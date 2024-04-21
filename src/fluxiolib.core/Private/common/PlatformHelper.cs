#if NET8_0_OR_GREATER
using fluxiolib.Internal;

namespace fluxiolib;
#else
using System;

namespace fluxiolib {
#endif

/// <summary>
/// 플랫폼과 런타임에 관련된 정보를 가져오는 도우미 클래스
/// </summary>
public static class PlatformHelper
{
    /// <summary>
    /// 현재 실행되는 플랫폼의 ID
    /// </summary>
    public static readonly PlatformID platform;

    /// <summary>
    /// 현재 Unix 계열 운영체제에서 실행되는지 여부
    /// </summary>
    public static readonly bool isUnix;
    /// <summary>
    /// 현재 Win32NT 계열 운영체제에서 실행되는지 여부
    /// </summary>
    public static readonly bool isWinNT;

#if NET8_0_OR_GREATER
    internal static readonly uint CSRTBASE_SIZE;
#endif

    /// <summary>
    /// 현재 Mono 런타임이 사용되고 있는지 여부
    /// </summary>
    public static readonly bool isRunningOnMono;

    unsafe static PlatformHelper()
    {
        var v = Environment.OSVersion.Platform;
        platform = v;

        isUnix = v == PlatformID.Unix;
        isWinNT = v == PlatformID.Win32NT;

#if NET8_0_OR_GREATER
        CSRTBASE_SIZE = 
            // T_CRITICAL_SECTION
            (uint)sizeof(RTL_CRST) +            // 40-bytes
            GetPAL_CS_NATIVE_DATA_SIZE() +
            // CsrtBase::m_dwFlags
            sizeof(long);                       // ALIGNMENT
#endif

        // ref.: https://github.com/ekfvoddl3536/0SuperComicLibs/blob/x-amd64/2SuperComicLib.Runtime.Managed/src/runtime/JITPlatformEnvironment.cs#L38
        isRunningOnMono = Type.GetType("Mono.Runtime") != null;
    }

#if NET8_0_OR_GREATER
    // Test pass:
    //      Windows 11          x64
    //      Ubuntu 22.04        x64
    private static uint GetPAL_CS_NATIVE_DATA_SIZE()
    {
        // ref.: ../inc/corsscomp.h#687
        const string ERR_MSG1 = "Unsupported platform. Requires 64-bit processor and operating system.";
        const string ERR_MSG2 = "Unsupported platform. The runtime environment must be one of OSX, FREEBSD, LINUX, or WINDOWS.";

        if (!Environment.Is64BitProcess)
        {
            // fastfail
            Environment.FailFast(ERR_MSG1);
            return 0;
        }
        
        if (OperatingSystem.IsWindows()) return 0;

        if (OperatingSystem.IsMacOS()) return 120;

        if (OperatingSystem.IsFreeBSD()) return 24;

        if (OperatingSystem.IsLinux())
        {
            var OSArch = RuntimeInformation.OSArchitecture;

            return 
                OSArch == Architecture.Arm64 
                ? 104u
                // LOONGARCH64
                // AMD64
                // S390X
                // RISCV64
                // POWERPC64
                : 96u;
        }

        Environment.FailFast(ERR_MSG2);
        return 0;
    }
#endif
}

#if !NET8_0_OR_GREATER
} // namespace 'fluxiolib'
#endif