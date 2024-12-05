using System.Text;

namespace AoC.Tools;

public static class Input
{
    public static string ReadAll()
        => File.ReadAllText(@$"..\..\..\..\data\{System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name}.txt");

    public static string ReadAll(int day)
        => File.ReadAllText(@$"..\..\..\..\data\day{day:00}.txt");

    public static string[] ReadAllLines()
        => File.ReadAllLines(@$"..\..\..\..\data\{System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name}.txt");

    public static string[] ReadAllLines(int day)
        => File.ReadAllLines(@$"..\..\..\..\data\day{day:00}.txt");

    public static IEnumerable<T> ReadAllLinesTo<T>(StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries) where T : IParsable<T>
        => ReadAll().SplitLines(splitOptions)
                    .Select(x => T.Parse(x, null));

    public static IEnumerable<T> ReadAllLinesTo<T>(int day, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries) where T : IParsable<T>
        => ReadAll(day).SplitLines(splitOptions)
                       .Select(x => T.Parse(x, null));

    public static string[] SplitLines(this string text, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        => text.Split(["\n", "\r\n"], splitOptions);

    public static IEnumerable<T> SplitTo<T>(this string text, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries) where T : IParsable<T>
        => text.SplitLines(splitOptions)
               .Select(x => T.Parse(x, null));

    public static IEnumerable<T> SplitTo<T>(this string text, params string[] separators) where T : IParsable<T>
        => text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
               .Select(x => T.Parse(x, null));

    public static string[] SplitOn(this string text, StringSplitOptions splitOptions, params string[] separators)
        => text.Split(separators, splitOptions);

    public static string[] SplitOn(this string text, params string[] separators)
        => text.Split(separators, StringSplitOptions.None);

    public static string[] SplitOn(this string text, StringSplitOptions splitOptions, params char[] separators)
        => text.Split(separators, splitOptions);

    public static string[] SplitOn(this string text, params char[] separators)
        => text.Split(separators, StringSplitOptions.None);

    public static string[] ReadAsParagraphs(StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        => ReadAll().Split(["\n\n", "\r\n\r\n"], splitOptions);

    public static string[] ReadAsParagraphs(int day, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        => ReadAll(day).Split(["\n\n", "\r\n\r\n"], splitOptions);

    public static IEnumerable<string> SplitIntoColumns(this string text)
    {
        var rows = text.SplitLines();
        var cols = rows[0].Length;
        List<string> result = [];
        for (int i = 0; i < cols; i++)
        {
            StringBuilder sb = new();
            foreach (var row in rows)
            {
                sb.Append(row[i]);
            }
            result.Add(sb.ToString());
        }
        return result;
    }

    public static T[,] ReadAsGrid<T>() where T : IParsable<T>
    {
        var lines = ReadAllLines();
        var grid = new T[lines.Length, lines[0].Length];
        for (int x = 0; x < lines.Length; x++)
        {
            for (int y = 0; y < lines[x].Length; y++)
            {
                grid[x, y] = T.Parse(lines[x][y].ToString(), null);
            }
        }
        return grid;
    }
}
