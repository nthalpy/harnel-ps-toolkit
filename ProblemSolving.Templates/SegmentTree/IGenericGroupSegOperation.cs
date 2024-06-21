using ProblemSolving.Templates.Merger;

namespace ProblemSolving.Templates.SegmentTree
{
    [IncludeIfReferenced]
    public interface IGenericGroupSegOperation<TElement, TUpdate, TDiff>
    {
        TElement Identity();
        TDiff CreateDiff(TElement element, TUpdate val);
        TElement ApplyDiff(TElement element, TDiff diff);
        TElement Merge(TElement l, TElement r);
    }
}
