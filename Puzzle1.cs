namespace AdventOfCode2024;

public class Puzzle1
{
    public void Solve()
    {
        var input = File.ReadAllLines("input.txt");
        var splited = input.Select(x => x.Split("   ")).ToList();

        var firstColumn = splited
            .Select(x => x[0])
            .Select(int.Parse)
            .ToList();
        var secondColumn = splited
            .Select(x => x[1])
            .Select(int.Parse)
            .ToList();

        var secondListValueToFreq = new Dictionary<int, int>();
        foreach (var number in secondColumn)
        {
            secondListValueToFreq[number] = secondListValueToFreq.GetValueOrDefault(number, 0) + 1;
        }

        var sum = firstColumn.Sum(number => number * secondListValueToFreq.GetValueOrDefault(number, 0));
        Console.WriteLine(sum);
    }
}