namespace fluxiolib.Internal;

internal static class PlatformHelper
{
    public static readonly PlatformID platform;

    public static readonly bool isUnix;
    public static readonly bool isWinNT;

    public static readonly uint CSRTBASE_SIZE;

    unsafe static PlatformHelper()
    {
        var v = Environment.OSVersion.Platform;
        platform = v;

        isUnix = v == PlatformID.Unix;
        isWinNT = v == PlatformID.Win32NT;

        CSRTBASE_SIZE = 
            // T_CRITICAL_SECTION
            (uint)sizeof(RTL_CRST) +            // 40-bytes
            GetPAL_CS_NATIVE_DATA_SIZE() +
            // CsrtBase::m_dwFlags
            sizeof(long);                       // ALIGNMENT
    }

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
}
