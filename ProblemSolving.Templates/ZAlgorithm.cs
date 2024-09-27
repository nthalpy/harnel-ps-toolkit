using System.Collections.Generic;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public static class ZAlgorithm
    {
        public static int[] Z<T>(IList<T> s)
            where T : struct
        {
            var n = s.Count;
            var z = new int[n];

            z[0] = n;
            var width = 0;
            var idx = 1;

            while (idx < n)
            {
                while (idx + width < n && s[width].Equals(s[idx + width]))
                    width++;

                z[idx] = width;
                if (width == 0)
                {
                    idx++;
                    continue;
                }

                var offset = 1;
                while (offset < width && offset + z[offset] < width)
                {
                    z[idx + offset] = z[offset];
                    offset++;
                }

                idx += offset;
                width -= offset;
            }

            return z;
        }
    }
}
