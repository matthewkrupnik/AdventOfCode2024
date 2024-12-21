using System.Linq;
using System.Text;

internal class Day2
{
    internal string SolveA(List<string> input)
    {
        var reports = ParseInput(input);
        int safeCount = 0;
        StringBuilder sb = new StringBuilder();

        foreach (var report in reports)
        {
            var safe = AnalyzeReport(report);
            if (safe)
            {
                safeCount++;
                sb.AppendLine(string.Join(' ', report));
            }
        }

        //Console.WriteLine(sb.ToString());
        return safeCount.ToString();
    }

    private bool AnalyzeReport(List<int> report)
    {
        var increasing = false;
        for (int i = 0; i < report.Count - 1; i++)
        {
            var diff = report[i] - report[i + 1];

            if (diff == 0 || Math.Abs(diff) > 3)
            {
                return false;
            }

            if (i == 0 && diff < 0)
            {
                increasing = true;
            }

            if (i > 0 && ((diff > 0 && increasing) || (diff < 0 && !increasing)))
            {
                return false;
            }
        }

        return true;
    }

    internal string SolveB(List<string> input)
    {
        var reports = ParseInputWithDiff(input);
        int safeCount = 0;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < reports.Count; i++)
        {
            //var reportString = string.Join(' ', reports[i]);
            sb.AppendLine(input[i]);
            //sb.AppendLine(reportString);
            var safe = AnalyzeDiffReportWithSkip(reports[i], sb);
            if (safe)
            {
                sb.AppendLine("+++Safe+++");
                safeCount++;
            }
            else sb.AppendLine("---UNSAFE---");
            sb.AppendLine();
        }

        //Console.WriteLine(sb.ToString());
        File.WriteAllText("out.txt", sb.ToString());
        return safeCount.ToString();
    }

    private bool AnalyzeDiffReportWithSkip(List<int> report, StringBuilder sb, bool allowSkip = true)
    {
        sb.AppendLine(string.Join(' ', report));

        var increasing = report[0] < 0;
        // Removing dupes does not impact diffs with neighbours
        if (report.Contains(0))
        {
            if (!allowSkip)
            {
                return false;
            }

            var zeroIndex = report.IndexOf(0);
            report.RemoveAt(zeroIndex);
            sb.AppendLine($"Removing '0' at {zeroIndex}");
            return AnalyzeDiffReportWithSkip(report, sb, false);
        }

        for (int i = 0; i < report.Count; i++)
        {
            var safe = true;
            if (Math.Abs(report[i]) > 3)
            {
                safe = false;
                sb.AppendLine($"Diff too large {report[i]}");
            }

            if ((report[i] > 0 && increasing) || (report[i] < 0 && !increasing))
            {
                sb.AppendLine($"Against the trend {increasing} {report[i]}");
                safe = false;
            }

            if (!safe)
            {
                if (!allowSkip)
                {
                    return false;
                }

                return CheckWithAFix(i, report, sb);
            }
        }

        return true;
    }

    private bool CheckWithAFix(int i, List<int> report, StringBuilder sb)
    {
        var reportClone = new List<int>(report.ToArray());
        bool safe;

        // If first diff is off, lets try removing first element so first diff is removed
        if (i <= 1)
        {
            sb.AppendLine($"Removing {reportClone[0]} at 0");
            reportClone.RemoveAt(0);
            safe = AnalyzeDiffReportWithSkip(reportClone, sb, false);
            if (safe) return true;
        }

        reportClone = new List<int>(report.ToArray());
        if (i < reportClone.Count - 1)
        {
            reportClone[i + 1] += reportClone[i];
        }
        sb.AppendLine($"Removing {reportClone[i]} at {i}");
        reportClone.RemoveAt(i);
        safe = AnalyzeDiffReportWithSkip(reportClone, sb, false);
        if (safe) return true;

        reportClone = new List<int>(report.ToArray());
        if (i > 0)
        {
            reportClone[i] += reportClone[i - 1];
            sb.AppendLine($"Removing {reportClone[i - 1]} at {i - 1}");
            reportClone.RemoveAt(i - 1);
            safe = AnalyzeDiffReportWithSkip(reportClone, sb, false);
            if (safe)
            {
                return true;
            }
        }

        reportClone = new List<int>(report.ToArray());
        if (i < report.Count - 2)
        {
            reportClone[i + 2] += reportClone[i + 1];
            sb.AppendLine($"Removing {reportClone[i + 1]} at {i + 1}");
            reportClone.RemoveAt(i + 1);
            safe = AnalyzeDiffReportWithSkip(reportClone, sb, false);
            if (safe)
            {
                return true;
            }
        }

        return false;
    }

    private bool AnalyzeReportWithSkip(List<int> report, StringBuilder sb)
    {
        var increasing = false;
        var allowSkip = true;
        var trendReversal = false;


        for (int i = 0; i < report.Count - 1; i++)
        {
            var a = report[i];
            var b = report[i + 1];
            var diff = a - b;

            if (i==0 && diff < 0)
            {
                increasing = true;
            }

            var safe = true;
            if (diff == 0 || Math.Abs(diff) > 3)
            {
                safe = false;
                sb.AppendLine($"Invalid diff {diff} between {a} and {b}");
            }

            if ((diff > 0 && increasing) || (diff < 0 && !increasing))
            {
                safe = false;
                trendReversal = true;
                sb.AppendLine($"Reversal of trend {increasing} with diff {diff} between {a} and {b}");
            }

            if (!safe)
            {
                if (allowSkip)
                {
                    allowSkip = false;
                    if (i == 1 && trendReversal)
                    {
                        i--;
                    }
                    sb.AppendLine($"Removing {report[i + 1]}");
                    report.RemoveAt(i + 1);
                    i--;
                } else
                {
                    sb.AppendLine("UNSAFE");
                    return false;
                }
            }
        }

        sb.AppendLine("Safe");
        return true;
    }

    internal List<List<int>> ParseInput(List<string> input)
    {
        var parsed = new List<List<int>>();
        foreach (var item in input)
        {
            var report = new List<int>();
            var splitItem = item.Split(' ');
            foreach (var level in splitItem)
            {
                report.Add(int.Parse(level));
            }
            parsed.Add(report);
        }

        return parsed;
    }

    internal List<List<int>> ParseInputWithDiff(List<string> input)
    {
        var parsed = new List<List<int>>();
        foreach (var item in input)
        {
            var report = new List<int>();
            var splitItem = item.Split(' ');
            for (var i = 0; i < splitItem.Length -1; i++)
            {
                report.Add(int.Parse(splitItem[i]) - int.Parse(splitItem[i+1]));
            }
            parsed.Add(report);
        }

        return parsed;
    }
}