using System.Numerics;

namespace AoC.Tools;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (T item in items)
            action(item);
    }

    public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
    {
        var index = 0;
        foreach (T item in items)
            action(item, index++);
    }

    public static IEnumerable<bool> CompareAll<T>(this IEnumerable<T> items, Func<T, T, bool> predicate)
    {
        var itemArray = items.ToArray();
        for (int i = 0; i < itemArray.Length; i++)
            for (int j = i + 1; j < itemArray.Length; j++)
                yield return predicate(itemArray[i], itemArray[j]);
    }

    public static IEnumerable<T> ApplyEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (T item in items)
        {
            action(item);
            yield return item;
        }
    }

    public static IEnumerable<T> ApplyEach<T>(this IEnumerable<T> items, Action<T, int> action)
    {
        var index = 0;
        foreach (T item in items)
        {
            action(item, index++);
            yield return item;
        }
    }

    public static IEnumerable<T> Without<T>(this IEnumerable<T> items, T value)
        => items.Where(x => !Equals(x, value));

    public static Dictionary<T, int> GetFrequencies<T>(this IEnumerable<T> items) where T : notnull
        => items.CountBy(x => x).ToDictionary();

    public static T Product<T>(this IEnumerable<T> items) where T : INumber<T>
        => items.Aggregate((total, x) => total * x);

    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> items)
        => items.SelectMany(x => x);

    public static bool IsAnyOf<T>(this T item, params T[] items)
        => items.Contains(item);

    public static IEnumerable<T> EnumerateDifferences<T>(this IEnumerable<T> items) where T : INumber<T>
        => items.Zip(items.Skip(1)).Select(x => x.Second - x.First);

    public static T RandomElement<T>(this IEnumerable<T> items)
        => items.ElementAt(Random.Shared.Next(items.Count()));
}
