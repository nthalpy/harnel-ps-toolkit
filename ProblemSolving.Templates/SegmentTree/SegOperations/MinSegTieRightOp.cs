namespace ProblemSolving.Templates.SegmentTree.SegOperations
{
    [IncludeIfReferenced]
    public struct MinSegTieRightOp : ISemigroupSegOp<(int idx, long val), (int idx, long val)>
    {
        public (int idx, long val) Merge((int idx, long val) l, (int idx, long val) r) => l.val < r.val ? l : r;
        public (int idx, long val) UpdateElement((int idx, long val) elem, (int idx, long val) update) => update;
    }
}
