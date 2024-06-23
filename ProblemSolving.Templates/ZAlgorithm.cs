namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public static class ZAlgorithm
    {
        public static int[] Z(string s)
        {
            var n = s.Length;
            var z = new int[n];

            z[0] = n;
            var width = 0;
            var idx = 1;

            while (idx < n)
            {
                while (idx + width < n && s[width] == s[idx + width])
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
