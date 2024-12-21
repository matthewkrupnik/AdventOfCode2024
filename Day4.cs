



internal class Day4
{
    internal string SolveA(List<string> input)
    {
        // Parse input
        char[][] chars = new char[input.Count][];

        foreach (var line in input)
        {
            chars[input.IndexOf(line)] = line.ToCharArray();
        }

        // Find all instances of "X"
        int xmasCount = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            for (int j = 0; j < chars[i].Length; j++)
            {
                if (chars[i][j] == 'X')
                {
                    //Console.WriteLine($"Found X at ({i},{j})");
                    xmasCount += FindMAS(chars, i, j);
                    //Console.WriteLine($"XMAS count is now {xmasCount}");
                }
            }
        }

        return xmasCount.ToString();
    }

    private int FindMAS(char[][] chars, int i, int j)
    {
        int successCount = 0;
        for (int k = i - 1; k <= i + 1; k++)
        {
            for (int l = j - 1; l <= j + 1; l++)
            {
                if (k < 0 || l < 0 || k >= chars.Length || l >= chars[k].Length)
                {
                    continue;
                }
                
                if (chars[k][l] == 'M')
                {
                    //Console.WriteLine($"Found M at ({k},{l})");
                    bool success =  FindAS(chars, k, l, k - i, l - j);
                    if (success)
                    {
                        successCount++;
                    }
                }
            }
        }

        return successCount;
    }

    private bool FindAS(char[][] chars, int i, int j, int v1, int v2)
    {
        if (i + v1 >= chars.Length || i + v1 < 0 || j + v2 >= chars[i + v1].Length || j + v2 < 0)
        {
            return false;
        }

        if (i + v1 + v1 >= chars.Length || i + v1 + v1 < 0 || j + v2 + v2 >= chars[i + v1 + v1].Length || j + v2 + v2 < 0)
        {
            return false;
        }

        //Console.WriteLine($"Letter at ({i + v1}, {j + v2}) is {chars[i + v1][j + v2]} and at ({i + v1 + v1}, {j + v2 + v2}) is {chars[i + v1 + v1][j + v2 + v2]}");
        bool success = chars[i + v1][j + v2] == 'A' && chars[i + v1 + v1][j + v2 + v2] == 'S';
        if (success)
        {
            //Console.WriteLine("Found AS");
        }
        else
        {
            //Console.WriteLine("Not AS");
        }

        return success;
    }

    internal string SolveB(List<string> input)
    {
        // Parse input
        char[][] chars = new char[input.Count][];

        foreach (var line in input)
        {
            chars[input.IndexOf(line)] = line.ToCharArray();
        }

        // Find all instances of "A"
        int xmasCount = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            for (int j = 0; j < chars[i].Length; j++)
            {
                if (chars[i][j] == 'A')
                {
                    //Console.WriteLine($"Found A at ({i},{j})");
                    xmasCount += CheckX(chars, i, j);
                    //Console.WriteLine($"X-MAS count is now {xmasCount}");
                }
            }
        }

        return xmasCount.ToString();
    }

    private int CheckX(char[][] chars, int i, int j)
    {
        if (i - 1 < 0 || j - 1 < 0 || i + 1 >= chars.Length || j + 1 >= chars[i].Length)
        {
            return 0;
        }

        // Check that top left is either M or S
        if (chars[i - 1][j - 1] != 'M' && chars[i - 1][j - 1] != 'S')
        {
            return 0;
        }

        // Check that bottom left is either M or S
        if (chars[i + 1][j - 1] != 'M' && chars[i + 1][j - 1] != 'S')
        {
            return 0;
        }

        // First check if top left is M
        if (chars[i - 1][j - 1] == 'M')
        {
            // Check that the opposite is S
            if (chars[i + 1][j + 1] != 'S')
            {
                return 0;
            }
        }

        // Then check if bottom right is S
        if (chars[i - 1][j - 1] == 'S')
        {
            // Check that the opposite is M
            if (chars[i + 1][j + 1] != 'M')
            {
                return 0;
            }
        }

        // Then check if bottom left is M
        if (chars[i + 1][j - 1] == 'M')
        {
            // Check that the opposite is S
            if (chars[i - 1][j + 1] != 'S')
            {
                return 0;
            }
        }

        // Then check if bottom left is S
        if (chars[i + 1][j - 1] == 'S')
        {
            // Check that the opposite is M
            if (chars[i - 1][j + 1] != 'M')
            {
                return 0;
            }
        }

        //Console.WriteLine($"{chars[i + 1][j - 1]}.{chars[i + 1][j + 1]}");
        //Console.WriteLine($".A.");
        //Console.WriteLine($"{chars[i - 1][j - 1]}.{chars[i - 1][j + 1]}");

        return 1;
    }
}
