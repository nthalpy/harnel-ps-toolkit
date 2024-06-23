using ProblemSolving.Templates.Merger;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates.SegmentTree
{
    [IncludeIfReferenced]
    public class SemigroupSegTree<TElement, TUpdate, TOperation>
        where TElement : struct
        where TUpdate : struct
        where TOperation : struct, ISemigroupSegOp<TElement, TUpdate>
    {
        private TElement[] _tree;
        private int _leafMask;

        private List<int> _lefts;
        private List<int> _rights;

        private TOperation _op = default;

        public SemigroupSegTree(int size)
        {
            _leafMask = (int)BitOperations.RoundUpToPowerOf2((uint)size);
            var treeSize = _leafMask << 1;

            _lefts = new List<int>();
            _rights = new List<int>();

            _tree = new TElement[treeSize];
        }

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
            _tree[curr] = _op.UpdateElement(_tree[curr], val);
            curr >>= 1;

            while (curr != 0)
            {
                _tree[curr] = _op.Merge(_tree[2 * curr], _tree[2 * curr + 1]);
                curr >>= 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TElement Range(int stIncl, int edExcl)
        {
            var leftNode = _leafMask | stIncl;
            var rightNode = _leafMask | (edExcl - 1);

            while (leftNode <= rightNode)
            {
                if ((leftNode & 1) == 1)
                    _lefts.Add(leftNode++);
                if ((rightNode & 1) == 0)
                    _rights.Add(rightNode--);

                leftNode >>= 1;
                rightNode >>= 1;
            }

            foreach (var idx in _rights.AsEnumerable().Reverse())
                _lefts.Add(idx);

            var aggregated = _tree[_lefts[0]];
            foreach (var idx in _lefts.Skip(1))
                aggregated = _op.Merge(aggregated, _tree[idx]);

            return aggregated;
        }
    }
}
