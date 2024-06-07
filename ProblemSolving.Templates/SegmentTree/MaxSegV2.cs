using System;

namespace ProblemSolving.Templates.SegmentTree
{
    [IncludeIfReferenced]
    public sealed class MaxSegV2 : GenericSeg<long, long>
    {
        public MaxSegV2(int size)
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
