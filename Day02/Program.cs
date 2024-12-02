namespace Day02;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve(0)}");
        Console.WriteLine($"Part 2: {Solve(1)}");
    }

    private static int Solve(int maxProblems) => _input.Count(x => IsSafe(x.ExtractInts().ToList(), maxProblems));

    private static bool IsSafe(List<int> levels, int maxProblems)
    {
        var differences = levels.EnumerateDifferences().ToList();

        var problemCount = differences.Count(x => x == 0 || x > 3 || x < -3);
        problemCount += differences[0] > 0
            ? differences.Count(x => x < 0)
            : differences.Count(x => x > 0);

        return problemCount <= maxProblems;
    }
}
