using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ProblemSolving.Templates.SegmentTree
{
    /// <summary>
    /// Generic segment tree w/ semigroup operations.
    /// </summary>
    [IncludeIfReferenced]
    public abstract class MonoidSegTree<TElement, TUpdate>
        where TElement : struct
        where TUpdate : struct
    {
        protected TElement[] _tree;
        protected int _leafMask;

        public int Size { get; private set; }

        private List<int> _lefts;
        private List<int> _rights;

        public MonoidSegTree(int size)
        {
            _leafMask = (int)BitOperations.RoundUpToPowerOf2((uint)size);
            var treeSize = _leafMask << 1;

            _lefts = new List<int>();
            _rights = new List<int>();

            _tree = new TElement[treeSize];
            Size = size;
        }

        public TElement AllRange => _tree[1];
        public TElement ElementAt(int idx) => _tree[_leafMask | idx];

        public void Init(IList<TElement> init)
        {
            var id = Identity();
            for (var idx = 0; idx < init.Count; idx++)
                _tree[_leafMask | idx] = init[idx];
            for (var idx = init.Count; idx < _leafMask; idx++)
                _tree[_leafMask | idx] = id;

            for (var idx = _leafMask - 1; idx > 0; idx--)
                _tree[idx] = Merge(_tree[2 * idx], _tree[2 * idx + 1]);
        }

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

        public TElement Range(int stIncl, int edExcl)
        {
            if (stIncl >= _leafMask || edExcl > _leafMask)
                throw new ArgumentOutOfRangeException();

            var leftNode = _leafMask | stIncl;
            var rightNode = _leafMask | (edExcl - 1);

            _lefts.Clear();
            _rights.Clear();

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
                aggregated = Merge(aggregated, _tree[idx]);

            return aggregated;
        }

        protected abstract TElement Identity();
        protected abstract TElement UpdateElement(TElement elem, TUpdate update);
        protected abstract TElement Merge(TElement l, TElement r);
    }
}
