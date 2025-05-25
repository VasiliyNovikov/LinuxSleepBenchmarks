# Testing different kinds of high-resolution waiting on Linux

# 100us default scheduler

| Method     | Mean     | Error    | StdDev  | Ratio | RatioSD |
|----------- |---------:|---------:|--------:|------:|--------:|
| BusyWait   | 101.0 us |  3.01 us | 0.16 us |  1.00 |    0.00 |
| NanoSleep  | 164.2 us | 31.94 us | 1.75 us |  1.63 |    0.02 |
| FutexWait  | 164.0 us |  7.76 us | 0.43 us |  1.62 |    0.00 |
| HybridWait | 101.1 us |  5.93 us | 0.32 us |  1.00 |    0.00 |

NanoSleep overhead: 76109ns

# 100us FIFO scheduler max priority

| Method     | Mean     | Error    | StdDev  | Ratio |
|----------- |---------:|---------:|--------:|------:|
| BusyWait   | 106.8 us |  2.53 us | 0.14 us |  1.00 |
| NanoSleep  | 111.4 us |  9.49 us | 0.52 us |  1.04 |
| FutexWait  | 162.6 us | 18.04 us | 0.99 us |  1.52 |
| HybridWait | 104.5 us | 24.33 us | 1.33 us |  0.98 |

NanoSleep overhead: 8841ns

# 50us default scheduler

| Method     | Mean      | Error     | StdDev   | Ratio | RatioSD |
|----------- |----------:|----------:|---------:|------:|--------:|
| BusyWait   |  50.67 us |  2.860 us | 0.157 us |  1.00 |    0.00 |
| NanoSleep  | 114.09 us | 31.996 us | 1.754 us |  2.25 |    0.03 |
| FutexWait  | 112.38 us | 13.842 us | 0.759 us |  2.22 |    0.01 |
| HybridWait |  50.68 us |  1.937 us | 0.106 us |  1.00 |    0.00 |

Calculated nanosleep overhead: 76206ns

# 50us FIFO scheduler max priority

| Method     | Mean      | Error     | StdDev   | Ratio | RatioSD |
|----------- |----------:|----------:|---------:|------:|--------:|
| BusyWait   |  52.52 us | 32.982 us | 1.808 us |  1.00 |    0.04 |
| NanoSleep  |  59.77 us | 11.623 us | 0.637 us |  1.14 |    0.04 |
| FutexWait  | 110.68 us |  8.831 us | 0.484 us |  2.11 |    0.06 |
| HybridWait |  52.05 us |  4.651 us | 0.255 us |  0.99 |    0.03 |

Calculated nanosleep overhead: 8928 ns

# 10us default scheduler

| Method     | Mean     | Error     | StdDev   | Ratio | RatioSD |
|----------- |---------:|----------:|---------:|------:|--------:|
| BusyWait   | 10.23 us |  0.492 us | 0.027 us |  1.00 |    0.00 |
| NanoSleep  | 71.70 us | 25.613 us | 1.404 us |  7.01 |    0.12 |
| FutexWait  | 70.29 us | 18.699 us | 1.025 us |  6.87 |    0.09 |
| HybridWait | 10.20 us |  0.387 us | 0.021 us |  1.00 |    0.00 |

NanoSleep overhead: 76118ns

# 10us FIFO scheduler max priority

| Method     | Mean     | Error     | StdDev   | Ratio | RatioSD |
|----------- |---------:|----------:|---------:|------:|--------:|
| BusyWait   | 10.68 us |  8.098 us | 0.444 us |  1.00 |    0.05 |
| NanoSleep  | 18.41 us |  4.046 us | 0.222 us |  1.73 |    0.07 |
| FutexWait  | 69.58 us | 11.605 us | 0.636 us |  6.52 |    0.25 |
| HybridWait | 10.70 us |  7.962 us | 0.436 us |  1.00 |    0.05 |

NanoSleep overhead: 2992ns

# 1us default scheduler

| Method     | Mean      | Error     | StdDev    | Ratio | RatioSD |
|----------- |----------:|----------:|----------:|------:|--------:|
| BusyWait   |  1.044 us | 0.0515 us | 0.0028 us |  1.00 |    0.00 |
| NanoSleep  | 60.081 us | 9.6129 us | 0.5269 us | 57.53 |    0.46 |
| FutexWait  | 60.825 us | 4.1444 us | 0.2272 us | 58.25 |    0.23 |
| HybridWait |  1.051 us | 0.0834 us | 0.0046 us |  1.01 |    0.00 |

NanoSleep overhead: 76262ns

# 1us FIFO scheduler max priority

| Method     | Mean      | Error    | StdDev    | Ratio | RatioSD |
|----------- |----------:|---------:|----------:|------:|--------:|
| BusyWait   |  1.084 us | 1.021 us | 0.0560 us |  1.00 |    0.06 |
| NanoSleep  |  5.986 us | 9.363 us | 0.5132 us |  5.53 |    0.48 |
| FutexWait  | 61.032 us | 5.435 us | 0.2979 us | 56.42 |    2.46 |
| HybridWait |  1.116 us | 1.089 us | 0.0597 us |  1.03 |    0.07 |

NanoSleep overhead: 8950ns
