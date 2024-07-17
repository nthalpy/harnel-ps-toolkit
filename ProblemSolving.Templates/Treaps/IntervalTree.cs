using System;

namespace ProblemSolving.Templates.Treaps
{
    [IncludeIfReferenced]
    public class IntervalTree : Treap<(long StIncl, long EdExcl)>
    {
        public IntervalTree()
            : base(0)
        {
        }

        public bool FindRangeFromPoint(long point, out long stIncl, out long edExcl)
        {
            (stIncl, edExcl) = (default, default);

            var curr = _root;
            while (curr != null)
            {
                if (curr.Key.StIncl <= point && point < curr.Key.EdExcl)
                {
                    (stIncl, edExcl) = curr.Key;
                    return true;
                }
                else if (curr.Key.EdExcl <= point)
                {
                    curr = curr.Right;
                }
                else if (point < curr.Key.StIncl)
                {
                    curr = curr.Left;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return false;
        }
    }
}
