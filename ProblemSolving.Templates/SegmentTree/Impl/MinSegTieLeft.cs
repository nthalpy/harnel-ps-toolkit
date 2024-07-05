using System.Collections.Generic;

namespace ProblemSolving.Templates.SegmentTree.Impl
{
    [IncludeIfReferenced]
    public class MinSegTieLeft : SemigroupSegTree<(int idx, long val), long>
    {
        public MinSegTieLeft(int size) : base(size)
        {
        }

        public void Init(long init)
        {
            for (var idx = 0; idx < _leafMask; idx++)
                _tree[_leafMask | idx] = (idx, init);

            for (var idx = _leafMask - 1; idx > 0; idx--)
                _tree[idx] = Merge(_tree[2 * idx], _tree[2 * idx + 1]);
        }
        public void Init(IList<long> init)
        {
            for (var idx = 0; idx < init.Count; idx++)
                _tree[_leafMask | idx] = (idx, init[idx]);

            for (var idx = _leafMask - 1; idx > 0; idx--)
                _tree[idx] = Merge(_tree[2 * idx], _tree[2 * idx + 1]);
        }

        protected override (int idx, long val) Merge((int idx, long val) l, (int idx, long val) r)
        {
            return l.val <= r.val ? l : r;
        }

        protected override (int idx, long val) UpdateElement((int idx, long val) elem, long update)
        {
            return (elem.idx, update);
        }
    }
}
