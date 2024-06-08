using ProblemSolving.Templates.Splay;
using ProblemSolving.Templates.Utility;
using System;
using System.IO;
using System.Linq;

#nullable disable

public sealed class CharSplayNode : SplayNode<CharSplayNode>
{
    public Char Ch;

    public CharSplayNode(char ch)
    {
        Ch = ch;
    }
}
public sealed class StringSplayTree : SplayTree<CharSplayNode>
{
    public CharSplayNode Extract(int stIncl, int edExcl)
    {
        var g = Gather(stIncl, edExcl);

        if (g == _root)
        {
            _root = null;
        }
        else
        {
            var p = g.Parent!;

            if (g.IsLeftChild)
                p.Left = null;
            else
                p.Right = null;

            Splay(p);
        }

        return g;
    }
}

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
        var (n, qc) = sr.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
        var s = sr.ReadLine();
        var splay = new StringSplayTree();

        foreach (var ch in s)
            splay.InsertKth(splay.NodeCount, new CharSplayNode(ch));

        while (qc-- > 0)
        {
            var q = sr.ReadLine().Split(' ');
            var type = Int32.Parse(q[0]);

            if (type == 1)
            {
                var node = new CharSplayNode(q[1][0]);
                var pos = Int32.Parse(q[2]);

                splay.InsertKth(pos - 1, node);
            }
            else
            {
                var (stIncl, edIncl) = (Int32.Parse(q[1]), Int32.Parse(q[2]));
                var k = splay.Extract(stIncl - 1, edIncl);

                Print(sw, k);
                sw.WriteLine();
            }
        }
    }

    private static void Print(StreamWriter sw, CharSplayNode k)
    {
        if (k.Left != null)
            Print(sw, k.Left);

        sw.Write(k.Ch);

        if (k.Right != null)
            Print(sw, k.Right);
    }
}
