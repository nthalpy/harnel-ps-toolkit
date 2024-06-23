using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class OldGenericSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        [IncludeIfReferenced]
        public abstract class OldGenericSeg<TElement, TUpdate>
            where TElement : struct
            where TUpdate : struct
        {
            private TElement[] _tree;
            private int _leafMask;

            public OldGenericSeg(int size)
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

        [IncludeIfReferenced]
        public sealed class OldGenericSumSeg : OldGenericSeg<long, long>
        {
            public OldGenericSumSeg(int size) : base(size)
            {
            }

            protected override long Merge(long l, long r) => l + r;
            protected override long UpdateElement(long element, long val) => val;
        }

        private OldGenericSumSeg _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new OldGenericSumSeg(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
