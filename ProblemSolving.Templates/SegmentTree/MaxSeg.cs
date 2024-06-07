using System;

namespace ProblemSolving.Templates.SegmentTree
{
    [IncludeIfReferenced]
    public sealed class MaxSeg : GenericSeg<long, long>
    {
        public MaxSeg(int size)
            : base(size)
        {
        }

        protected override long Merge(long l, long r)
        {
            return Math.Max(l, r);
        }

        protected override long UpdateElement(long element, long val)
        {
            return Math.Max(element, val);
        }
    }
}
