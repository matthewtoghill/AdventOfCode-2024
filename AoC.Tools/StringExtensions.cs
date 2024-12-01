using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Tools;

public static partial class StringExtensions
{
    public static string Left(this string input, int length)
    {
        return input.Length > length ? input[..length] : input;
    }

    public static string Right(this string input, int length)
    {
        return input.Length > length ? input[^length..] : input;
    }

    public static string LeftOf(this string input, char c)
    {
        int i = input.IndexOf(c);
        if (i >= 0) return input.Substring(0, i);
        return input;
    }

    public static string LeftOf(this string input, string s)
    {
        int i = input.IndexOf(s);
        if (i >= 0) return input[..i];
        return input;
    }

    public static string RightOf(this string input, char c)
    {
        int i = input.IndexOf(c);
        if (i == -1) return input;
        return input[(i + 1)..];
    }

    public static string RightOf(this string input, string s)
    {
        int i = input.IndexOf(s);
        if (i == -1) return input;
        return input[(i + s.Length)..];
    }

    public static string RightOfLast(this string input, char c)
    {
        int i = input.LastIndexOf(c);
        if (i == -1) return input;
        return input[(i + 1)..];
    }

    public static string RightOfLast(this string input, string s)
    {
        int i = input.LastIndexOf(s);
        if (i == -1) return input;
        return input[(i + s.Length)..];
    }

    public static string Mid(this string input, int start)
    {
        return input.Substring(Math.Min(start, input.Length));
    }

    public static string Mid(this string input, int start, int count)
    {
        return input.Substring(Math.Min(start, input.Length), Math.Min(count, Math.Max(input.Length - start, 0)));
    }

    public static string GetBetween(this string input, string before, string after)
    {
        int beforeIndex = input.IndexOf(before);
        int startIndex = beforeIndex + before.Length;
        int afterIndex = input.IndexOf(after, startIndex);

        if (beforeIndex == -1 || afterIndex == -1) return "";

        return input.Substring(startIndex, afterIndex - startIndex);
    }

    public static string StripOut(this string input, char character)
    {
        return input.Replace(character.ToString(), "");
    }

    public static string StripOut(this string input, params char[] chars)
    {
        foreach (char c in chars)
        {
            input = input.Replace(c.ToString(), "");
        }
        return input;
    }

    public static string StripOut(this string input, string subString)
    {
        return input.Replace(subString, "");
    }

    public static string SortString(string input)
    {
        char[] chars = input.ToCharArray();
        Array.Sort(chars);
        return new string(chars);
    }

    public static void PrintAllLines(this string[] lines, bool includeLineNums = false)
    {
        if (includeLineNums)
        {
            int count = lines.Length;
            string formatString = " {0," + count.ToString().Length + "}: {1}";
            for (int i = 0; i < count; i++)
                Console.WriteLine(formatString, i, lines[i]);
        }
        else
        {
            foreach (var line in lines)
                Console.WriteLine(line);
        }
    }

    public static string ReplaceAtIndex(this string text, int index, char c)
    {
        StringBuilder sb = new(text);
        sb[index] = c;
        return sb.ToString();
    }

    public static bool IsNullOrWhiteSpace(this string text) => string.IsNullOrWhiteSpace(text);
    public static bool IsNotNullOrWhiteSpace(this string text) => !string.IsNullOrWhiteSpace(text);

    public static IEnumerable<int> ExtractInts(this string text) => NumbersOnlyRegex().Matches(text).Select(m => int.Parse(m.Value));
    public static IEnumerable<int> ExtractPositiveInts(this string text) => PositiveNumbersOnlyRegex().Matches(text).Select(m => int.Parse(m.Value));

    public static IEnumerable<T> ExtractNumeric<T>(this string text) where T : IParsable<T>
        => NumbersOnlyRegex().Matches(text).Select(m => T.Parse(m.Value, null));

    public static IEnumerable<T> ExtractPositiveNumeric<T>(this string text) where T : IParsable<T>
        => PositiveNumbersOnlyRegex().Matches(text).Select(m => T.Parse(m.Value, null));

    [GeneratedRegex(@"-?\d+")] private static partial Regex NumbersOnlyRegex();

    [GeneratedRegex(@"\d+")] private static partial Regex PositiveNumbersOnlyRegex();
}
