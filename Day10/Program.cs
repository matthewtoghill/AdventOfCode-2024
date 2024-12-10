using AoC.Tools.Models;

namespace Day10;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static readonly Position MinPos = new(0, 0);
    private static readonly Position MaxPos = new(_input[0].Length - 1, _input.Length - 1);
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Solve(true)}");
        Console.WriteLine($"Part 2: {Solve(false)}");
    }

    private static int Solve(bool uniqueTrailEnds)
    {
        Dictionary<Position, List<Position>> trailheads = [];
        _input.IterateGrid((row, col, c) =>
        {
            if (c.ToInt() == 0)
            {
                var start = new Position(col, row);
                Queue<Position> queue = new([start]);
                trailheads[start] = [];

                while (queue.TryDequeue(out var current))
                {
                    var currentHeight = GetHeight(current);

                    if (currentHeight == 9)
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

        return trailheads.Sum(x => uniqueTrailEnds ? x.Value.ToHashSet().Count : x.Value.Count);
    }

    private static int GetHeight(Position position) => _input[position.Row][position.Col].ToInt();
}
