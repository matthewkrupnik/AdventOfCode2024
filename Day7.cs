class Day7 : Day
{
    public Day7(string filename = "Day7.txt") : base(filename)
    {
    }

    internal string SolveA()
    {
        List<CalibrationEquatuion> solvable = new List<CalibrationEquatuion>();

        foreach (var line in input)
        {
            var ce = new CalibrationEquatuion(line);
            //Console.WriteLine($"{ce.result}: {string.Join(",", ce.inputs)}");
            bool hasSolution = CheckIfSolvable(ce.result, ce.inputs);
            if (hasSolution) {
                solvable.Add(ce);
                //Console.WriteLine($"{ce.result}: {string.Join(",", ce.inputs)} +++++");
            }
        }

        return solvable.Sum(ce => (long)ce.result).ToString();
    }

    private bool CheckIfSolvable(double previous, double[] inputs)
    {
        //Console.WriteLine("Getting " + previous + " from " + string.Join(", ", inputs));
        if (previous % 1 != 0 || previous < 0)
        {
            return false;
        }

        if (inputs.Length == 1) 
        {
            if (previous == inputs[0])
            {
                return true;
            }
            
            return false;
        }
        
        //Console.WriteLine("Checking add");
        bool checkAdd = CheckIfSolvable(previous - inputs.Last(), inputs.Take(inputs.Length - 1).ToArray());
        //Console.WriteLine("checkAdd: " + checkAdd);
        //Console.WriteLine("Checking mult");
        bool checkMult = CheckIfSolvable(previous / inputs.Last(), inputs.Take(inputs.Length - 1).ToArray());
        //Console.WriteLine("checkMult: " + checkMult);
        return checkAdd || checkMult;
    }

    internal string SolveB()
    {
        List<CalibrationEquatuion> solvable = new List<CalibrationEquatuion>();

        foreach (var line in input)
        {
            var ce = new CalibrationEquatuion(line);
            //Console.WriteLine($"{ce.result}: {string.Join(",", ce.inputs)}");
            bool hasSolution = CheckWithConcatenation(ce.result, ce.inputs);
            if (hasSolution)
            {
                solvable.Add(ce);
                //Console.WriteLine($"{ce.result}: {string.Join(",", ce.inputs)} +++++");
            }
        }

        return solvable.Sum(ce => (long)ce.result).ToString();
    }

    private bool CheckWithConcatenation(double previous, double[] inputs)
    {
        //Console.WriteLine("Getting " + previous + " from " + string.Join(", ", inputs));
        if (previous % 1 != 0 || previous < 0)
        {
            return false;
        }

        if (inputs.Length == 1)
        {
            if (previous == inputs[0])
            {
                return true;
            }

            return false;
        }

        if (CheckWithConcatenation(previous - inputs.Last(), inputs.Take(inputs.Length - 1).ToArray()))
        {
            return true;
        }

        if (CheckWithConcatenation(previous / inputs.Last(), inputs.Take(inputs.Length - 1).ToArray()))
        {
            return true;
        }

        if (previous.ToString().Length > inputs.Last().ToString().Length)
        {
            var trimmed = StupidTrim(previous, inputs.Last());
            if (trimmed == previous)
            {
                return false;
            }

            if (CheckWithConcatenation(trimmed, inputs.Take(inputs.Length - 1).ToArray()))
            {
                Console.WriteLine($"Solved concat {previous} with {inputs.Last()} ({string.Join(", ", inputs)})");
                return true;
            }
        }

        return false;
    }

    private double StupidTrim(double previous, double input)
    {
        var inputChars = input.ToString();
        var previousString = previous.ToString();

        if (previousString.EndsWith(inputChars))
        {
            previousString = previousString.Substring(0, previousString.Length - inputChars.Length);
            //Console.WriteLine($"Trimmed {previous} to {previousString} by {inputChars}");
        }

        return double.Parse(previousString);
    }
}

class CalibrationEquatuion
{
    public CalibrationEquatuion(string input)
    {
        var parts = input.Split(" ");
        result = double.Parse(parts[0].Trim(':'));
        inputs = parts.Skip(1).Select(double.Parse).ToArray();
    }

    public double result { get; private set; }
    public double[] inputs { get; private set; }
}