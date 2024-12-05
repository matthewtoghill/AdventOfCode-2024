namespace Day05;

public class Program
{
    private static readonly string[] _input = Input.ReadAsParagraphs();
    private static void Main()
    {
        var rules = ParseRules(_input[0]);
        var lines = _input[1].SplitLines();

        Console.WriteLine($"Part 1: {Solve(rules, lines, true)}");
        Console.WriteLine($"Part 2: {Solve(rules, lines, false)}");
    }

    private static int Solve(Comparer<int> rules, string[] lines, bool isPart1)
        => lines.Sum(x =>
        {
            var nums = x.ExtractInts().ToList();
            var ordered = nums.Order(rules).ToList();
            return nums.Order(rules).SequenceEqual(nums) == isPart1 ? ordered[ordered.Count / 2] : 0;
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