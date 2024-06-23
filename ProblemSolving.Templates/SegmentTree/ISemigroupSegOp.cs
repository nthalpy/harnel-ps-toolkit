namespace ProblemSolving.Templates.SegmentTree
{
    public interface ISemigroupSegOp<TElement, TUpdate>
    {
        TElement UpdateElement(TElement elem, TUpdate update);
        TElement Merge(TElement l, TElement r);
    }
}
