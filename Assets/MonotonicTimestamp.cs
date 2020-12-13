// https://antonymale.co.uk/monotonic-timestamps-in-csharp.html
using System;
using System.Runtime.InteropServices;

public struct MonotonicTimestamp
{
    private static double tickFrequency;
    private static double counterFrequency;

    private long timestamp;

    static MonotonicTimestamp()
    {
        long frequency;
        bool succeeded = NativeMethods.QueryPerformanceFrequency(out frequency);
        if (!succeeded)
        {
            throw new PlatformNotSupportedException("Requires Windows XP or later");
        }

        tickFrequency = (double)TimeSpan.TicksPerSecond / frequency;
        counterFrequency = (double)frequency;
    }

    private MonotonicTimestamp(long timestamp)
    {
        this.timestamp = timestamp;
    }
    
    public double Seconds() {
        return ((double)timestamp)/counterFrequency;
    }

    public static MonotonicTimestamp Now()
    {
        long value;
        NativeMethods.QueryPerformanceCounter(out value);
        return new MonotonicTimestamp(value);
    }

    private static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long value);

        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(out long value);
    }
}