using AoC.Tools.Models;

namespace Day04;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Part1()}");
        Console.WriteLine($"Part 2: {Part2()}");
    }

    private static int Part1()
    {
        var minPos = new Position(0, 0);
        var maxPos = new Position(_input.Length - 1, _input[0].Length - 1);
        var count = 0;

        for (int row = 0; row < _input.Length; row++)
        {
            for (int col = 0; col < _input[row].Length; col++)
            {
                if (_input[row][col] != 'X')
                    continue;

                var current = new Position(col, row);
                foreach (var direction in Position.AllDirections)
                {
                    var pos2 = current + direction;
                    var pos3 = pos2 + direction;
                    var pos4 = pos3 + direction;

                    if (pos4.IsOutsideBounds(minPos, maxPos))
                        continue;

                    if (new[] { pos2, pos3, pos4 }.Select(GetLetter).SequenceEqual("MAS"))
                        count++;
                }
            }
        }

        return count;
    }

    private static int Part2()
    {
        var minPos = new Position(0, 0);
        var maxPos = new Position(_input.Length - 1, _input[0].Length - 1);
        var count = 0;

        for (int row = 0; row < _input.Length; row++)
        {
            for (int col = 0; col < _input[row].Length; col++)
            {
                if (_input[row][col] != 'A')
                    continue;

                var current = new Position(col, row);

                var NW = current + (-1, -1);
                var SE = current + (1, 1);

                var SW = current + (1, -1);
                var NE = current + (-1, 1);

                if (new[] { NW, SE, SW, NE }.Any(p => p.IsOutsideBounds(minPos, maxPos)))
                    continue;

                var textA = $"{GetLetter(NW)}{GetLetter(SE)}";
                var textB = $"{GetLetter(SW)}{GetLetter(NE)}";

                if ((textA == "MS" || textA == "SM") && (textB == "MS" || textB == "SM"))
                    count++;
            }
        }

        return count;
    }

    private static char GetLetter(Position pos) => _input[pos.Row][pos.Col];
}