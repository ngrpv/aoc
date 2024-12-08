namespace AdventOfCode2024;

public class Puzzle6
{
    public static (Map map, Guard guard) Parse(string[] input)
    {
        var (x, y) = input
            .Select((x, i) => (x.IndexOf('^'), i))
            .First(x => x.Item1 != -1);
        return (new Map(input), new Guard(new Point(x, y), new Point(0, -1)));
    }

    public static int Solve(Map map, Guard guard)
    {
        return guard.GoThrough(map)
            .Distinct()
            .Count();
    }

    public static int Solve2(Map map, Guard guard)
    {
        var allPositions = new Guard(guard.Position, guard.DirectionVector)
            .GoThrough(map)
            .Where(x => x != guard.Position)
            .Distinct();

        var count = 0;
        foreach (var position in allPositions)
        {
            map.PutObstruction(position);
            if (IsLooped(new Guard(guard.Position, guard.DirectionVector), map)) 
                count++;
            map.RemoveObstruction(position);
        }

        return count;
    }

    private static bool IsLooped(Guard guard, Map map)
    {
        var visited = new HashSet<(Point, Point)>();
        return guard.GoThrough(map).Any(position => !visited.Add((position, guard.DirectionVector)));
    }

    // (0, -1) => (1, 0) => (0, 1) => (-1, 0)
    private static Point Step(Map map, Guard guard)
    {
        var nextPosition = guard.Position + guard.DirectionVector;
        if (map.IsObstruction(nextPosition))
        {
            guard.DirectionVector = new Point(-guard.DirectionVector.Y, guard.DirectionVector.X);
            return Step(map, guard);
        }

        guard.Position = nextPosition;
        return nextPosition;
    }
}

public record struct Point(int X, int Y)
{
    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Y + p2.Y);
    }
}

public class Guard
{
    public Guard(Point position, Point directionVector)
    {
        Position = position;
        DirectionVector = directionVector;
    }

    public Point Position { get; set; }
    public Point DirectionVector { get; set; }

    public IEnumerable<Point> GoThrough(Map map)
    {
        yield return Position;
        while (true)
        {
            var next = Step(map);
            if (map.IsOutOfBounds(next)) yield break;
            yield return next;
        }
    }

    private Point Step(Map map)
    {
        var nextPosition = Position + DirectionVector;
        if (map.IsObstruction(nextPosition))
        {
            DirectionVector = new Point(-DirectionVector.Y, DirectionVector.X);
            return Step(map);
        }

        Position = nextPosition;
        return nextPosition;
    }
}

public class Map(string[] map)
{
    public bool IsObstruction(Point p)
    {
        return !IsOutOfBounds(p) && map[p.Y][p.X] == '#';
    }

    public bool IsOutOfBounds(Point p)
    {
        return p.Y < 0 || p.X < 0 || p.Y >= map.Length || p.X >= map[0].Length;
    }

    private void Set(int x, int y, char ch)
    {
        map[y] = map[y][..x] + ch + map[y][(x + 1)..];
    }

    public void PutObstruction(Point p)
    {
        Set(p.X, p.Y, '#');
    }

    public void RemoveObstruction(Point p)
    {
        Set(p.X, p.Y, '.');
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, map);
    }
}