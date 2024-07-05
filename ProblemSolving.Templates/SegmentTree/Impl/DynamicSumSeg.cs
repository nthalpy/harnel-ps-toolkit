namespace ProblemSolving.Templates.SegmentTree.Impl
{
    [IncludeIfReferenced]
    public sealed class DynamicSumSeg : DynamicAbelianGroupSegTree<long, long, long>
    {
        public DynamicSumSeg(int size) : base(size)
        {
        }

        protected override long ApplyDiff(long element, long diff) => element + diff;
        protected override long CreateDiff(long element, long val) => val - element;
        protected override long Identity() => 0;
        protected override long Merge(long l, long r) => l + r;
    }
}
