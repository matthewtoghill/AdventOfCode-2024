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

            var corners = 0;
            foreach (var pos in region)
            {
                var north = pos.MoveInDirection('n');
                var east = pos.MoveInDirection('e');
                var south = pos.MoveInDirection('s');
                var west = pos.MoveInDirection('w');
                var northEast = north.MoveInDirection('e');
                var northWest = north.MoveInDirection('w');
                var southEast = south.MoveInDirection('e');
                var southWest = south.MoveInDirection('w');

                // exterior corners
                if (!region.Contains(north) && !region.Contains(east)) corners++;
                if (!region.Contains(north) && !region.Contains(west)) corners++;
                if (!region.Contains(south) && !region.Contains(east)) corners++;
                if (!region.Contains(south) && !region.Contains(west)) corners++;

                // interior corners
                if (region.Contains(north) && region.Contains(east) && !region.Contains(northEast)) corners++;
                if (region.Contains(north) && region.Contains(west) && !region.Contains(northWest)) corners++;
                if (region.Contains(south) && region.Contains(east) && !region.Contains(southEast)) corners++;
                if (region.Contains(south) && region.Contains(west) && !region.Contains(southWest)) corners++;
            }

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