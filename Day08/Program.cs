using AoC.Tools.Models;

namespace Day08;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static readonly Position MinPos = new(0, 0);
    private static readonly Position MaxPos = new(_input[0].Length - 1, _input.Length - 1);
    private static void Main()
    {
        var antennas = GetAntennas(_input);
        Console.WriteLine($"Part 1: {Part1(antennas)}");
        Console.WriteLine($"Part 2: {Part2(antennas)}");
    }

    private static int Part1(HashSet<Antenna> antennas)
    {
        HashSet<Position> antinodes = [];

        var grouped = antennas.GroupBy(x => x.Frequency).ToDictionary(x => x.Key, x => x.Select(y => y.Position).ToList());

        foreach (var (_, positions) in grouped)
        {
            foreach (var pair in positions.GetPermutations(2).Select(x => x.ToList()))
            {
                var antinode = GetAntinode(pair[0], pair[1]);

                if (antinode.IsBetween(MinPos, MaxPos))
                    antinodes.Add(antinode);
            }
        }

        return antinodes.Count;
    }

    private static int Part2(HashSet<Antenna> antennas)
    {
        HashSet<Position> antinodes = [];

        var grouped = antennas.GroupBy(x => x.Frequency).ToDictionary(x => x.Key, x => x.Select(y => y.Position).ToList());

        foreach (var (_, positions) in grouped)
        {
            foreach (var pair in positions.GetPermutations(2).Select(x => x.ToList()))
            {
                GetAllAntinodes(pair[0], pair[1]).ForEach(x => antinodes.Add(x));
            }
        }

        return antinodes.Count;
    }

    private static HashSet<Antenna> GetAntennas(string[] input)
    {
        HashSet<Antenna> antennas = [];
        for (int row = 0; row < input.Length; row++)
        {
            for (int col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] != '.')
                    antennas.Add(new Antenna(new Position(col, row), input[row][col]));
            }
        }

        return antennas;
    }

    public static Position GetAntinode(Position start, Position end)
    {
        var distance = start.DirectDistance(end);
        double directionX = (end.X - start.X) / distance;
        double directionY = (end.Y - start.Y) / distance;

        double newX = end.X + (directionX * distance);
        double newY = end.Y + (directionY * distance);

        return new Position((int)newX, (int)newY);
    }

    public static List<Position> GetAllAntinodes(Position start, Position end)
    {
        List<Position> positions = [start, end];

        while (true)
        {
            var nextPos = GetAntinode(start, end);
            if (nextPos.IsOutsideBounds(MinPos, MaxPos))
                break;

            positions.Add(nextPos);
            start = end;
            end = nextPos;
        }

        return positions;
    }
}

internal record Antenna(Position Position, char Frequency);