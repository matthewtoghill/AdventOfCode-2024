using AoC.Tools.Models;

namespace Day12;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        var regions = GetAllRegions(_input.AsCharMap());

        Console.WriteLine($"Part 1: {Part1(regions)}");
        Console.WriteLine($"Part 2: {Part2(regions)}");
    }

    private static int Part1(List<HashSet<Position>> regions)
    {
        var result = 0;

        foreach (var region in regions)
        {
            var area = region.Count;
            var perimeter = region.Sum(p => 4 - p.GetNeighbours().Count(region.Contains));
            result += (area * perimeter);
        }

        return result;
    }

    private static int Part2(List<HashSet<Position>> regions)
    {
        var result = 0;

        foreach (var region in regions)
        {
            var area = region.Count;
            var corners = region.CountCorners();

            result += (area * corners);
        }

        return result;
    }

    private static List<HashSet<Position>> GetAllRegions(Dictionary<Position, char> map)
    {
        HashSet<Position> visited = [];
        List<HashSet<Position>> regions = [];

        foreach (var (pos, _) in map)
        {
            if (!visited.Contains(pos))
            {
                var region = GetRegion(map, pos);
                regions.Add(region);
                region.ForEach(p => visited.Add(p));
            }
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