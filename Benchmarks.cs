using BenchmarkDotNet.Attributes;

namespace LinuxSleepBenchmarks;

[ShortRunJob]
public class Benchmarks
{
    private const long SleepTimeNanoseconds = 1_000;

    [Benchmark(Baseline = true)]
    public void BusyWait() => SleepUtils.BusyWait(SleepTimeNanoseconds);

    [Benchmark]
    public void NanoSleep() => SleepUtils.NanoSleep(SleepTimeNanoseconds);

    [Benchmark]
    public void FutexWait() => SleepUtils.FutexWait(SleepTimeNanoseconds);
    
    [Benchmark]
    public void HybridWait() => SleepUtils.HybridWait(SleepTimeNanoseconds);
}