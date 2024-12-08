namespace AdventOfCode2024;

public class Puzzle7
{
    public static Equation[] Parse(string[] input)
    {
        return input.Select(line => line.Split(":"))
            .Select(arr => new Equation
            {
                Result = long.Parse(arr[0].Trim()),
                Operands = arr[1].Trim().Split(" ").Select(long.Parse).ToArray()
            })
            .ToArray();
    }

    public static long Solve(Equation[] equations)
    {
        return equations.Where(PossiblyBeTrue)
            .Select(x => x.Result)
            .Sum();
    }

    private static bool PossiblyBeTrue(Equation equation)
    {
        if (equation.Operands.Length == 1)
            return equation.Result == equation.Operands[0];

        var multInTheEnd = new Equation()
        {
            Result = equation.Result / equation.Operands[^1],
            Operands = equation.Operands[..^1]
        };

        var addInTheEnd = new Equation()
        {
            Result = equation.Result - equation.Operands[^1],
            Operands = equation.Operands[..^1]
        };
        if (equation.Result % equation.Operands[^1] != 0)
            return PossiblyBeTrue(addInTheEnd);
        return PossiblyBeTrue(multInTheEnd) || PossiblyBeTrue(addInTheEnd);
    }
}

public class Equation
{
    public long Result { get; set; }
    public long[] Operands { get; set; }
}