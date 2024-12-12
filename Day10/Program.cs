using AoC.Tools.Models;

namespace Day10;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        var trailheads = FindTrailheads(_input);
        Console.WriteLine($"Part 1: {trailheads.Sum(x => x.Value.ToHashSet().Count)}");
        Console.WriteLine($"Part 2: {trailheads.Sum(x => x.Value.Count)}");
    }

    private static Dictionary<Position, List<Position>> FindTrailheads(string[] input)
    {
        Dictionary<Position, List<Position>> trailheads = [];
        var map = input.AsCharMap();

        foreach (var (start, _) in map.Where(x => x.Value == '0'))
        {
            Queue<Position> queue = new([start]);
            trailheads[start] = [];

            while (queue.TryDequeue(out var current))
            {
                var height = map[current];
                if (height == '9')
                {
                    trailheads[start].Add(current);
                    continue;
                }

                foreach (var neighbour in current.GetNeighbours())
                {
                    if (map.GetValueOrDefault(neighbour) == height + 1)
                        queue.Enqueue(neighbour);
                }
            }
        }

        return trailheads;
    }
}
