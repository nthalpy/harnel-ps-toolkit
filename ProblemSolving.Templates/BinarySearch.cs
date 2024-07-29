using System;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public static class BinarySearch
    {
        public static int Count(int stIncl, int edExcl, Predicate<int> pred)
        {
            var lo = stIncl;
            var hi = edExcl - 1;

            var flo = pred(lo);
            var fhi = pred(hi);

            if (flo == fhi)
                return flo ? (edExcl - stIncl) : 0;

            while (lo + 1 < hi)
            {
                var mid = (lo + hi) / 2;
                var fmid = pred(mid);

                if (flo == fmid)
                    lo = mid;
                else
                    hi = mid;
            }

            return flo ? (lo + 1 - stIncl) : (edExcl - hi);
        }

        public static bool TryFindMin(int stIncl, int edExcl, Predicate<int> pred, out int idx)
        {
            var lo = stIncl;
            var hi = edExcl - 1;

            var flo = pred(lo);
            var fhi = pred(hi);

            if (flo == fhi)
            {
                idx = lo;
                return flo;
            }

            while (lo + 1 < hi)
            {
                var mid = (lo + hi) / 2;
                var fmid = pred(mid);

                if (flo == fmid)
                    lo = mid;
                else
                    hi = mid;
            }

            idx = flo ? lo : hi;
            return true;
        }

        public static bool TryFindMax(int stIncl, int edExcl, Predicate<int> pred, out int idx)
        {
            var lo = stIncl;
            var hi = edExcl - 1;

            var flo = pred(lo);
            var fhi = pred(hi);

            if (flo == fhi)
            {
                idx = hi;
                return flo;
            }

            while (lo + 1 < hi)
            {
                var mid = (lo + hi) / 2;
                var fmid = pred(mid);

                if (flo == fmid)
                    lo = mid;
                else
                    hi = mid;
            }

            idx = flo ? lo : hi;
            return true;
        }
    }
}
