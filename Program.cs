// See https://aka.ms/new-console-template for more information

using AdventOfCode2024;

var input = File.ReadAllLines("puzzle7.txt");

var equations = Puzzle7.Parse(input);
var count = Puzzle7.Solve(equations);
Console.WriteLine(count);