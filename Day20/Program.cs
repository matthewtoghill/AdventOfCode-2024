using AoC.Tools.Models;

namespace Day20;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve(2)}");
        Console.WriteLine($"Part 2: {Solve(20)}");
    }

    private static int Solve(int maxDuration)
    {
        var map = _input.AsCharMap();
        var walls = map.Where(x => x.Value == '#').Select(x => x.Key).ToHashSet();
        var start = map.First(x => x.Value == 'S').Key;
        var end = map.First(x => x.Value == 'E').Key;

        var stepsFromEnd = StepsFromEnd(start, end, walls);

        return stepsFromEnd.Keys.AsParallel()
                                .SelectMany(x => StepsSavedWithCheats(stepsFromEnd, x, maxDuration))
                                .Count(x => x >= 100);
    }

    private static List<int> StepsSavedWithCheats(Dictionary<Position, int> stepsFromEnd, Position start, int maxDuration)
        => stepsFromEnd.Where(x => x.Key.ManhattanDistance(start) <= maxDuration)
                       .Select(x => CalculateStepsSaved(stepsFromEnd, start, x.Key)).ToList();

    private static int CalculateStepsSaved(Dictionary<Position, int> stepsFromEnd, Position start, Position next)
        => stepsFromEnd[next] - (stepsFromEnd[start] + next.ManhattanDistance(start));

    private static Dictionary<Position, int> StepsFromEnd(Position start, Position end, HashSet<Position> walls)
    {
        PriorityQueue<Position, int> queue = new([(end, 0)]);
        Dictionary<Position, int> stepsFromEnd = new() { [end] = 0 };

        while (queue.TryDequeue(out var current, out var steps))
        {
            if (current == start) break;

            foreach (var next in current.GetNeighbours())
            {
                if (walls.Contains(next)) continue;

                stepsFromEnd.TryAdd(next, int.MaxValue);

                if (stepsFromEnd[next] > steps + 1)
                {
                    stepsFromEnd[next] = steps + 1;
                    queue.Enqueue(next, steps + 1);
                }
            }
        }

        return stepsFromEnd;
    }
}