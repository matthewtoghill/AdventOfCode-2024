using AoC.Tools.Models;

namespace Day06;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static readonly Position MinPos = new(0, 0);
    private static readonly Position MaxPos = new(_input[0].Length - 1, _input.Length - 1);
    private static void Main()
    {
        var (start, obstacles) = ProcessMap(_input);
        var guard = new Guard(start, 'N');

        var route = Part1(guard, obstacles);
        Console.WriteLine($"Part 1: {route.Count}");
        Console.WriteLine($"Part 2: {Part2(guard, obstacles, route)}");
    }

    private static HashSet<Position> Part1(Guard guard, HashSet<Position> obstacles)
    {
        HashSet<Position> visited = [];

        while (true)
        {
            var next = guard.Position.MoveInDirection(guard.Direction);

            if (next.IsOutsideBounds(MinPos, MaxPos))
                break;

            visited.Add(guard.Position);

            guard = obstacles.Contains(next)
                ? new Guard(guard.Position, TurnRight(guard.Direction))
                : new Guard(next, guard.Direction);
        }

        return visited;
    }

    private static int Part2(Guard start, HashSet<Position> obstacles, HashSet<Position> originalRoute)
        => originalRoute.Count(x => CheckLoop([.. obstacles, x], start));

    private static bool CheckLoop(HashSet<Position> obstacles, Guard guard)
    {
        HashSet<Guard> corners = [];
        var lastDir = guard.Direction;

        while (true)
        {
            if (corners.Contains(guard))
                return true;

            if (lastDir != guard.Direction)
            {
                corners.Add(guard);
                lastDir = guard.Direction;
            }

            var next = guard.Position.MoveInDirection(guard.Direction);

            if (next.IsOutsideBounds(MinPos, MaxPos))
                break;

            guard = obstacles.Contains(next)
                ? new Guard(guard.Position, TurnRight(guard.Direction))
                : new Guard(next, guard.Direction);
        }

        return false;
    }

    private static char TurnRight(char direction)
        => direction switch
        {
            'N' => 'E',
            'E' => 'S',
            'S' => 'W',
            'W' => 'N',
            _ => throw new InvalidOperationException()
        };

    private static (Position, HashSet<Position>) ProcessMap(string[] input)
    {
        var start = new Position(0, 0);
        HashSet<Position> obstacles = [];

        for (int row = 0; row < input.Length; row++)
        {
            for (int col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] == '^')
                    start = new Position(col, row);
                else if (input[row][col] == '#')
                    obstacles.Add(new Position(col, row));
            }
        }

        return (start, obstacles);
    }
}

public record Guard(Position Position, char Direction);