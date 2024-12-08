namespace AdventOfCode2024;

public class Puzzle5
{
    public static IEnumerable<(int, int)> ParseOrderingRules(string[] lines)
    {
        return lines.Select(line => line.Split("|"))
            .Select(numbers => (int.Parse(numbers[0]), int.Parse(numbers[1])));
    }

    public static IEnumerable<IEnumerable<int>> ParseUpdates(string[] lines)
    {
        return lines.Select(ParsePageNumbersForUpdate);
    }

    private static IEnumerable<int> ParsePageNumbersForUpdate(string line)
    {
        return line.Split(",").Select(int.Parse);
    }

    public static (IEnumerable<(int, int)> rules, IEnumerable<IEnumerable<int>> updates) ParseAll(string[] input)
    {
        var rulesPart = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        var updatesPart = input.Skip(rulesPart.Length + 1).ToArray();
        var rules = ParseOrderingRules(rulesPart);
        var updates = ParseUpdates(updatesPart);
        return (rules, updates);
    }

    public static int Solve(IEnumerable<(int, int)> rules, IEnumerable<IEnumerable<int>> updates)
    {
        var rulesArray = rules.ToArray();
        return updates.Where(x => IsInRightOrder(x, rulesArray))
            .Select(GetMiddlePageNumber)
            .Sum();
    }
    
    public static int Solve2(IEnumerable<(int, int)> rules, IEnumerable<IEnumerable<int>> updates)
    {
        var rulesArray = rules.ToArray();
        return updates.Where(x => !IsInRightOrder(x, rulesArray))
            .Select(x => OrderWithRules(x, rulesArray))
            .Select(GetMiddlePageNumber)
            .Sum();
    }

    private static IEnumerable<int> OrderWithRules(IEnumerable<int> enumerable, (int, int)[] rulesArray)
    {
        return enumerable.OrderBy(x => x, Comparer<int>.Create((x, y) => rulesArray.Contains((x, y)) ? -1 : 1));
    }

    private static int GetMiddlePageNumber(IEnumerable<int> update)
    {
        var updateArray = update.ToArray();
        return updateArray[updateArray.Length / 2];
    }

    private static bool IsInRightOrder(IEnumerable<int> update, (int, int)[] rules)
    {
        var updateArray = update.ToArray();
        for (var i = 1; i < updateArray.Length; i++)
        {
            var previous = updateArray[i - 1];
            var current = updateArray[i];
            if (rules.Contains((current, previous)))
                return false;
        }

        return true;
    }
}