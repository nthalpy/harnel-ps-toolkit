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
    public sealed class SumSeg
    {
        private long[] _tree;
        private int _leafMask;

        public SumSeg(int size)
        {
            _leafMask = (int)BitOperations.RoundUpToPowerOf2((uint)size);
            var treeSize = _leafMask << 1;

            _tree = new long[treeSize];
        }

        public void Init(IList<long> init)
        {
            for (var idx = 0; idx < init.Count; idx++)
                _tree[_leafMask | idx] = init[idx];

            for (var idx = _leafMask - 1; idx > 0; idx--)
                _tree[idx] = _tree[2 * idx] + _tree[2 * idx + 1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(int index, long val)
        {
            var curr = _leafMask | index;
            var diff = val - _tree[curr];

            while (curr != 0)
            {
                _tree[curr] += diff;
                curr >>= 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Range(int stIncl, int edExcl)
        {
            var leftNode = _leafMask | stIncl;
            var rightNode = _leafMask | edExcl - 1;

            var aggregated = 0L;
            while (leftNode <= rightNode)
            {
                if ((leftNode & 1) == 1)
                    aggregated += _tree[leftNode++];
                if ((rightNode & 1) == 0)
                    aggregated += _tree[rightNode--];

                leftNode >>= 1;
                rightNode >>= 1;
            }

            return aggregated;
        }
    }
}

