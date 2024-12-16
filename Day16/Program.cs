﻿using AoC.Tools.Models;

namespace Day16;

public class Program
{
    private static readonly string[] _input = Input.ReadAllLines();
    private static readonly Dictionary<char, char> TurnRight = new() { ['N'] = 'E', ['E'] = 'S', ['S'] = 'W', ['W'] = 'N' };
    private static readonly Dictionary<char, char> TurnLeft = new() { ['N'] = 'W', ['W'] = 'S', ['S'] = 'E', ['E'] = 'N' };

    private static void Main()
    {
        var (lowestScore, tilesOnBestPaths) = Solve();
        Console.WriteLine($"Part 1: {lowestScore}");
        Console.WriteLine($"Part 2: {tilesOnBestPaths}");
    }

    private static (int, int) Solve()
    {
        var map = _input.AsCharMap();
        var start = map.First(x => x.Value == 'S').Key;
        var end = map.First(x => x.Value == 'E').Key;

        var initialState = new ReindeerState(start, 'E', 0, [start]);
        PriorityQueue<ReindeerState, int> queue = new([(initialState, 0)]);

        var lowestScore = int.MaxValue;
        List<ReindeerState> endStates = [];
        Dictionary<(Position, char), int> minScores = [];

        while (queue.TryDequeue(out var current, out var score))
        {
            if (score > lowestScore) continue;

            if (current.Position == end)
            {
                endStates.Add(current);
                lowestScore = Math.Min(score, lowestScore);
                continue;
            }

            AddStep(current, current.Direction, score + 1);
            AddStep(current, TurnRight[current.Direction], score + 1001);
            AddStep(current, TurnLeft[current.Direction], score + 1001);

            void AddStep(ReindeerState state, char direction, int nextScore)
            {
                var next = state.Position.MoveInDirection(direction);

                if (map.GetValueOrDefault(next) == '#') return;

                if (!minScores.ContainsKey((next, direction)))
                    minScores[(next, direction)] = int.MaxValue;

                if (minScores[(next, direction)] >= nextScore)
                {
                    minScores[(next, direction)] = nextScore;
                    var nextState = new ReindeerState(next, direction, nextScore, [.. state.Route, next]);
                    queue.Enqueue(nextState, nextScore);
                }
            }
        }

        var tilesOnBestPaths = endStates.Where(x => x.Score == lowestScore).SelectMany(x => x.Route).ToHashSet().Count;

        return (lowestScore, tilesOnBestPaths);
    }
}

internal record ReindeerState(Position Position, char Direction, int Score, HashSet<Position> Route);