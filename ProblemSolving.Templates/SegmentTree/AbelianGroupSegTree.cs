using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates.SegmentTree
{
    /// <summary>
    /// Generic segment tree w/ Abelian group operations.
    /// </summary>
    [IncludeIfReferenced]
    public class AbelianGroupSegTree<TElement, TUpdate, TDiff, TOp>
        where TElement : struct
        where TUpdate : struct
        where TDiff : struct
        where TOp : struct, IAbelianGroupSegOp<TElement, TUpdate, TDiff>
    {
        private TElement[] _tree;
        private int _leafMask;

        private TOp _op = default;

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
                _tree[idx] = _op.Merge(_tree[2 * idx], _tree[2 * idx + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
}
