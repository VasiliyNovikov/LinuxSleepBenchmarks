using BenchmarkDotNet.Running;
using LinuxSleepBenchmarks;

LinuxScheduler.SetScheduler(Environment.ProcessId, LinuxScheduler.Policy.Fifo, 99);

BenchmarkRunner.Run<Benchmarks>();
Console.WriteLine($"NanoSleep overhead: {SleepUtils.NanoSleepOverhead}ns");