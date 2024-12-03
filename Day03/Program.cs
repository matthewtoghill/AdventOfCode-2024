using System.Text.RegularExpressions;

namespace Day03;

public partial class Program
{
    private static readonly string _input = Input.ReadAll();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve(Part1Regex())}");
        Console.WriteLine($"Part 2: {Solve(Part2Regex())}");
    }

    private static int Solve(Regex regex)
    {
        bool isEnabled = true;
        int result = 0;

        foreach (Match item in regex.Matches(_input))
        {
            switch (item.Value[..3])
            {
                case "do(":
                    isEnabled = true;
                    break;
                case "don":
                    isEnabled = false;
                    break;
                case "mul":
                    if (!isEnabled) continue;
                    result += int.Parse(item.Groups[1].Value) * int.Parse(item.Groups[2].Value);
                    break;
            }
        }

        return result;
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex Part1Regex();

    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)")]
    private static partial Regex Part2Regex();
}
