using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class SumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        [IncludeIfReferenced]
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
                    curr /= 2;
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

        private SumSeg _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new SumSeg(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
