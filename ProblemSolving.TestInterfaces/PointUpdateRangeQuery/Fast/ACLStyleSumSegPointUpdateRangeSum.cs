using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;
using System.Collections.Generic;
using System.Numerics;

namespace ProblemSolving.TestInterfaces.PointUpdateRangeQuery.Fast
{
    public class ACLStyleSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        public interface IAbelianGroupSegOp<TElement, TUpdate, TDiff>
        {
            TElement Identity();
            TDiff CreateDiff(TElement element, TUpdate val);
            TElement ApplyDiff(TElement element, TDiff diff);
            TElement Merge(TElement l, TElement r);
        }

        public class ACLStyleAbelianGroupSegTree<TElement, TUpdate, TDiff, TOp>
            where TElement : struct
            where TUpdate : struct
            where TDiff : struct
            where TOp : struct, IAbelianGroupSegOp<TElement, TUpdate, TDiff>
        {
            private TElement[] _tree;
            private int _leafMask;

            private TOp _op = default;

            public ACLStyleAbelianGroupSegTree(int size)
            {
                _leafMask = (int)BitOperations.RoundUpToPowerOf2((uint)size);
                var treeSize = _leafMask << 1;

                _tree = new TElement[treeSize];
            }

            public TElement AllRange => _tree[1];
            public TElement ElementAt(int idx) => _tree[_leafMask | idx];

            public void Init(IList<TElement> init)
            {
                for (var idx = 0; idx < init.Count; idx++)
                    _tree[_leafMask | idx] = init[idx];

                for (var idx = _leafMask - 1; idx > 0; idx--)
                    _tree[idx] = _op.Merge(_tree[2 * idx], _tree[2 * idx + 1]);
            }

            public void Update(int index, TUpdate val)
            {
                var curr = _leafMask | index;
                var diff = _op.CreateDiff(_tree[curr], val);

                while (curr != 0)
                {
                    _tree[curr] = _op.ApplyDiff(_tree[curr], diff);
                    curr >>= 1;
                }
            }

            public TElement Range(int stIncl, int edExcl)
            {
                var leftNode = _leafMask | stIncl;
                var rightNode = _leafMask | (edExcl - 1);

                var aggregated = _op.Identity();

                while (leftNode <= rightNode)
                {
                    if ((leftNode & 1) == 1)
                        aggregated = _op.Merge(aggregated, _tree[leftNode++]);
                    if ((rightNode & 1) == 0)
                        aggregated = _op.Merge(aggregated, _tree[rightNode--]);

                    leftNode >>= 1;
                    rightNode >>= 1;
                }

                return aggregated;
            }
        }

        public struct SumSegOp : IAbelianGroupSegOp<long, long, long>
        {
            public long ApplyDiff(long element, long diff) => element + diff;
            public long CreateDiff(long element, long val) => val - element;
            public long Identity() => 0;
            public long Merge(long l, long r) => l + r;
        }

        private ACLStyleAbelianGroupSegTree<long, long, long, SumSegOp> _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new ACLStyleAbelianGroupSegTree<long, long, long, SumSegOp>(size);
        }

        public override void PointUpdate(int index, long val) => _seg.Update(index, val);
        public override long RangeSum(int stIncl, int edExcl) => _seg.Range(stIncl, edExcl);
    }
}
