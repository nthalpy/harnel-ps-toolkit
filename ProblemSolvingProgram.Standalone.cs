using ProblemSolving.Templates.Merger;
using ProblemSolving.Templates.SegmentTree;
using ProblemSolving.Templates.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

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
        var (n, m, k) = sr.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
        var inits = new long[n];

        for (var idx = 0; idx < n; idx++)
            inits[idx] = Int64.Parse(sr.ReadLine());

        var seg = new GenericSumSeg(n);
        seg.Init(inits);

        for (var idx = 0; idx < m + k; idx++)
        {
            var (a, b, c) = sr.ReadLine().Split(' ').Select(Int32.Parse).ToArray();
            if (a == 1)
            {
                seg.Update(b - 1, c);
            }
            else
            {
                var v = seg.Range(b - 1, c);
                sw.WriteLine(v);
            }
        }
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


namespace ProblemSolving.Templates.SegmentTree
{
    public abstract class GenericSeg<TElement, TUpdate>
        where TElement : struct
        where TUpdate : struct
    {
        private TElement[] _tree;
        private int _leafMask;

        public GenericSeg(int size)
        {
            _leafMask = (int)BitOperations.RoundUpToPowerOf2((uint)size);
            var treeSize = _leafMask << 1;

            _tree = new TElement[treeSize];
        }

        public void Init(IList<TElement> init)
        {
            for (var idx = 0; idx < init.Count; idx++)
                _tree[_leafMask | idx] = init[idx];

            for (var idx = _leafMask - 1; idx > 0; idx--)
                _tree[idx] = Merge(_tree[2 * idx], _tree[2 * idx + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(int index, TUpdate val)
        {
            var curr = _leafMask | index;
            _tree[curr] = UpdateElement(_tree[curr], val);
            curr >>= 1;

            while (curr != 0)
            {
                _tree[curr] = Merge(_tree[2 * curr], _tree[2 * curr + 1]);
                curr >>= 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TElement Range(int stIncl, int edExcl)
        {
            var leftNode = _leafMask | stIncl;
            var rightNode = _leafMask | (edExcl - 1);

            var aggregated = default(TElement);
            var isFirst = true;

            while (leftNode <= rightNode)
            {
                if ((leftNode & 1) == 1)
                {
                    if (isFirst)
                        aggregated = _tree[leftNode++];
                    else
                        aggregated = Merge(aggregated, _tree[leftNode++]);

                    isFirst = false;
                }
                if ((rightNode & 1) == 0)
                {
                    if (isFirst)
                        aggregated = _tree[rightNode--];
                    else
                        aggregated = Merge(aggregated, _tree[rightNode--]);

                    isFirst = false;
                }

                leftNode >>= 1;
                rightNode >>= 1;
            }

            return aggregated;
        }

        protected abstract TElement UpdateElement(TElement element, TUpdate val);
        protected abstract TElement Merge(TElement l, TElement r);
    }
}

