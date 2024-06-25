namespace ProblemSolving.Templates.LazySegmentTree.Impl
{
    [IncludeIfReferenced]
    public sealed class LazySumSeg : GenericLazySeg<long, long, long>
    {
        public LazySumSeg(int size) : base(size)
        {
        }

        protected override long ApplyLazy(int lIncl, int rExcl, long element, long lazy)
            => element + (rExcl - lIncl) * lazy;
        protected override long CreateLazy(int lIncl, int rExcl, long val) => val;
        protected override long MergeLazy(long l, long r) => l + r;
        protected override (long left, long right) SplitLazy(long lazy, int stIncl, int mid, int edExcl)
            => (lazy, lazy);

        protected override long MergeElement(long l, long r) => l + r;
    }
}
