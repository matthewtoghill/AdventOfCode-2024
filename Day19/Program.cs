namespace Day19;

public class Program
{
    private static readonly string[] _input = Input.ReadAsParagraphs();
    private static void Main()
    {
        var possibleDesigns = Solve();
        Console.WriteLine($"Part 1: {possibleDesigns.Count}");
        Console.WriteLine($"Part 2: {possibleDesigns.Values.Sum()}");
    }

    private static Dictionary<string, long> Solve()
    {
        Dictionary<string, long> possibleDesigns = [];
        var towels = _input[0].SplitOn(", ");

        foreach (var design in _input[1].SplitLines())
        {
            var count = CalculatePossibleWays(design, towels, []);
            if (count > 0)
                possibleDesigns[design] = count;
        }

        return possibleDesigns;
    }

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
