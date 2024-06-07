using ProblemSolving.Templates.SegmentTree;
using System;
using System.IO;

#nullable disable

public static class Program
{
    public static void Main()
    {
        using var sr = new StreamReader(Console.OpenStandardInput());
        using var sw = new StreamWriter(Console.OpenStandardOutput());

        Solve(sr, sw);
    }

    public static void Solve(StreamReader sr, StreamWriter sw)
    {
        var seg = new MaxSegV2(1024);

    }
}
