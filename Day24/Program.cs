namespace Day24;

public class Program
{
    private static readonly string[] _input = Input.ReadAsParagraphs();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Part1()}");
        Console.WriteLine($"Part 2: {Part2()}");
    }

    private static long Part1()
    {
        var wires = ParseWires(_input[0].SplitLines());
        Queue<string> queue = [];

        _input[1].SplitLines().ForEach(queue.Enqueue);

        while(queue.TryDequeue(out var line))
        {
            var split = line.SplitOn(StringSplitOptions.RemoveEmptyEntries, " ", "->");
            var leftGate = split[0];
            var func = split[1];
            var rightGate = split[2];
            var wire = split[3];

            if (!wires.ContainsKey(leftGate) || !wires.ContainsKey(rightGate))
            {
                queue.Enqueue(line);
                continue;
            }

            wires[wire] = func switch
            {
                "AND" => wires[leftGate] && wires[rightGate],
                "OR" => wires[leftGate] || wires[rightGate],
                "XOR" => wires[leftGate] ^ wires[rightGate],
                _ => throw new NotImplementedException()
            };
        }

        var zWires = wires.Keys.Where(x => x.StartsWith('z')).OrderDescending();
        var output = string.Concat(zWires.Select(x => wires[x] ? "1" : "0"));

        return Convert.ToInt64(output, 2);
    }

    private static int Part2()
    {
        return 0;
    }

    private static Dictionary<string, bool> ParseWires(string[] input)
    {
        Dictionary<string, bool> wires = [];
        foreach (string line in input)
        {
            var split = line.Split(": ");
            wires.Add(split[0], split[1] == "1");
        }
        return wires;
    }
}
