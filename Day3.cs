
using System.Text.RegularExpressions;

internal class Day3
{
    internal string SolveA(List<string> input)
    {
        var combinedInput = string.Join("", input);
        var matches = Regex.Matches(combinedInput, @"mul\(\d{1,3},\d{1,3}\)");
        var total = 0;
        foreach (var match in matches)
        {
            var trimmedCommand = match.ToString().Replace("mul(", "").Replace(")", "");
            var ints = trimmedCommand.Split(",");
            total += int.Parse(ints[0]) * int.Parse(ints[1]);
        }

        return total.ToString();
    }

    internal string SolveB(List<string> input)
    {
        var combinedInput = string.Join("", input);
        var regexString = @"(mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\))";
        var match = Regex.Match(combinedInput, regexString);
        var total = 0;

        bool enabled = true;
        while (match.Success)
        {
            if (match.Value.StartsWith("mul") && enabled)
            {
                var trimmedCommand = match.Value.Replace("mul(", "").Replace(")", "");
                var ints = trimmedCommand.Split(",");
                total += int.Parse(ints[0]) * int.Parse(ints[1]);
            }
            else if (match.Value.StartsWith("don't"))
            {
                enabled = false;
            }
            else if (match.Value.StartsWith("do"))
            {
                enabled = true;
            }

            match = match.NextMatch();
        }

        return total.ToString();
    }
}
