using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public class RollingHash<T>
        where T : struct
    {
        public const long LargePrime1 = 116694184624691L;

        private IList<T> _seq;
        private long _mult;
        private long _mod;

        private long _multinv;
        private long _firstDigitMult;

        public int LeftIncl { get; private set; }
        public int RightExcl { get; private set; }
        public long Hash { get; private set; }

        public RollingHash(IList<T> sequence, long mult, long mod)
        {
            _seq = sequence;
            _mult = mult;
            _mod = mod;

            _multinv = (long)BigInteger.ModPow(_mult, _mod - 2, _mod);
            _firstDigitMult = _multinv;

            LeftIncl = 0;
            RightExcl = 0;
            Hash = 0;
        }

        private long Get(int idx)
        {
            var e = _seq[idx];

            if (typeof(T) == typeof(Char))
                return 1 + (long)Unsafe.As<T, Char>(ref e);
            else if (typeof(T) == typeof(int))
                return 1 + (long)Unsafe.As<T, int>(ref e);
            else if (typeof(T) == typeof(long))
                return 1 + Unsafe.As<T, long>(ref e);
            else
                throw new NotImplementedException();
        }

        public void LeftInc()
        {
            Hash = (Hash + (_mod - _firstDigitMult) * Get(LeftIncl)) % _mod;

            LeftIncl++;
            _firstDigitMult = _firstDigitMult * _multinv % _mod;
        }

        public void LeftDec()
        {
            _firstDigitMult = _firstDigitMult * _mult % _mod;
            Hash = (Hash + Get(LeftIncl - 1) * _firstDigitMult) % _mod;

            LeftIncl--;
        }

        public void RightInc()
        {
            Hash = (Hash * _mult + Get(RightExcl)) % _mod;

            RightExcl++;
            _firstDigitMult = _firstDigitMult * _mult % _mod;
        }

        public void RightDec()
        {
            Hash = (_mod + Hash - Get(RightExcl - 1)) * _multinv % _mod;

            RightExcl--;
            _firstDigitMult = _firstDigitMult * _multinv % _mod;
        }
    }
}
