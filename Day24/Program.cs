namespace Day24;

public static class Program
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
        var queue = new Queue<Gate>(ParseGates(_input[1].SplitLines()));

        while (queue.TryDequeue(out var gate))
        {
            if (!wires.TryGetValue(gate.LeftWire, out bool left) || !wires.TryGetValue(gate.RightWire, out bool right))
            {
                queue.Enqueue(gate);
                continue;
            }

            wires[gate.OutputWire] = gate.Func switch
            {
                "AND" => left && right,
                "XOR" => left ^ right,
                _ => left || right
            };
        }

        var zWires = wires.Keys.Where(x => x.StartsWith('z')).OrderDescending();
        var output = string.Concat(zWires.Select(x => wires[x] ? "1" : "0"));

        return Convert.ToInt64(output, 2);
    }

    private static string Part2()
    {
        List<string> swaps = [];
        int index = 0;
        var carryWire = "";

        var gates = ParseGates(_input[1].SplitLines()).ToDictionary(x => x.OutputWire, x => x);

        while (swaps.Count < 8)
        {
            var (xWire, yWire, zWire) = GetWires(index);

            if (index == 0)
            {
                carryWire = gates.FindGate(xWire, "AND", yWire);
                index++;
                continue;
            }

            var xorWire = gates.FindGate(xWire, "XOR", yWire);
            var andWire = gates.FindGate(xWire, "AND", yWire);
            var carryInWire = gates.FindGate(xorWire, "XOR", carryWire);

            if (carryInWire is null && !string.IsNullOrWhiteSpace(xorWire) && !string.IsNullOrWhiteSpace(andWire))
            {
                gates.ApplySwap(xorWire, andWire, swaps);
                continue;
            }

            if (carryInWire != zWire && !string.IsNullOrWhiteSpace(carryInWire))
            {
                gates.ApplySwap(carryInWire, zWire, swaps);
                continue;
            }

            carryInWire = gates.FindGate(xorWire, "AND", carryWire);
            carryWire = gates.FindGate(andWire, "OR", carryInWire);

            index++;
        }

        return string.Join(",", swaps.Order());
    }

    private static (string xWire, string yWire, string zWire) GetWires(int index) => ($"x{index:00}", $"y{index:00}", $"z{index:00}");

    private static void ApplySwap(this Dictionary<string, Gate> gates, string wire1, string wire2, List<string> swaps)
    {
        swaps.AddRange(wire1, wire2);
        (gates[wire1], gates[wire2]) = (gates[wire2], gates[wire1]);
    }

    private static string FindGate(this Dictionary<string, Gate> gates, string leftWire, string func, string rightWire)
        => gates.FirstOrDefault(x => (x.Value.LeftWire == leftWire && x.Value.Func == func && x.Value.RightWire == rightWire)
                                  || (x.Value.LeftWire == rightWire && x.Value.Func == func && x.Value.RightWire == leftWire)).Key;

    private static Dictionary<string, bool> ParseWires(string[] input)
        => input.Select(x =>
        {
            var split = x.Split(": ");
            return (split[0], split[1] == "1");
        }).ToDictionary();

    private static List<Gate> ParseGates(string[] input)
        => input.Select(x =>
        {
            var split = x.Split([" ", "->"], StringSplitOptions.RemoveEmptyEntries);
            return new Gate(split[0], split[2], split[1], split[3]);
        }).ToList();
}

internal record Gate(string LeftWire, string RightWire, string Func, string OutputWire);