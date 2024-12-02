namespace Day02;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {CountSafe(0)}");
        Console.WriteLine($"Part 2: {CountSafe(1)}");
    }

    private static int CountSafe(int maxProblems)
        => _input.Count(line => line.ExtractInts()
                                    .GetSubsetVariations(maxProblems)
                                    .Any(x => IsSafe(x.EnumerateDifferences())));

    private static bool IsSafe(IEnumerable<int> differences)
        => differences.All(x => x is > 0 and <= 3)
        || differences.All(x => x is < 0 and >= -3);
}
