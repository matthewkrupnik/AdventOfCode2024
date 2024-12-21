internal class Day6 : Day
{
    public Day6(string filePath = "Day6.txt") : base(filePath) 
    {
        input = LoadInputToLineList("Day6.txt");
    }

    private List<string> input;

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private char[][] ConvertInputToCharMatrix()
    {
        char[][] matrix = new char[input.Count][];
        for (int i = 0; i < input.Count; i++)
        {
            matrix[i] = input[i].ToCharArray();
        }
        return matrix;
    }

    private int[][] VisitedSpacesMap()
    {
        int[][] visited = new int[input.Count][];
        for (int i = 0; i < input.Count; i++)
        {
            visited[i] = new int[input[i].Length];
        }

        return visited;
    }

    private Tuple<int,int> FindStart(char[][] matrix)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                if (matrix[i][j] != '.' && matrix[i][j] != '#')
                {
                    return new Tuple<int, int>(i, j);
                }
            }
        }

        throw new Exception("Mo start found");
    }

    internal string SolveA()
    {
        var map = ConvertInputToCharMatrix();
        var start = FindStart(map);
        Direction direction = Direction.Up;
        direction = FindStartingDirection(map, start, direction);

        var visited = FindGuardPath(map, start, direction);

        int count = GetVisitedSpacesCount(visited);

        return count.ToString();
    }

    private Direction FindStartingDirection(char[][] map, Tuple<int, int> start, Direction direction)
    {
        switch (map[start.Item1][start.Item2])
        {
            case '^':
                direction = Direction.Up;
                break;
            case 'V':
                direction = Direction.Down;
                break;
            case '<':
                direction = Direction.Left;
                break;
            case '>':
                direction = Direction.Right;
                break;
        }

        return direction;
    }

    private int[][] FindGuardPath(char[][] map, Tuple<int, int> start, Direction direction)
    {
        var visited = VisitedSpacesMap();
        visited[start.Item1][start.Item2] = 1;
        map[start.Item1][start.Item2] = '.';
        int[] currentPosition = [start.Item1, start.Item2];
        int[] newPosition = [start.Item1, start.Item2];

        while (true)
        {
            //PrintMap(start, currentPosition, direction, map);

            switch (direction)
            {
                case Direction.Up:
                    newPosition[0] = currentPosition[0] - 1;
                    break;
                case Direction.Down:
                    newPosition[0] = currentPosition[0] + 1;
                    break;
                case Direction.Left:
                    newPosition[1] = currentPosition[1] - 1;
                    break;
                case Direction.Right:
                    newPosition[1] = currentPosition[1] + 1;
                    break;
            }

            if (newPosition[0] < 0 || newPosition[0] >= map.Length || newPosition[1] < 0 || newPosition[1] >= map[0].Length)
            {
                break;
            }

            if (map[newPosition[0]][newPosition[1]] == '.')
            {
                currentPosition[0] = newPosition[0];
                currentPosition[1] = newPosition[1];
                visited[currentPosition[0]][currentPosition[1]] += 1;

                if (visited[currentPosition[0]][currentPosition[1]] == 5)
                {
                    throw new Exception("Too many visits");
                }
            }
            else if (map[newPosition[0]][newPosition[1]] == '#')
            {
                newPosition = [currentPosition[0], currentPosition[1]];
                switch (direction)
                {
                    case Direction.Up:
                        direction = Direction.Right;
                        break;
                    case Direction.Down:
                        direction = Direction.Left;
                        break;
                    case Direction.Left:
                        direction = Direction.Up;
                        break;
                    case Direction.Right:
                        direction = Direction.Down;
                        break;
                }
            }
        }

        return visited;
    }

    private void PrintMap(Tuple<int, int> start, int[] currentPosition, Direction direction, char[][] map)
    {
        var mapList = map.ToList();
        mapList[start.Item1][start.Item2] = '.';
        switch (direction)
        {
            case Direction.Up:
                map[currentPosition[0]][currentPosition[1]] = '^';
                break;
            case Direction.Down:
                map[currentPosition[0]][currentPosition[1]] = 'V';
                break;
            case Direction.Left:
                map[currentPosition[0]][currentPosition[1]] = '<';
                break;
            case Direction.Right:
                map[currentPosition[0]][currentPosition[1]] = '>';
                break;
        }

        foreach (var line in mapList)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine("------------------------");
    }

    private int GetVisitedSpacesCount(int[][] visited)
    {
        int count = 0;

        for (int i = 0; i < visited.Length; i++)
        {
            for (int j = 0; j < visited[i].Length; j++)
            {
                if (visited[i][j] > 0)
                {
                    count++;
                }
            }
        }

        return count;
    }

    internal string SolveB()
    {
        var map = ConvertInputToCharMatrix();
        var start = FindStart(map);
        Direction direction = Direction.Up;
        direction = FindStartingDirection(map, start, direction);

        var visited = FindGuardPath(map, start, direction);

        // For each visited place, replace it with an obstacle and check if it creates an infinite loop
        var count = 0;
        for (int i = 0; i < visited.Length; i++)
        {
            for (int j = 0; j < visited[i].Length; j++)
            {
                if (visited[i][j] > 0)
                {
                    bool hasLoop = CheckForLoop(map, i, j, start, direction);
                    if (hasLoop) count++;
                }
            }
        }

        return count.ToString();
    }

    private bool CheckForLoop(char[][] mapOriginal, int i, int j, Tuple<int, int> start, Direction direction)
    {
        // Make deep copy of map
        var map = ConvertInputToCharMatrix();

        // Add obstacle to map
        map[i][j] = '#';

        try
        {
            var visited = FindGuardPath(map, start, direction);
        }
        catch (Exception e)
        {
            if (e.Message == "Too many visits")
            {
                return true;
            }
        }

        return false;
    }
}