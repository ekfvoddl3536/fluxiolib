#if NET8_0_OR_GREATER
namespace fluxiolib.Internal;

// "typedef struct _RTL_CRITICAL_SECTION"
[StructLayout(LayoutKind.Sequential, Pack = sizeof(long))]
internal readonly unsafe struct RTL_CRST
{
    // TARGET_WINDOWS, layout
    public readonly void* DebugInfo;

    public readonly uint LockCount;
    public readonly uint RecursionCount;

    public readonly void* OwningThread;
    public readonly void* LockSemaphore;

    public readonly uint* SpinCount;

    // TARGET_UNIX, layout
    // [FieldOffset(0)]
    // public readonly pthread_mutex_t mutex; // is64Bit: sizeof(pthread_mutex_t) = 40
}
#endif