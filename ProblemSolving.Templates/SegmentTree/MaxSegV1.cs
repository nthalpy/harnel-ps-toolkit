using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates.SegmentTree
{
    [IncludeIfReferenced]
    internal sealed class MaxSegV1
    {
        private long[] _tree;
        private int _leafMask;

        public MaxSegV1(int size)
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
                _tree[idx] = Math.Max(_tree[2 * idx], _tree[2 * idx + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(int index, long val)
        {
            var curr = _leafMask | index;
            _tree[curr] = val;
            curr >>= 1;

            while (curr != 0)
            {
                _tree[curr] = Math.Max(_tree[2 * curr], _tree[2 * curr + 1]);
                curr >>= 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Range(int stIncl, int edExcl)
        {
            var leftNode = _leafMask | stIncl;
            var rightNode = _leafMask | edExcl - 1;

            var aggregated = Int64.MinValue;
            while (leftNode <= rightNode)
            {
                if ((leftNode & 1) == 1)
                    aggregated = Math.Max(aggregated, _tree[leftNode++]);
                if ((rightNode & 1) == 0)
                    aggregated = Math.Max(aggregated, _tree[rightNode--]);

                leftNode >>= 1;
                rightNode >>= 1;
            }

            return aggregated;
        }
    }
}
