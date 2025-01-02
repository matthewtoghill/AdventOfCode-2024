namespace Day25;

public static class Program
{
    private static readonly List<string[]> _input = Input.ReadAsParagraphs().Select(x => x.SplitLines()).ToList();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve(ParseSchematics('.', _input), ParseSchematics('#', _input))}");
        Console.WriteLine($"Part 2: Happy Christmas!");
    }

    public static int Solve(IEnumerable<int[]> keys, IEnumerable<int[]> locks)
        => keys.Sum(k => locks.Count(l => Enumerable.Range(0, k.Length).All(i => k[i] + l[i] <= 7)));

    private static List<int[]> ParseSchematics(char c, IEnumerable<string[]> schematics)
        => schematics.Where(x => x[0].StartsWith(c)).Select(GetKeyPattern).ToList();

    private static int[] GetKeyPattern(string[] lines)
        => Enumerable.Range(0, lines[0].Length).Select(x => lines.Count(y => y[x] == '#')).ToArray();
}
