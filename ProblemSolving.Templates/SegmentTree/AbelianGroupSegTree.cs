using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates.SegmentTree
{
    /// <summary>
    /// Generic segment tree w/ Abelian group operations.
    /// </summary>
    [IncludeIfReferenced]
    public abstract class AbelianGroupSegTree<TElement, TUpdate, TDiff>
        where TElement : struct
        where TUpdate : struct
        where TDiff : struct
    {
        protected TElement[] _tree;
        protected int _leafMask;

        public AbelianGroupSegTree(int size)
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
                _tree[idx] = Merge(_tree[2 * idx], _tree[2 * idx + 1]);
        }

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

        public TElement Range(int stIncl, int edExcl)
        {
            var leftNode = _leafMask | stIncl;
            var rightNode = _leafMask | (edExcl - 1);

            var aggregated = Identity();

            while (leftNode <= rightNode)
            {
                if ((leftNode & 1) == 1)
                    aggregated = Merge(aggregated, _tree[leftNode++]);
                if ((rightNode & 1) == 0)
                    aggregated = Merge(aggregated, _tree[rightNode--]);

                leftNode >>= 1;
                rightNode >>= 1;
            }

            return aggregated;
        }

        protected abstract TElement Identity();
        protected abstract TDiff CreateDiff(TElement element, TUpdate val);
        protected abstract TElement ApplyDiff(TElement element, TDiff diff);
        protected abstract TElement Merge(TElement l, TElement r);
    }
}
