namespace AoC.Tools.Models;

public static class PositionExtensions
{
    public static Position[] MoveAllInDirection(this Position[] positions, char direction)
    {
        for (int i = 0; i < positions.Length; i++)
            positions[i] = positions[i].MoveInDirection(direction);

        return positions;
    }

    public static List<Position> MoveAllInDirection(this List<Position> positions, char direction)
    {
        for (int i = 0; i < positions.Count; i++)
            positions[i] = positions[i].MoveInDirection(direction);

        return positions;
    }

    public static int CalculateArea(this List<Position> positions)
    {
        var result = 0;
        var zip = positions.Zip(positions.Skip(1));
        foreach (var (a, b) in zip)
        {
            result += (a.X * b.Y) - (a.Y * b.X);
        }

        return Math.Abs(result);
    }

    public static int CountPositionsInsideArea(this List<Position> positions)
    {
        var count = positions.ToHashSet().Count;
        var area = CalculateArea(positions);
        return 1 + (area - count) / 2;
    }

    public static int CountPerimeter(this List<Position> positions) => positions.ToHashSet().CountPerimeter();
    public static int CountPerimeter(this HashSet<Position> positions) => positions.Sum(p => 4 - p.GetNeighbours().Count(positions.Contains));

    public static int CountCorners(this List<Position> positions) => positions.ToHashSet().CountCorners();
    public static int CountCorners(this HashSet<Position> positions)
    {
        var corners = 0;
        var directionPairs = new List<(char, char)> { ('n', 'e'), ('n', 'w'), ('s', 'e'), ('s', 'w') };
        foreach (var pos in positions)
        {
            foreach (var (dirA, dirB) in directionPairs)
            {
                var a = pos.MoveInDirection(dirA);
                var b = pos.MoveInDirection(dirB);

                if (!positions.Contains(a) && !positions.Contains(b))
                    corners++;

                if (positions.Contains(a) && positions.Contains(b) && !positions.Contains(a.MoveInDirection(dirB)))
                    corners++;
            }
        }

        return corners;
    }
}
