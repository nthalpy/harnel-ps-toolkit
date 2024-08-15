using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public static class NumberTheory
    {
        public static long GCD(long x, long y)
        {
            while (x > 0 && y > 0)
                if (x > y) x %= y;
                else y %= x;

            return Math.Max(x, y);
        }

        public static long FastPow(long b, long p, long mod)
        {
            var rv = 1L;
            while (p != 0)
            {
                if ((p & 1) == 1)
                    rv = rv * b % mod;

                b = b * b % mod;
                p >>= 1;
            }

            return rv;
        }

        public static List<(long prime, int count)> Factorize(long v)
        {
            var factors = new List<(long prime, int count)>();

            if (v % 2 == 0)
            {
                var rank2 = 0;
                while (v % 2 == 0)
                {
                    v /= 2;
                    rank2++;
                }

                factors.Add((2, rank2));
            }

            for (var p = 3L; p * p <= v; p += 2)
                if (v % p == 0)
                {
                    var rank = 0;
                    while (v % p == 0)
                    {
                        v /= p;
                        rank++;
                    }

                    factors.Add((p, rank));
                }

            if (v != 1)
                factors.Add((v, 1));

            return factors;
        }

        public static List<long> GetDivisors(List<(long prime, int count)> factors)
        {
            var divisors = new List<long>();
            divisors.Add(1);

            foreach (var (prime, count) in factors)
            {
                var divcount = divisors.Count;
                var ppow = 1L;

                for (var pow = 1; pow <= count; pow++)
                {
                    ppow *= prime;
                    for (var idx = 0; idx < divcount; idx++)
                        divisors.Add(divisors[idx] * ppow);
                }
            }

            return divisors;
        }
    }
}
