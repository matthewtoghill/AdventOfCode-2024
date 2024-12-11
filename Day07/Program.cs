namespace Day07;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve(false)}");
        Console.WriteLine($"Part 2: {Solve(true)}");
    }

    private static long Solve(bool tryConcat)
    {
        long calibrationResult = 0;
        foreach (var line in _input)
        {
            var nums = line.ExtractNumeric<long>().ToList();
            var target = nums[0];
            nums = nums[1..];

            if (CheckCalculation(target, nums[0], nums[1..], tryConcat))
                calibrationResult += target;
        }

        return calibrationResult;
    }

    private static bool CheckCalculation(long target, long current, List<long> nums, bool tryConcat)
    {
        if (current > target) return false;
        if (nums.Count == 0) return current == target;

        return CheckCalculation(target, current + nums[0], nums[1..], tryConcat)
            || CheckCalculation(target, current * nums[0], nums[1..], tryConcat)
            || (tryConcat && CheckCalculation(target, current.Concat(nums[0]), nums[1..], tryConcat));
    }
}
