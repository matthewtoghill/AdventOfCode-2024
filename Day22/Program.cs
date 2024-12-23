namespace Day22;

public static class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static void Main()
    {
        var (secretNumberSum, mostBananas) = Solve();
        Console.WriteLine($"Part 1: {secretNumberSum}");
        Console.WriteLine($"Part 2: {mostBananas}");
    }

    private static (long, long) Solve()
    {
        long endSecretSum = 0;
        Dictionary<string, int> cache = [];

        foreach (var line in _input)
        {
            var secret = long.Parse(line);
            List<long> prices = [];

            for (int i = 0; i < 2000; i++)
            {
                secret = GetNextSecretNumber(secret);
                prices.Add(secret % 10);
            }

            endSecretSum += secret;

            HashSet<string> seen = [];
            var differences = prices.EnumerateDifferences().ToList();

            for (int i = 0; i < prices.Count - 4; i++)
            {
                var pattern = string.Join("|", differences.Slice(i, 4));

                if (seen.Add(pattern))
                    cache.IncrementAt(pattern, (int)prices[i + 4]);
            }
        }

        return (endSecretSum, cache.Values.Max());
    }

    private static long GetNextSecretNumber(long secret)
        => secret.MixAndPrune(x => x * 64)
                 .MixAndPrune(x => x / 32)
                 .MixAndPrune(x => x * 2048);

    private static long MixAndPrune(this long secret, Func<long, long> step) => (secret ^ step(secret)) % 16777216;
}
