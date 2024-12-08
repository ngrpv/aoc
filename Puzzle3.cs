using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Puzzle3
{
    public static int Solve(string input)
    {
        return Regex.Matches(input, @"mul\((\d+),(\d+)\)")
            .Select(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value))
            .Sum();
    }

    public static int Solve2_2(string input)
    {
        return input.Split("do()").Sum(x => Solve(x.Split("don't()")[0]));
    }
    
    public static int Solve2(string input)
    {
        var matchCollection = Regex.Matches(input, @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)");
        var sum = 0;
        var isDo = true;
        foreach (Match match in matchCollection)
        {
            switch (match.Value)
            {
                case "do()":
                    isDo = true;
                    break;
                case "don't()":
                    isDo = false;
                    break;
                default:
                {
                    if (isDo)
                    {
                        var num1 = int.Parse(match.Groups[1].Value);
                        var num2 = int.Parse(match.Groups[2].Value);
                        sum += num1 * num2;
                    }

                    break;
                }
            }
        }

        return sum;
    }
}