﻿using AoC.Tools.Models;

namespace Day15;

public class Program
{
    private static readonly string _input = Input.ReadAll();
    private static void Main()
    {
        Console.WriteLine($"Part 1: {Part1()}");
        Console.WriteLine($"Part 2: {Part2()}");
    }

    private static int Part1()
    {
        var (map, movements) = ParseInput(_input.SplitParagraphs());
        var robot = map.First(x => x.Value == '@').Key;

        foreach (var move in movements)
        {
            var newRobotPos = robot.MoveInDirection(move);
            if (map[newRobotPos] == '#') continue;
            if (map[newRobotPos] == '.')
            {
                map[robot] = '.';
                robot = newRobotPos;
                map[robot] = '@';
                continue;
            }

            if (map[newRobotPos] == 'O')
            {
                var endBoxPos = newRobotPos;
                var canMove = true;

                // keep checking in the direction until we arrive at a wall or an empty space
                while (true)
                {
                    endBoxPos = endBoxPos.MoveInDirection(move);
                    if (map[endBoxPos] == 'O') continue;
                    if (map[endBoxPos] == '.') break;
                    if (map[endBoxPos] == '#') { canMove = false; break; }
                }

                if (canMove)
                {
                    map[robot] = '.';
                    map[newRobotPos] = '@';
                    robot = newRobotPos;
                    map[endBoxPos] = 'O';
                }
            }
        }

        return map.Where(x => x.Value == 'O').Select(x => x.Key).Sum(x => (x.Row * 100) + x.Col);
    }

    private static int Part2()
    {
        var (map, movements) = ParseInput(_input.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.").SplitParagraphs());
        var robot = map.First(x => x.Value == '@').Key;

        foreach (var move in movements)
        {
            var newRobotPos = robot.MoveInDirection(move);
            if (map[newRobotPos] == '#') continue;
            if (map[newRobotPos] == '.')
            {
                map[robot] = '.';
                map[newRobotPos] = '@';
                robot = newRobotPos;
                continue;
            }

            if (map[newRobotPos] == '[' || map[newRobotPos] == ']')
            {
                var left = map[newRobotPos] == '[' ? newRobotPos : newRobotPos.MoveInDirection('<');
                var right = map[newRobotPos] == ']' ? newRobotPos : newRobotPos.MoveInDirection('>');
                var newMap = map.ToDictionary();
                var canMove = true;
                Queue<Position> queue = new([left, right]);
                HashSet<Position> boxesToMove = [left, right];
                HashSet<Position> visited = [];

                // keep checking in the direction until we arrive at a wall or an empty space
                while (queue.TryDequeue(out var current))
                {
                    var next = current.MoveInDirection(move);
                    if (!visited.Add(next)) continue;
                    var c = map[next];
                    if (c == '.') continue;
                    if (c == '#') { canMove = false; break; }

                    // must be part of a box: [ or ]
                    left = c == '[' ? next : next.MoveInDirection('<');
                    right = c == ']' ? next : next.MoveInDirection('>');
                    queue.Enqueue(left);
                    queue.Enqueue(right);
                    boxesToMove.Add(left);
                    boxesToMove.Add(right);
                }

                if (canMove)
                {
                    boxesToMove.ForEach(b => newMap[b] = '.'); // clear all of the spaces they are moving out of first
                    boxesToMove.ForEach(b => newMap[b.MoveInDirection(move)] = map[b]); // move the boxes into their new positions
                    newMap[robot] = '.';
                    newMap[newRobotPos] = '@';
                    robot = newRobotPos;

                    map = newMap;
                }
            }
        }

        return map.Where(x => x.Value == '[').Select(x => x.Key).Sum(x => (x.Row * 100) + x.Col);
    }

    private static (Dictionary<Position, char>, string) ParseInput(string[] input)
    {
        var map = input[0].SplitLines().AsCharMap();
        var movements = string.Concat(input[1].SplitLines());

        return (map, movements);
    }
}