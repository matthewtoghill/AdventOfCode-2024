using System.Numerics;

namespace AoC.Tools.Models;

public record Pos2D<T> : IComparable<Pos2D<T>> where T : INumber<T>, INumberBase<T>
{
    public T X;
    public T Y;
    public T Row => Y;
    public T Col => X;

    public Pos2D(T x, T y) => (X, Y) = (x, y);
    public Pos2D((T x, T y) pos) => (X, Y) = (pos.x, pos.y);

    public Pos2D<T> MoveNext(char direction, T? distance)
    {
        return char.ToLower(direction) switch
        {
            'u' or '^' or 'n' => new(X, Y - (distance ?? T.One)),
            'd' or 'v' or 's' => new(X, Y + (distance ?? T.One)),
            'l' or '<' or 'w' => new(X - (distance ?? T.One), Y),
            'r' or '>' or 'e' => new(X + (distance ?? T.One), Y),
            _ => new(X, Y),
        };
    }

    public Pos2D<T> Rotate(RotationDegrees degrees = RotationDegrees.Clockwise_90)
        => degrees switch
        {
            RotationDegrees.Clockwise_90 or RotationDegrees.CounterClockwise_270 => new(Y, -X),
            RotationDegrees.Clockwise_270 or RotationDegrees.CounterClockwise_90 => new(-Y, X),
            RotationDegrees._180 => new(-X, -Y),
            _ => new(X, Y),
        };

    public int CompareTo(Pos2D<T>? other)
    {
        if (other is null) return -1;
        var dY = Y - other.Y;
        return dY == T.Zero ? T.Sign(X - other.X) : T.Sign(dY);
    }

    public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
    public override string ToString() => $"({X}, {Y})";

    public void Deconstruct(out T x, out T y)
    {
        x = X;
        y = Y;
    }

    public bool Equals((T X, T Y) other) => X == other.X && Y == other.Y;
    public static bool operator ==(Pos2D<T> a, (T X, T Y) b) => a.Equals(b);
    public static bool operator !=(Pos2D<T> a, (T X, T Y) b) => !a.Equals(b);
    public static Pos2D<T> operator +(Pos2D<T> a, Pos2D<T> b) => new(a.X + b.X, a.Y + b.Y);
    public static Pos2D<T> operator +(Pos2D<T> a, (T X, T Y) b) => new(a.X + b.X, a.Y + b.Y);
    public static Pos2D<T> operator -(Pos2D<T> a) => new(-a.X, -a.Y);
    public static Pos2D<T> operator -(Pos2D<T> a, Pos2D<T> b) => new(a.X - b.X, a.Y - b.Y);
    public static Pos2D<T> operator -(Pos2D<T> a, (T X, T Y) b) => new(a.X - b.X, a.Y - b.Y);
    public static Pos2D<T> operator *(Pos2D<T> a, Pos2D<T> b) => new(a.X * b.X, b.Y * b.Y);
    public static Pos2D<T> operator *(Pos2D<T> a, (T X, T Y) b) => new(a.X * b.X, b.Y * b.Y);
    public static Pos2D<T> operator /(Pos2D<T> a, Pos2D<T> b) => new(a.X / b.X, b.Y / b.Y);
    public static Pos2D<T> operator /(Pos2D<T> a, (T X, T Y) b) => new(a.X / b.X, b.Y / b.Y);
    public static Pos2D<T> operator %(Pos2D<T> a, Pos2D<T> b) => new(a.X % b.X, a.Y % b.Y);
    public static Pos2D<T> operator %(Pos2D<T> a, (T X, T Y) b) => new(a.X % b.X, a.Y % b.Y);

    public T ManhattanDistance(Pos2D<T> other) => (X, Y).ManhattanDistance((other.X, other.Y));
    //public double DirectDistance(Pos2D<T> other) => (X, Y).DirectDistance((other.X, other.Y));
    public T ChessDistance(Pos2D<T> other) => (X, Y).ChessDistance((other.X, other.Y));

    public bool IsBetween(Pos2D<T> min, Pos2D<T> max)
        => min.X <= X && X <= max.X
        && min.Y <= Y && Y <= max.Y;

    public bool IsOutsideBounds(Pos2D<T> min, Pos2D<T> max)
        => min.X > X || X > max.X
        || min.Y > Y || Y > max.Y;

    public IEnumerable<Pos2D<T>> GetNeighbours(bool includeDiagonal = false) => includeDiagonal ? GetAllNeighbours() : GetAdjacent();

    public static readonly (T X, T Y)[] CartesianDirections = [(-T.One, T.Zero), (T.Zero, -T.One), (T.One, T.Zero), (T.Zero, T.One)];
    public static readonly (T X, T Y)[] DiagonalDirections = [(-T.One, -T.One), (-T.One, T.One), (T.One, -T.One), (T.One, T.One)];
    public static readonly (T X, T Y)[] AllDirections = [.. CartesianDirections, .. DiagonalDirections];

    private IEnumerable<Pos2D<T>> GetAdjacent()
    {
        foreach (var dir in CartesianDirections)
            yield return this + dir;
    }

    private IEnumerable<Pos2D<T>> GetAllNeighbours()
    {
        foreach (var dir in AllDirections)
            yield return this + dir;
    }
}
