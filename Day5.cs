// read file one line at a time

internal class Day5
{
    public Day5()
    {
    }

    internal string SolveA(List<string> input)
    {
        List<int[]> rules = input.Where(line => line.Contains("|")).Select(line => Array.ConvertAll(line.Split("|"), int.Parse)).ToList();
        List<int[]> printQueue = input.Where(line => line.Contains(",")).Select(line => Array.ConvertAll(line.Split(","), int.Parse)).ToList();
        List<int[]> goodOrderQueues = new List<int[]>();
        
        foreach (var queue in printQueue)
        {
            if (CheckQueue(queue, rules.ToArray()))
            {
                goodOrderQueues.Add(queue);
            }
        }

        var total = goodOrderQueues.Sum(x => x[(int)Math.Floor(x.Length / 2.0)]);
        return total.ToString();
    }

    private bool CheckQueue(int[] queue, int[][] rules)
    {
        var rulesCopy = new List<int[]>(rules);

        foreach (var page in queue)
        {
            rulesCopy.RemoveAll(rule => rule[1] == page && !queue.Contains(rule[0]));

            if (rulesCopy.Any(rule => rule[1] == page))
            {
                return false;
            }

            rulesCopy.RemoveAll(rule => rule[0] == page);
        }

        return true;
    }

    internal string SolveB(List<string> input)
    {
        List<int[]> rules = input.Where(line => line.Contains("|")).Select(line => Array.ConvertAll(line.Split("|"), int.Parse)).ToList();
        List<int[]> printQueue = input.Where(line => line.Contains(",")).Select(line => Array.ConvertAll(line.Split(","), int.Parse)).ToList();
        List<int[]> badOrderQueues = new List<int[]>();
        List<int[]> goodOrderQueues = new List<int[]>();

        foreach (var queue in printQueue)
        {
            if (!CheckQueue(queue, rules.ToArray()))
            {
                badOrderQueues.Add(queue);
            }
        }

        foreach (var queue in badOrderQueues)
        {
            goodOrderQueues.Add(FixQueue(queue, rules.ToArray()));
            //Console.WriteLine(string.Join(',', goodOrderQueues.Last()));
        }

        var total = goodOrderQueues.Sum(x => x[(int)Math.Floor(x.Length / 2.0)]);
        return total.ToString();
    }

    private int[] FixQueue(int[] queue, int[][] rules)
    {
        var relevantRules = rules.Where(rule => queue.Contains(rule[0]) && queue.Contains(rule[1])).ToList();
        List<int> fixedQueue = new List<int>();
        var queueList = queue.ToList();

        while (relevantRules.Any())
        {
            // Find the last number
            foreach (var page in queueList)
            {
                var pageAheadRules = relevantRules.Where(rule => rule[0] == page);
                if (!pageAheadRules.Any())
                {
                    fixedQueue.Insert(0, page);
                    relevantRules.RemoveAll(rule => rule[1] == page);
                    break;
                }
            }

            queueList.Remove(fixedQueue[0]);

            if (queueList.Count == 1)
            {
                fixedQueue.Insert(0, queueList[0]);
                break;
            }
        }

        return fixedQueue.ToArray();
    }
}