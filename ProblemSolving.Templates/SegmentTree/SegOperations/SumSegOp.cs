using ProblemSolving.Templates.Merger;

namespace ProblemSolving.Templates.SegmentTree.SegOperations
{
    [IncludeIfReferenced]
    public struct SumSegOp : IAbelianGroupSegOp<long, long, long>
    {
        public long ApplyDiff(long element, long diff) => element + diff;
        public long CreateDiff(long element, long val) => val - element;
        public long Identity() => 0;
        public long Merge(long l, long r) => l + r;
    }
}
