using AoC.Tools.Models;

namespace Day10;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static readonly Position MinPos = new(0, 0);
    private static readonly Position MaxPos = new(_input[0].Length - 1, _input.Length - 1);

    private static void Main()
    {
        var trailheads = FindTrailheads();
        Console.WriteLine($"Part 1: {trailheads.Sum(x => x.Value.ToHashSet().Count)}");
        Console.WriteLine($"Part 2: {trailheads.Sum(x => x.Value.Count)}");
    }

    private static Dictionary<Position, List<Position>> FindTrailheads()
    {
        Dictionary<Position, List<Position>> trailheads = [];
        _input.IterateGrid((row, col, c) =>
        {
            if (c == '0')
            {
                var start = new Position(col, row);
                Queue<Position> queue = new([start]);
                trailheads[start] = [];

                while (queue.TryDequeue(out var current))
                {
                    var currentHeight = GetHeight(current);

                    if (currentHeight == '9')
                    {
                        trailheads[start].Add(current);
                        continue;
                    }

                    current.GetNeighbours().ForEach(n =>
                    {
                        if (n.IsBetween(MinPos, MaxPos) && GetHeight(n) == (currentHeight + 1))
                            queue.Enqueue(n);
                    });
                }
            }
        });

        return trailheads;
    }

    private static char GetHeight(Position position) => _input[position.Row][position.Col];
}
