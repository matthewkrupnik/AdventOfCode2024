
internal class Day1
{
    internal string SolveA(List<string> input)
    {
        var distance = 0;
        var parsedInput = ParseInput(input);
        parsedInput.Item1.Sort();
        parsedInput.Item2.Sort();
        for (int i = 0; i < parsedInput.Item1.Count; i++)
        {
            distance += Math.Abs(parsedInput.Item1[i] - parsedInput.Item2[i]);
        }

        return distance.ToString();
    }

    internal string SolveB(List<string> input)
    {
        var similarity = 0;
        var parsedInput = ParseInput(input);

        for (int i = 0; i < parsedInput.Item1.Count; i++)
        {
            similarity += parsedInput.Item1[i] * parsedInput.Item2.Count(x => x == parsedInput.Item1[i]);
        }

        return similarity.ToString();
    }

    internal Tuple<List<int>, List<int>> ParseInput(List<string> input)
    {
        List<int> a = new List<int>();
        List<int> b = new List<int>();
        foreach (var item in input)
        {
            var splitItem = item.Split("   ");
            a.Add(int.Parse(splitItem[0].Trim()));
            b.Add(int.Parse(splitItem[1].Trim()));
        }

        return Tuple.Create(a, b);
    }
}
