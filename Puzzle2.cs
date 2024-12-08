namespace AdventOfCode2024;

public class Puzzle2
{
    public static int[] ParseReport(string input)
    {
        return input.Split().Select(int.Parse).ToArray();
    }

    public int Solve(int[][] reports)
    {
        return reports.Where(IsSafe).Count();
    }

    public int Solve2(int[][] reports)
    {
        return reports.Where(IsSafeWithProblemDampener).Count();
    }

    private bool IsSafeWithProblemDampener(int[] levels)
    {
        for (var i = 0; i < levels.Length; i++)
        {
            if (IsSafe(levels[..i].Concat(levels[(i + 1)..levels.Length]).ToArray()))
                return true;
        }

        return false;
    }

    private static bool IsSafe(int[] levels)
    {
        if (levels.Length < 2)
            return true;

        var isIncreasing = levels[1] > levels[0];

        for (var i = 1; i < levels.Length; i++)
        {
            if (isIncreasing && levels[i] <= levels[i - 1])
                return false;

            if (!isIncreasing && levels[i] >= levels[i - 1])
                return false;

            var diff = Math.Abs(levels[i] - levels[i - 1]);
            if (diff is < 1 or > 3)
                return false;
        }

        return true;
    }
}