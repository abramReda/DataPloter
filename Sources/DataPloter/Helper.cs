using System;
using System.Collections.Generic;
using System.Text;

namespace DataPloter;

internal static class Helper
{
    public static bool IsInRange(double value, (double start, double end) Range) => value >= Range.start && value <= Range.end;

    public static double GetInRange(double value, (double start, double end) Range)
    {
        return Math.Min(Range.end, Math.Max(value, Range.start));
    }
}
