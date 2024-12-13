namespace Day13;

public class Program
{
    private static readonly string[] _input = Input.ReadAsParagraphs();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve()}");
        Console.WriteLine($"Part 2: {Solve(10000000000000)}");
    }

    private static long Solve(long correction = 0)
    {
        long total = 0;
        foreach (var group in _input)
        {
            var nums = string.Join(", ", group).ExtractNumeric<long>().ToList();
            XY buttonA = new(nums[0], nums[1]);
            XY buttonB = new(nums[2], nums[3]);
            XY prize = new(nums[4] + correction, nums[5] + correction);

            total += TokensForPrize(buttonA, buttonB, prize);
        }

        return total;
    }

    private static long TokensForPrize(XY a, XY b, XY target)
    {
        long determinant = (a.X * b.Y) - (a.Y * b.X);

        // Calculate the number of presses for each button
        var pressA = ((target.X * b.Y) - (target.Y * b.X)) / determinant;
        var pressB = ((a.X * target.Y) - (a.Y * target.X)) / determinant;

        // Check X and Y values are met and values are not negative
        if (pressA < 0 || pressB < 0) return 0;
        if ((pressA * a.X) + (pressB * b.X) != target.X) return 0;
        if ((pressA * a.Y) + (pressB * b.Y) != target.Y) return 0;

        return (pressA * 3) + pressB;
    }
}

internal record XY (long X, long Y);