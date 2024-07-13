namespace ProblemSolving.Templates.PersistenceSegmentTree.Impl
{
    [IncludeIfReferenced]
    public class PersistenceSumSeg : MonoidPersistenceSegTree<PersistenceSumSeg, long, long>
    {
        public PersistenceSumSeg(int size) : base(size)
        {
        }
        protected PersistenceSumSeg(Node? root, long size) : base(root, size)
        {
        }

        protected override PersistenceSumSeg CreateNew(Node root, long size) => new PersistenceSumSeg(root, size);
        protected override long Identity() => 0;
        protected override long Merge(long l, long r) => l + r;
        protected override long UpdateElement(long elem, long update) => update;
    }
}
