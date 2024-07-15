namespace ProblemSolving.Templates.SegmentTree.Impl
{
    [IncludeIfReferenced]
    public sealed class ModSumSeg : AbelianGroupSegTree<long, long, long>
    {
        private int _mod;

        public ModSumSeg(int size, int mod) : base(size)
        {
            _mod = mod;
        }

        protected override long ApplyDiff(long element, long diff) => (element + diff) % _mod;
        protected override long CreateDiff(long element, long val) => (val + _mod - element) % _mod;
        protected override long Identity() => 0;
        protected override long Merge(long l, long r) => (l + r) % _mod;
    }
}
