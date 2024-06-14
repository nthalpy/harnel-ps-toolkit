namespace ProblemSolving.Templates.SegmentTree
{
    public interface IGenericGroupSegOperation<TElement, TUpdate, TDiff>
    {
        TElement Identity();
        TDiff CreateDiff(TElement element, TUpdate val);
        TElement ApplyDiff(TElement element, TDiff diff);
        TElement Merge(TElement l, TElement r);
    }
}
