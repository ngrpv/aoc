using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Puzzle4
{
    public static int Solve2(string[] field)
    {
        var leftDiagonals = GetDiagonalsLeftUpToRightDown(field);
        var rightDiagonals = GetDiagonalFromLeftDownToRightUp(field);

        return leftDiagonals
            .Concat(rightDiagonals)
            .Select(points => GetStringValuesWithPoints(points, field))
            .SelectMany(GetX_MasPoints)
            .GroupBy(x => x)
            .Count(x => x.Count() > 1);
    }

    private static IEnumerable<(int x, int y)> GetX_MasPoints(IEnumerable<(char charValue, int x, int y)> arg)
    {
        var argArray = arg.ToArray();
        var str = string.Join("", argArray.Select(x => x.charValue.ToString()));
        var matches = Regex.Matches(str, "MAS").Concat(Regex.Matches(str, "SAM"));
        var indexesOfA = matches.Select(match => match.Index + 1).ToList();
        return indexesOfA.Select(x => (argArray[x].x, argArray[x].y)).ToArray();
    }

    private static IEnumerable<(char charValue, int x, int y)> GetStringValuesWithPoints((int x, int y)[] points,
        string[] field)
    {
        return points.Select(i => (field[i.y][i.x], i.x, i.y));
    }

    public static int Solve(string[] field)
    {
        var horizontals = GetHorizontals(field);
        var verticals = GetVerticals(field);
        var leftDiagonals = GetDiagonalsLeftUpToRightDown(field);
        var rightDiagonals = GetDiagonalFromLeftDownToRightUp(field);

        return horizontals
            .Concat(verticals)
            .Concat(leftDiagonals)
            .Concat(rightDiagonals)
            .Select(points => GetStringValues(points, field))
            .Sum(GetXmasCount);
    }

    private static IEnumerable<(int x, int y)[]> GetHorizontals(string[] field)
    {
        var height = field.Length;
        for (var i = 0; i < height; i++)
        {
            var list = new List<(int, int)>();
            for (var j = 0; j < field[0].Length; j++)
                list.Add((j, i));

            yield return list.ToArray();
        }
    }

    private static int GetXmasCount(string s)
    {
        return Regex.Matches(s, "XMAS").Count + Regex.Matches(s, "SAMX").Count;
    }

    private static IEnumerable<(int x, int y)[]> GetDiagonalFromLeftDownToRightUp(string[] field)
    {
        var maxX = field[0].Length - 1;
        var maxY = field.Length - 1;
        return GenerateRightThenDown(0, 0, maxX, maxY)
            .Select(i => GetDiagonalToLeftDown(i.x, i.y, maxX, maxY).ToArray());
    }

    private static IEnumerable<(int x, int y)> GetDiagonalToLeftDown(int x, int y, int maxX, int maxY)
    {
        while (x >= 0 && y <= maxY)
            yield return (x--, y++);
    }

    private static IEnumerable<(int x, int y)> GenerateRightThenDown(int x, int y, int maxX, int maxY)
    {
        while (true)
        {
            yield return (x, y);
            (x, y) = GoRightOrDown(x, y);
            if (y > maxY)
                yield break;
        }


        (int x, int y) GoRightOrDown(int x, int y)
            => x < maxX
                ? (x + 1, y)
                : (x, y + 1);
    }

    public static IEnumerable<(int x, int y)[]> GetDiagonalsLeftUpToRightDown(string[] field) // \\\ 
    {
        var maxX = field[0].Length - 1;
        var maxY = field.Length - 1;

        return GenerateUpThenRight(0, maxY, maxX, maxY)
            .Select(i => GetDiagonalToRightDown(i.x, i.y, maxX, maxY).ToArray());
    }

    private static string GetStringValues(IEnumerable<(int x, int y)> points, string[] field)
    {
        return string.Join("", points.Select(i => field[i.y][i.x]).ToArray());
    }

    static IEnumerable<(int x, int y)> GetDiagonalToRightDown(int x, int y, int maxX, int maxY)
    {
        while (x <= maxX && y <= maxY)
            yield return (x++, y++);
    }

    static IEnumerable<(int x, int y)> GenerateUpThenRight(int x, int y, int maxX, int maxY)
    {
        while (true)
        {
            yield return (x, y);
            (x, y) = GoUpOrRight(x, y);
            if (x > maxX)
                yield break;
        }

        static (int x, int y) GoUpOrRight(int x, int y)
            => y > 0
                ? (x, y - 1)
                : (x + 1, y);
    }


    private static IEnumerable<(int x, int y)[]> GetVerticals(string[] field)
    {
        var width = field[0].Length;
        for (var i = 0; i < width; i++)
        {
            var list = new List<(int, int)>();
            for (var j = 0; j < field.Length; j++)
                list.Add((i, j));

            yield return list.ToArray();
        }
    }
}