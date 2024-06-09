using ProblemSolving.Templates.Merger;
using ProblemSolving.Templates.Splay;
using ProblemSolving.Templates.Utility;
using System;
using System.IO;
using System.Linq;
namespace ProblemSolving.Templates.Merger {}
namespace ProblemSolving.Templates.Splay {}
namespace ProblemSolving.Templates.Utility {}
namespace System {}
namespace System.IO {}
namespace System.Linq {}

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


namespace ProblemSolving.Templates.Utility
{
    public static class DeconstructHelper
    {
        public static void Deconstruct<T>(this T[] arr, out T v1, out T v2) => (v1, v2) = (arr[0], arr[1]);
        public static void Deconstruct<T>(this T[] arr, out T v1, out T v2, out T v3) => (v1, v2, v3) = (arr[0], arr[1], arr[2]);
        public static void Deconstruct<T>(this T[] arr, out T v1, out T v2, out T v3, out T v4) => (v1, v2, v3, v4) = (arr[0], arr[1], arr[2], arr[3]);
        public static void Deconstruct<T>(this T[] arr, out T v1, out T v2, out T v3, out T v4, out T v5) => (v1, v2, v3, v4, v5) = (arr[0], arr[1], arr[2], arr[3], arr[4]);
        public static void Deconstruct<T>(this T[] arr, out T v1, out T v2, out T v3, out T v4, out T v5, out T v6) => (v1, v2, v3, v4, v5, v6) = (arr[0], arr[1], arr[2], arr[3], arr[4], arr[5]);
        public static void Deconstruct<T>(this T[] arr, out T v1, out T v2, out T v3, out T v4, out T v5, out T v6, out T v7) => (v1, v2, v3, v4, v5, v6, v7) = (arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6]);
        public static void Deconstruct<T>(this T[] arr, out T v1, out T v2, out T v3, out T v4, out T v5, out T v6, out T v7, out T v8) => (v1, v2, v3, v4, v5, v6, v7, v8) = (arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7]);
    }
}


namespace ProblemSolving.Templates.Splay
{
    public class SplayTree<TNode> where TNode : SplayNode<TNode>
    {
        protected TNode? _root;

        public int NodeCount => _root?.SubtreeSize ?? 0;

        public SplayTree()
        {
        }

        protected void Rotate(TNode x)
        {
            var p = x.Parent;
            if (p == null)
                return;

            TNode? b;
            if (x.IsLeftChild)
            {
                b = x.Right;
                (p.Left, x.Right) = (x.Right, p);
            }
            else
            {
                b = x.Left;
                (p.Right, x.Left) = (x.Left, p);
            }

            (x.Parent, p.Parent) = (p.Parent, x);

            if (b != null)
                b.Parent = p;

            if (x.Parent != null)
            {
                if (p == x.Parent.Left)
                    x.Parent.Left = x;
                else
                    x.Parent.Right = x;
            }
            else
            {
                _root = x;
            }

            p.UpdateTracked();
            x.UpdateTracked();
        }

        /// <summary>
        /// Attach x to target, when target is ancestor of x.
        /// </summary>
        protected void Splay(TNode x, TNode? target = null)
        {
            x.UpdateTracked();

            while (x.Parent != target)
            {
                var p = x.Parent;
                var g = p!.Parent;

                if (g == target)
                {
                    Rotate(x);
                    return;
                }

                if (g != null)
                    Rotate((x.IsLeftChild == p.IsLeftChild) ? p : x);

                Rotate(x);
            }

            if (target == null)
                _root = x;
        }

        /// <summary>
        /// index is 0-based.
        /// </summary>
        public void InsertKth(int index, TNode node)
        {
            if (_root == null)
            {
                _root = node;
                node.UpdateTracked();
                return;
            }

            if (index == 0)
            {
                var curr = _root;
                while (curr!.Left != null)
                    curr = curr.Left;

                (curr.Left, node.Parent) = (node, curr);
            }
            else
            {
                var k = FindKth(index - 1, false)!;

                if (k.Right == null)
                {
                    (k.Right, node.Parent) = (node, k);
                }
                else
                {
                    k = k.Right;
                    while (k.Left != null)
                        k = k.Left;

                    (k.Left, node.Parent) = (node, k);
                }
            }

            Splay(node);
        }

        /// <summary>
        /// index is 0-based.
        /// </summary>
        public TNode? FindKth(int index, bool splay)
        {
            index++;

            if (index == 0 || _root == null || _root.SubtreeSize < index)
                return null;

            var curr = _root;
            while (true)
            {
                var lsize = curr!.Left?.SubtreeSize ?? 0;

                if (index <= lsize)
                {
                    curr = curr.Left;
                }
                else if (index == lsize + 1)
                {
                    if (splay)
                        Splay(curr);

                    return curr;
                }
                else
                {
                    index -= lsize + 1;
                    curr = curr.Right;
                }
            }
        }

        /// <summary>
        /// index is 0-based.
        /// </summary>
        public void RemoveKth(int index)
        {
            var k = FindKth(index, true)!;

            if (k.Left != null && k.Right != null)
            {
                var l = k.Left;
                var r = k.Right;

                _root = l;
                _root.Parent = null;
                _root.UpdateTracked();

                while (l.Right != null)
                    l = l.Right;

                (l.Right, r.Parent) = (r, l);
                r.UpdateTracked();
                Splay(r);
            }
            else if (k.Left != null)
            {
                _root = k.Left;
                _root.Parent = null;
                _root.UpdateTracked();
            }
            else if (k.Right != null)
            {
                _root = k.Right;
                _root.Parent = null;
                _root.UpdateTracked();
            }
            else
            {
                _root = null;
            }
        }

        public TNode? Gather(int stIncl, int edExcl)
        {
            var right = FindKth(edExcl, true);
            var left = FindKth(stIncl - 1, true);

            if (right == null)
            {
                if (left == null)
                {
                    return _root;
                }
                else
                {
                    Splay(left);
                    return left.Right;
                }
            }
            else
            {
                if (left != null)
                    Splay(left);

                Splay(right, left);
                return right.Left;
            }
        }
    }
}


namespace ProblemSolving.Templates.Splay
{
    public abstract class SplayNode<TSelf> where TSelf : SplayNode<TSelf>
    {
        public TSelf? Parent;
        public TSelf? Left;
        public TSelf? Right;

        public bool IsLeftChild => Parent?.Left == this;
        public bool IsRightChild => Parent?.Right == this;

        public int SubtreeSize;

        public virtual void UpdateTracked()
        {
            SubtreeSize = 1 + (Left?.SubtreeSize ?? 0) + (Right?.SubtreeSize ?? 0);
        }
    }
}

// This is source code merged w/ template
// Timestamp: 2024-06-09 14:49:28 UTC+9
