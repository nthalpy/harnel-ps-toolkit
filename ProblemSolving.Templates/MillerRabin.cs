namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public class MillerRabin
    {
        public static bool IsPrime(long p)
        {
            if (p == 1)
                return false;

            var q = p - 1;
            var k = 0;
            while (q % 2 == 0)
            {
                q /= 2;
                k++;
            }

            foreach (var a in new long[] { 2, 7, 61 })
            {
                if (a >= p)
                    break;

                var aq = NumberTheory.FastPow(a, q, p);
                if (aq == 1 || aq == p - 1)
                    continue;

                var pass = false;
                for (var step = 0; step < k; step++)
                {
                    aq = (aq * aq) % p;
                    if (aq == p - 1)
                    {
                        pass = true;
                        break;
                    }
                }

                if (!pass)
                    return false;
            }

            return true;
        }
    }
}
