namespace Day11;

public class Program
{
    private static readonly string _input = Input.ReadAll();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve(25)}");
        Console.WriteLine($"Part 2: {Solve(75)}");
    }

    private static long Solve(int iterations)
    {
        var stones = _input.ExtractNumeric<long>().CountBy(x => x).ToDictionary(x => x.Key, x => (long)x.Value);

        for (int i = 0; i < iterations; i++)
        {
            foreach (var (key, count) in stones.ToList())
            {
                stones.IncrementAt(key, -count);
                if (stones[key] == 0) stones.Remove(key);

                if (key == 0)
                {
                    stones.IncrementAt(1, count);
                }
                else
                {
                    var digits = key.DigitCount();
                    if (digits % 2 == 0)
                    {
                        var (left, right) = SplitNumber(key, digits);
                        stones.IncrementAt(left, count);
                        stones.IncrementAt(right, count);
                    }
                    else
                    {
                        stones.IncrementAt(key * 2024, count);
                    }
                }
            }
        }

        return stones.Values.Sum();
    }

    private static (long, long) SplitNumber(long num, int digits)
    {
        var divisor = (long)Math.Pow(10, digits / 2);
        return (num / divisor, num % divisor);
    }
}