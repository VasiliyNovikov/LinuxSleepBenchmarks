using System.Runtime.InteropServices;

namespace LinuxSleepBenchmarks;

public static partial class LinuxScheduler
{
    public enum Policy
    {
        Other = 0,
        Fifo = 1,
        RoundRobin = 2,
        Batch = 3,
        Idle = 5,
        Deadline = 6 // Kernel 3.14+
    }

    public static void SetScheduler(int pid, Policy policy, int priority)
    {
        if (pid < 0)
            throw new ArgumentOutOfRangeException(nameof(pid), "PID must be non-negative.");

        if (priority is < 0 or > 99)
            throw new ArgumentOutOfRangeException(nameof(priority), "Priority must be between 0 and 99.");

        var param = new Param { Priority = priority };
        if (SetSchedulerNative(pid, policy, in param) != 0)
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        SleepUtils.UpdateNanoSleepOverhead();
    }

    [LibraryImport("libc", EntryPoint = "sched_setscheduler", SetLastError = true)]
    private static partial int SetSchedulerNative(int pid, Policy policy, in Param param);

    [StructLayout(LayoutKind.Sequential)]
    private struct Param
    {
        public int Priority;
    }
}