namespace ProblemSolving.Templates.SegmentTree.Impl
{
    [IncludeIfReferenced]
    public sealed class CountingSeg : AbelianGroupSegTree<long, long, long>
    {
        public CountingSeg(int size) : base(size)
        {
        }

        /// <summary>
        /// k is 1-based
        /// </summary>
        public int? KthElement(long k)
        {
            if (k <= 0 || Range(0, Size) < k)
                return null;

            var idx = 1;
            while (idx < _leafMask)
            {
                var l = _tree[2 * idx];
                if (l < k)
                {
                    k -= l;
                    idx = 2 * idx + 1;
                }
                else
                {
                    idx = 2 * idx;
                }
            }

            return idx ^ _leafMask;
        }

        protected override long ApplyDiff(long element, long diff) => element + diff;
        protected override long CreateDiff(long element, long val) => val - element;
        protected override long Identity() => 0;
        protected override long Merge(long l, long r) => l + r;
    }
}
