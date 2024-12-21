internal class Day
{
    internal List<string> input;

    public Day(string filename)
    {
        input = LoadInputToLineList(filename);
    }

    public List<string> LoadInputToLineList(string filename)
    {
        // read file one line at a time
        var fileStream = new StreamReader(File.OpenRead(filename));
        List<string> input = new();
        while (!fileStream.EndOfStream)
        {
            var line = fileStream.ReadLine();
            input.Add(line);
        }

        return input;
    }
}