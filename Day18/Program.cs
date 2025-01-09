using AoC.Tools.Models;

namespace Day18;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Part1()}");
        Console.WriteLine($"Part 2: {Part2()}");
    }

    private static int Part1() => TryReachExit(GetCorruptedBytes().Take(1024).ToHashSet(), out var minSteps) ? minSteps : 0;

    private static string Part2()
    {
        var corruptedBytes = GetCorruptedBytes().ToList();
        int result = BinarySearch(0, _input.Length - 1, x => !TryReachExit(corruptedBytes.Take(x).ToHashSet(), out _));

        return _input[result];
    }

    private static bool TryReachExit(HashSet<Position> corruptedBytes, out int minSteps)
    {
        var start = new Position(0, 0);
        var end = new Position(70, 70);

        PriorityQueue<Position, int> queue = new([(start, 0)]);
        Dictionary<Position, int> positionMinSteps = [];

        minSteps = 0;
        bool canExit = false;

        while (queue.TryDequeue(out var current, out var steps))
        {
            if (current == end)
            {
                canExit = true;
                minSteps = positionMinSteps[end];
                continue;
            }

            foreach (var neighbour in current.GetNeighbours())
            {
                if (corruptedBytes.Contains(neighbour)) continue;
                if (!neighbour.IsBetween(start, end)) continue;

                positionMinSteps.TryAdd(neighbour, int.MaxValue);

                if (positionMinSteps[neighbour] > steps + 1)
                {
                    positionMinSteps[neighbour] = steps + 1;
                    queue.Enqueue(neighbour, steps + 1);
                }
            }
        }

        return canExit;
    }

    private static int BinarySearch(int min, int max, Func<int, bool> predicate)
    {
        while (min < max)
        {
            int mid = min + ((max - min) / 2);
            if (predicate(mid))
                max = mid;
            else
                min = mid + 1;
        }

        return min - 1;
    }

    private static IEnumerable<Position> GetCorruptedBytes()
        => _input.Select(x => x.ExtractInts().ToList()).Select(x => new Position(x[0], x[1]));
}
