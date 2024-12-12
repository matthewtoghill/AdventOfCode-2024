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

    public static int CountCorners(this HashSet<Position> positions)
    {
        var corners = 0;

        foreach (var pos in positions)
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
            if (!positions.Contains(north) && !positions.Contains(east)) corners++;
            if (!positions.Contains(north) && !positions.Contains(west)) corners++;
            if (!positions.Contains(south) && !positions.Contains(east)) corners++;
            if (!positions.Contains(south) && !positions.Contains(west)) corners++;

            // interior corners
            if (positions.Contains(north) && positions.Contains(east) && !positions.Contains(northEast)) corners++;
            if (positions.Contains(north) && positions.Contains(west) && !positions.Contains(northWest)) corners++;
            if (positions.Contains(south) && positions.Contains(east) && !positions.Contains(southEast)) corners++;
            if (positions.Contains(south) && positions.Contains(west) && !positions.Contains(southWest)) corners++;
        }

        return corners;
    }
}
