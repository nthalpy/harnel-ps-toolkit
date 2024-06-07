using ProblemSolving.Templates.SegmentTree;
using ProblemSolving.Templates.Utility;
using System;
using System.IO;
using System.Linq;

#nullable disable

public static class Program
{
    public static void Main()
    {
        using var sr = new StreamReader(Console.OpenStandardInput(), bufferSize: 65536);
        using var sw = new StreamWriter(Console.OpenStandardOutput(), bufferSize: 65536);

        Solve(sr, sw);
    }

    public static void Solve(StreamReader sr, StreamWriter sw)
    {
        var (n, m, k) = sr.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
        var inits = new long[n];

        for (var idx = 0; idx < n; idx++)
            inits[idx] = Int64.Parse(sr.ReadLine());

        var seg = new SumSeg(n);
        seg.Init(inits);

        for (var idx = 0; idx < m + k; idx++)
        {
            var (a, b, c) = sr.ReadLine().Split(' ').Select(Int64.Parse).ToArray();
            if (a == 1)
            {
                seg.Update((int)b - 1, c);
            }
            else
            {
                var v = seg.Range((int)b - 1, (int)c);
                sw.WriteLine(v);
            }
        }
    }
}
