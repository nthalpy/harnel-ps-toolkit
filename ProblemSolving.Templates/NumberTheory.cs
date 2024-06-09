using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProblemSolving.Templates.Merger;
using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public static class NumberTheory
    {
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

        /// <summary>
        /// Find b, such that b^p = x
        /// </summary>
        public static bool BabyStepGiantStep(long b, long x, long mod, Dictionary<long, long> baby, out long p)
        {
            baby.Clear();

            var step = (long)Math.Sqrt(mod);

            var babypow = 1L;
            for (var idx = 0; idx < step; idx++)
            {
                if (baby.ContainsKey(babypow))
                    break;

                baby[babypow] = idx;
                babypow = babypow * b % mod;
            }

            var g = FastPow(b, (mod - 2) * step, mod);
            var giantpow = x;
            for (var idx = 0; idx < step + 2; idx++)
            {
                if (baby.TryGetValue(giantpow, out var smolidx))
                {
                    p = smolidx + idx * step;
                    return true;
                }

                giantpow = giantpow * g % mod;
            }

            p = default;
            return false;
        }
    }
}
