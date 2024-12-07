namespace Day07;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve("+", "*")}");
        Console.WriteLine($"Part 2: {Solve("+", "*", "||")}");
    }

    private static long Solve(params string[] operators)
    {
        List<long> calibationResults = [];

        foreach (var line in _input)
        {
            var nums = line.ExtractNumeric<long>().ToList();
            var target = nums[0];
            nums = nums.Skip(1).ToList();

            var result = CheckCalculation(nums, target, operators);
            calibationResults.Add(result);
        }

        return calibationResults.Sum();
    }

    private static long CheckCalculation(List<long> nums, long target, string[] operators)
    {
        foreach (var permutation in operators.GeneratePermutations(nums.Count - 1).ToList())
        {
            var result = nums[0];
            var ops = permutation.ToList();
            for (int i = 0; i < ops.Count; i++)
            {
                if (ops[i] == "+") result += nums[i + 1];
                if (ops[i] == "*") result *= nums[i + 1];
                if (ops[i] == "||") result = long.Parse($"{result}{nums[i + 1]}");
            }

            if (result == target)
                return result;
        }

        return 0;
    }
}
