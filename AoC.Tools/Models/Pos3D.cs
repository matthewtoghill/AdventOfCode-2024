using System.Numerics;

namespace AoC.Tools.Models;

public record Pos3D<T> : IComparable<Pos3D<T>> where T : INumber<T>, INumberBase<T>
{
    public readonly T X;
    public readonly T Y;
    public readonly T Z;

    public Pos3D(T x, T y, T z) => (X, Y, Z) = (x, y, z);
    public Pos3D((T x, T y, T z) pos) => (X, Y, Z) = (pos.x, pos.y, pos.z);

    public int CompareTo(Pos3D<T>? other)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());
    public override string ToString() => $"({X}, {Y}, {Z})";

    public void Deconstruct(out T x, out T y, out T z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    public bool Equals((T X, T Y, T Z) other) => X == other.X && Y == other.Y && Z == other.Z;
    public static bool operator ==(Pos3D<T> a, (T X, T Y, T Z) b) => a.Equals(b);
    public static bool operator !=(Pos3D<T> a, (T X, T Y, T Z) b) => !a.Equals(b);
    public static Pos3D<T> operator +(Pos3D<T> a, Pos3D<T> b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Pos3D<T> operator +(Pos3D<T> a, (T X, T Y, T Z) b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Pos3D<T> operator -(Pos3D<T> a) => new(-a.X, -a.Y, -a.Z);
    public static Pos3D<T> operator -(Pos3D<T> a, Pos3D<T> b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Pos3D<T> operator -(Pos3D<T> a, (T X, T Y, T Z) b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Pos3D<T> operator *(Pos3D<T> a, Pos3D<T> b) => new(a.X * b.X, b.Y * b.Y, a.Z * b.Z);
    public static Pos3D<T> operator *(Pos3D<T> a, (T X, T Y, T Z) b) => new(a.X * b.X, b.Y * b.Y, a.Z * b.Z);
    public static Pos3D<T> operator /(Pos3D<T> a, Pos3D<T> b) => new(a.X / b.X, b.Y / b.Y, a.Z / b.Z);
    public static Pos3D<T> operator /(Pos3D<T> a, (T X, T Y, T Z) b) => new(a.X / b.X, b.Y / b.Y, a.Z / b.Z);
    public static Pos3D<T> operator %(Pos3D<T> a, Pos3D<T> b) => new(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
    public static Pos3D<T> operator %(Pos3D<T> a, (T X, T Y, T Z) b) => new(a.X % b.X, a.Y % b.Y, a.Z % b.Z);

    public T ManhattanDistance(Pos3D<T> other) => (X, Y, Z).ManhattanDistance((other.X, other.Y, other.Z));

    public bool IsBetween(Pos3D<T> min, Pos3D<T> max)
        => min.X <= X && X <= max.X
        && min.Y <= Y && Y <= max.Y
        && min.Z <= Z && Z <= max.Z;

    public bool IsOutsideBounds(Pos3D<T> min, Pos3D<T> max)
        => min.X > X || X > max.X
        || min.Y > Y || Y > max.Y
        || min.Z > Z || Z > max.Z;

    public IEnumerable<Pos3D<T>> GetNeighbours(bool includeDiagonal = false) => includeDiagonal ? GetAllNeighbours() : GetAdjacent();

    public static readonly (T X, T Y, T Z)[] AdjacentDirections =
        [(-T.One, T.Zero, T.Zero), (T.Zero, -T.One, T.Zero), (T.Zero, T.Zero, -T.One), (T.One, T.Zero, T.Zero), (T.Zero, T.One, T.Zero), (T.Zero, T.Zero, T.One)];
    public static readonly (T X, T Y, T Z)[] AllDirections
        = Enumerable.Range(-1, 3)
                    .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                    .Select(z => (T.CreateChecked(x), T.CreateChecked(y), T.CreateChecked(z)))))
                    .Where(d => d != (T.Zero, T.Zero, T.Zero)).ToArray();

    private IEnumerable<Pos3D<T>> GetAdjacent()
    {
        foreach (var dir in AdjacentDirections)
            yield return this + dir;
    }

    private IEnumerable<Pos3D<T>> GetAllNeighbours()
    {
        foreach (var dir in AllDirections)
            yield return this + dir;
    }
}
