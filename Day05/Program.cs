namespace Day05;

public class Program
{
    private static readonly string[] _input = Input.ReadAsParagraphs();
    private static void Main()
    {
        var rules = ParseRules(_input[0]);
        var lines = _input[1].SplitLines();

        Console.WriteLine($"Part 1: {Part1(rules, lines)}");
        Console.WriteLine($"Part 2: {Part2(rules, lines)}");
    }

    private static int Part1(Comparer<int> rules, string[] lines)
        => lines.Sum(x =>
        {
            var nums = x.ExtractInts().ToList();
            return nums.Order(rules).SequenceEqual(nums) ? nums[nums.Count / 2] : 0;
        });

    private static int Part2(Comparer<int> rules, string[] lines)
        => lines.Sum(x =>
        {
            var nums = x.ExtractInts().ToList();
            var ordered = nums.Order(rules).ToList();
            return !ordered.SequenceEqual(nums) ? ordered[ordered.Count / 2] : 0;
        });

    private static Comparer<int> ParseRules(string input)
    {
        Dictionary<int, HashSet<int>> rules = [];

        foreach (var item in input.SplitLines())
        {
            var nums = item.ExtractInts().ToList();

            if (rules.TryGetValue(nums[0], out var set))
            {
                set.Add(nums[1]);
            }
            else
            {
                rules[nums[0]] = [];
                rules[nums[0]].Add(nums[1]);
            }
        }

        return Comparer<int>.Create((a, b) => rules.GetValueOrDefault(a, []).Contains(b) ? -1 : 1);
    }
}