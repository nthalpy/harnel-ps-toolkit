﻿namespace ProblemSolving.Templates.LazySegmentTree
{
    [IncludeIfReferenced]
    public sealed class LazySumSeg : GenericLazySeg<long, long, long>
    {
        public LazySumSeg(int size) : base(size)
        {
        }

        protected override long ApplyLazy(int lIncl, int rExcl, long element, long lazy)
        {
            return element + (rExcl - lIncl) * lazy;
        }

        protected override long CreateLazy(int lIncl, int rExcl, long val)
        {
            return val;
        }

        protected override long MergeElement(long l, long r)
        {
            return l + r;
        }

        protected override long MergeLazy(long l, long r)
        {
            return l + r;
        }

        protected override (long left, long right) SplitLazy(long lazy, int stIncl, int mid, int edExcl)
        {
            return (lazy, lazy);
        }
    }
}
