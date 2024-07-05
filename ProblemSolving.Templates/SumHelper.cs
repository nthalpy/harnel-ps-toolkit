using System;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public static class SumHelper
    {
        /// <summary>
        /// Returns {a+st*b + a+(st+1)*b + ... + a+ed*b} % mod
        /// </summary>
        public static long ArithmetricSum(long a, long b, long st, long ed, long mod)
        {
            var l = (ed - st + 1);
            if (l % 2 == 0)
            {
                var halfl = l / 2;
                halfl %= mod;

                var r = (2 * a + b * (st + ed)) % mod;
                return halfl * r % mod;
            }
            else
            {
                l %= mod;
                var halfr = (a + b * (st + ed) / 2) % mod;
                return l * halfr % mod;
            }
        }

        /// <summary>
        /// Returns {1+p+..p^(n-1)} % mod
        /// </summary>
        public static long GeometricSum(long p, long n, long mod)
        {
            p %= mod;

            if (n <= 0)
                throw new ArgumentException();

            if (n == 1)
                return 1;
            if (n == 2)
                return (1 + p) % mod;

            if (n % 2 == 0)
            {
                // 1+p+...+p^{2k-1} = (1+p^k)(1+p+...+p^{k-1})
                var k = n / 2;

                return (1 + NumberTheory.FastPow(p, k, mod)) * GeometricSum(p, k, mod) % mod;
            }
            else
            {
                // 1+p+...+p^{n-1} = 1 + p(1+p+...+p^{n-2})

                return (1 + p * GeometricSum(p, n - 1, mod)) % mod;
            }
        }
    }
}
