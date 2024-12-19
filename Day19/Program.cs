namespace Day19;

public class Program
{
    private static readonly string[] _input = Input.ReadAsParagraphs();
    private static void Main()
    {
        var possibleDesigns = Solve(_input[0].SplitOn(", "), _input[1].SplitLines());
        Console.WriteLine($"Part 1: {possibleDesigns.Count}");
        Console.WriteLine($"Part 2: {possibleDesigns.Values.Sum()}");
    }

    private static Dictionary<string, long> Solve(string[] towels, string[] designs)
        => designs.Select(x => new { Design = x, Count = CalculatePossibleWays(x, towels, []) })
                  .Where(x => x.Count > 0)
                  .ToDictionary(x => x.Design, x => x.Count);

    private static long CalculatePossibleWays(string design, string[] towels, Dictionary<string, long> cache)
    {
        if (string.IsNullOrWhiteSpace(design))
            return 1;

        if (cache.TryGetValue(design, out var count))
            return count;

        long result = towels.Where(design.StartsWith).Sum(x => CalculatePossibleWays(design[x.Length..], towels, cache));

        cache[design] = result;

        return result;
    }
}
