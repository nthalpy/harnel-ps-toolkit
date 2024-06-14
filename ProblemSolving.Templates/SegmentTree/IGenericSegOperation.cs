namespace ProblemSolving.Templates.SegmentTree
{
    public interface IGenericSegOperation<TElement, TUpdate>
    {
        TElement UpdateElement(TElement elem, TUpdate update);
        TElement Merge(TElement l, TElement r);
    }
}
