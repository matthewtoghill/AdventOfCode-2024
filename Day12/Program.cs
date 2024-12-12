using AoC.Tools.Models;

namespace Day12;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        var regions = GetAllRegions(_input.AsCharMap());

        Console.WriteLine($"Part 1: {regions.Sum(x => x.Count * x.CountPerimeter())}");
        Console.WriteLine($"Part 2: {regions.Sum(x => x.Count * x.CountCorners())}");
    }

    private static List<HashSet<Position>> GetAllRegions(Dictionary<Position, char> map)
    {
        HashSet<Position> visited = [];
        List<HashSet<Position>> regions = [];

        foreach (var (position, _) in map)
        {
            if (visited.Contains(position)) continue;

            var region = GetRegion(map, position);
            regions.Add(region);
            region.ForEach(p => visited.Add(p));
        }

        return regions;
    }

    private static HashSet<Position> GetRegion(Dictionary<Position, char> map, Position start)
    {
        HashSet<Position> region = [];
        Queue<Position> queue = new([start]);
        var key = map[start];

        while (queue.TryDequeue(out var current))
        {
            if (!region.Add(current)) continue;

            current.GetNeighbours().ForEach(n =>
            {
                if (map.GetValueOrDefault(n) == key)
                    queue.Enqueue(n);
            });
        }

        return region;
    }
}