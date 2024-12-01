﻿namespace AoC.Tools;

public static class NumericExtensions
{
    public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable<T>
    {
        return min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;
    }

    public static int Mod(this int num, int divisor) => ((num %= divisor) < 0) ? num + divisor : num;
    public static long Mod(this long num, long divisor) => ((num %= divisor) < 0) ? num + divisor : num;

    public static long GreatestCommonFactor(int x, int y)
    {
        var b = Math.Max(x, y);
        var r = Math.Min(x, y);

        while (r != 0)
        {
            var a = b;
            b = r;
            r = a % b;
        }

        return b;
    }

    public static long GreatestCommonFactor(long x, long y)
    {
        var b = Math.Max(x, y);
        var r = Math.Min(x, y);

        while (r != 0)
        {
            var a = b;
            b = r;
            r = a % b;
        }

        return b;
    }

    public static long LowestCommonMultiple(int x, int y) => x / GreatestCommonFactor(x, y) * y;
    public static long LowestCommonMultiple(long x, long y) => x / GreatestCommonFactor(x, y) * y;

    public static long LowestCommonMultiple(this IEnumerable<int> items)
    {
        var uniqueValues = new HashSet<int>(items).ToArray();

        if (uniqueValues.Length == 0) return 0;
        if (uniqueValues.Length == 1) return uniqueValues[0];

        var current = LowestCommonMultiple(uniqueValues[0], uniqueValues[1]);

        for (int i = 2; i < uniqueValues.Length; i++)
        {
            current = LowestCommonMultiple(current, uniqueValues[i]);
        }

        return current;
    }
}
