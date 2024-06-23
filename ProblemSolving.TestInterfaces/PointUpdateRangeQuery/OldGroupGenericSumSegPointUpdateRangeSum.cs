using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class OldGroupGenericSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        /// <summary>
        /// Generic segment tree w/ Group operations.
        /// </summary>
        [IncludeIfReferenced]
        public abstract class OldGenericGroupSeg<TElement, TUpdate, TDiff>
            where TElement : struct
            where TUpdate : struct
            where TDiff : struct
        {
            private TElement[] _tree;
            private int _leafMask;

            public OldGenericGroupSeg(int size)
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
                var diff = CreateDiff(_tree[curr], val);

                while (curr != 0)
                {
                    _tree[curr] = ApplyDiff(_tree[curr], diff);
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

            protected abstract TDiff CreateDiff(TElement element, TUpdate val);
            protected abstract TElement ApplyDiff(TElement element, TDiff diff);
            protected abstract TElement Merge(TElement l, TElement r);
        }

        [IncludeIfReferenced]
        public sealed class OldGroupGenericSumSeg : OldGenericGroupSeg<long, long, long>
        {
            public OldGroupGenericSumSeg(int size) : base(size)
            {
            }

            protected override long ApplyDiff(long element, long diff) => element + diff;
            protected override long CreateDiff(long element, long val) => val - element;
            protected override long Merge(long l, long r) => l + r;
        }

        private OldGroupGenericSumSeg _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new OldGroupGenericSumSeg(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
