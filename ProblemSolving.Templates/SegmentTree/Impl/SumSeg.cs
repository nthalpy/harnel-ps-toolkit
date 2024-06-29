namespace ProblemSolving.Templates.SegmentTree.Impl
{
    [IncludeIfReferenced]
    public sealed class SumSeg : AbelianGroupSegTree<long, long, long>
    {
        public SumSeg(int size) : base(size)
        {
        }

        protected override long ApplyDiff(long element, long diff) => element + diff;
        protected override long CreateDiff(long element, long val) => val - element;
        protected override long Identity() => 0;
        protected override long Merge(long l, long r) => l + r;
    }
}
