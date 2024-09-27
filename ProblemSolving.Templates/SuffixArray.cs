using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public static class SuffixArray
    {
        public static (int[] sa, long[] lcp) GetSuffixArray(IList<int> s)
        {
            var n = s.Count;
            var sa = new int[n];
            var group = new int[n];
            var buf = new int[n];

            for (var idx = 0; idx < n; idx++)
            {
                sa[idx] = idx;
                group[idx] = s[idx];
            }

            var t = 1;
            var comp = Comparer<int>.Create((l, r) =>
            {
                if (group[l] != group[r])
                    return group[l].CompareTo(group[r]);

                var lv = l + t < n ? group[l + t] : -1;
                var rv = r + t < n ? group[r + t] : -1;

                return lv.CompareTo(rv);
            });

            for (t = 1; t <= n; t <<= 1)
            {
                Array.Sort(sa, comp);

                buf[sa[0]] = 0;
                for (var idx = 1; idx < n; idx++)
                    if (comp.Compare(sa[idx], sa[idx - 1]) == 0)
                        buf[sa[idx]] = buf[sa[idx - 1]];
                    else
                        buf[sa[idx]] = 1 + buf[sa[idx - 1]];

                (group, buf) = (buf, group);
            }

            var revsa = new int[n];
            for (var idx = 0; idx < n; idx++)
                revsa[sa[idx]] = idx;

            var lcp = new long[n];
            var lastlcp = 0;

            for (var idx = 0; idx < n; idx++)
            {
                var saidx = revsa[idx];
                if (saidx == 0)
                {
                    lastlcp = 0;
                    continue;
                }

                var v = Math.Max(0, lastlcp - 1);
                var maxoffset = Math.Max(sa[saidx], sa[saidx - 1]);

                while (maxoffset + v < n && s[sa[saidx] + v] == s[sa[saidx - 1] + v])
                    v++;

                lcp[saidx] = v;
                lastlcp = v;
            }

            return (sa, lcp);
        }
    }
}
