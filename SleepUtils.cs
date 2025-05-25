using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LinuxSleepBenchmarks;

public static unsafe partial class SleepUtils
{
    internal static long NanoSleepOverhead = GetNanoSleepOverhead();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BusyWait(long timeoutNanoseconds)
    {
        var endTime = GetTimeNanoseconds() + timeoutNanoseconds;
        while (GetTimeNanoseconds() < endTime) {}
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NanoSleep(long timeoutNanoseconds)
    {
        var duration = new TimeSpec(timeoutNanoseconds);
        if (NanoSleepNative(&duration, null) < 0)
            throw new Win32Exception(Marshal.GetLastSystemError());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FutexWait(long timeoutNanoseconds)
    {
        var futex = 0;
        FutexWait(ref futex, 0, timeoutNanoseconds);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HybridWait(long timeoutNanoseconds)
    {
        var endTime = GetTimeNanoseconds() + timeoutNanoseconds;
        var sleepWaitTime = timeoutNanoseconds - NanoSleepOverhead;
        if (sleepWaitTime > 0)
            NanoSleep(sleepWaitTime);
        while (GetTimeNanoseconds() < endTime) {}
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long GetTimeNanoseconds()
    {
        Unsafe.SkipInit(out TimeSpec timeSpec);
        if (ClockGetTime(ClockId.Monotonic, &timeSpec) < 0)
            throw new Win32Exception(Marshal.GetLastSystemError());
        return timeSpec.TotalNanoseconds;
    }

    [SkipLocalsInit]
    private static long GetNanoSleepOverhead()
    {
        const long sampleDuration = 1_000;
        const int sampleCount = 100;
        NanoSleep(sampleDuration);
        Span<long> overheadSamples = stackalloc long[sampleCount];
        for (var i = 0; i < sampleCount; ++i)
        {
            var start = GetTimeNanoseconds();
            NanoSleep(sampleDuration);
            overheadSamples[i] = GetTimeNanoseconds() - start - sampleDuration;
        }
        return overheadSamples[sampleCount * 3 / 4] * 4 / 3;
    }

    internal static void UpdateNanoSleepOverhead() => NanoSleepOverhead = GetNanoSleepOverhead();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FutexWait(ref int futex, int value, long timeoutNanoseconds)
    {
        var timeout = new TimeSpec(timeoutNanoseconds);
        fixed (int* futexPtr = &futex)
            if (FutexSyscall(FutexWaitSyscallNumber, futexPtr, FutexOperation.WaitPrivate, value, &timeout) < 0)
            {
                var error = Marshal.GetLastSystemError();
                if (error == 110) // ETIMEDOUT
                    return false;
                throw new Win32Exception(error);
            }
        return true;
    }

    [LibraryImport("libc", EntryPoint = "clock_gettime")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressGCTransition]
    private static partial int ClockGetTime(ClockId clockId, TimeSpec* timeSpec);

    [LibraryImport("libc", EntryPoint = "nanosleep")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressGCTransition]
    private static partial int NanoSleepNative(TimeSpec* duration, TimeSpec* reminder);

    private const long FutexWaitSyscallNumber = 202; // SYS_futex for wait operation

    [LibraryImport("libc", EntryPoint = "syscall")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressGCTransition]
    private static partial int FutexSyscall(long number, int* uaddr, FutexOperation operation, int val, TimeSpec* timeout = null, int* uaddr2 = null, int val3 = 0);

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct TimeSpec
    {
        private const long NanosecondsPerSecond = 1_000_000_000L;

        private readonly long _seconds;
        private readonly long _nanoseconds;

        public long TotalNanoseconds
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _seconds * NanosecondsPerSecond + _nanoseconds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpec(long totalNanoseconds) => _seconds = Math.DivRem(totalNanoseconds, NanosecondsPerSecond, out _nanoseconds);
    }

    private enum FutexOperation : int
    {
        Wait = 0,
        Wake = 1,
        FileDescriptor = 2,
        Requeue = 3,
        CompareRequeue = 4,
        WakeOperation = 5,
        LockPi = 6,
        UnlockPi = 7,
        TryLockPi = 8,
        WaitBitset = 9,
        PrivateFlag = 128,
        ClockRealtime = 256,
        // Common combinations
        WaitPrivate = Wait | PrivateFlag,
        WakePrivate = Wake | PrivateFlag,
    }

    private enum ClockId : int
    {
        // CLOCK_MONOTONIC
        Monotonic = 1
    }
}