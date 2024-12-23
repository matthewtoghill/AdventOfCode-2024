namespace Day23;

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
        var computers = GetComputers();
        HashSet<string> groups = [];

        foreach (var (computer, connections) in computers)
        {
            foreach (var pairs in connections.GeneratePermutations(2))
            {
                string[] nodes = [computer, .. pairs];

                if (!nodes.Any(n => n[0] == 't')) continue;

                if (nodes.Zip(nodes.Skip(1)).All(x => computers[x.First].Contains(x.Second)))
                    groups.Add(string.Join(",", nodes.Order()));
            }
        }

        return groups.Count;
    }

    private static string Part2()
    {
        var computers = GetComputers();
        var connections = computers.Keys.ToHashSet();

        while (connections.Count > 1)
        {
            connections = ExpandNetwork(computers, connections);
        }

        return connections.Single();
    }

    private static HashSet<string> ExpandNetwork(Dictionary<string, HashSet<string>> computers, HashSet<string> connections)
    {
        HashSet<string> result = [];

        foreach (var connection in connections)
        {
            var nodes = connection.Split(',');
            foreach (var newNode in nodes.SelectMany(m => computers[m]).ToHashSet())
            {
                if (!nodes.Contains(newNode) && nodes.All(x => computers[newNode].Contains(x)))
                    result.Add(string.Join(",", nodes.Append(newNode).Order()));
            }
        }

        return result;
    }

    private static Dictionary<string, HashSet<string>> GetComputers()
    {
        Dictionary<string, HashSet<string>> computers = [];

        void AddOrUpdate(string key, string connection)
        {
            if (computers.TryGetValue(key, out HashSet<string>? value))
                value.Add(connection);
            else
                computers.Add(key, [connection]);
        }

        foreach (var line in _input)
        {
            var split = line.Split('-');

            AddOrUpdate(split[0], split[1]);
            AddOrUpdate(split[1], split[0]);
        }

        return computers;
    }
}
