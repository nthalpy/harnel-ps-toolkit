using ProblemSolving.Templates.Merger;
using System;
using System.Runtime.CompilerServices;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public struct Frac : IEquatable<Frac>, IComparable<Frac>
    {
        public long P;
        public long Q;

        public Frac(long p, long q)
        {
            var g = GCD(Math.Abs(p), Math.Abs(q));
            var s = q < 0 ? -1 : 1;

            P = s * p / g;
            Q = s * q / g;
        }

        public static implicit operator Frac(long v) => new Frac(v, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Frac operator +(Frac l, Frac r) => new Frac(l.P * r.Q + l.Q * r.P, l.Q * r.Q);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Frac operator -(Frac l, Frac r) => new Frac(l.P * r.Q - l.Q * r.P, l.Q * r.Q);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Frac operator *(Frac l, Frac r) => new Frac(l.P * r.P, l.Q * r.Q);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Frac operator /(Frac l, Frac r) => new Frac(l.P * r.Q, l.Q * r.P);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <(Frac l, Frac r) => l.P * r.Q - l.Q * r.P < 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <=(Frac l, Frac r) => l.P * r.Q - l.Q * r.P <= 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >(Frac l, Frac r) => l.P * r.Q - l.Q * r.P > 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >=(Frac l, Frac r) => l.P * r.Q - l.Q * r.P >= 0;

        public static bool operator ==(Frac l, Frac r) => l.Equals(r);
        public static bool operator !=(Frac l, Frac r) => !l.Equals(r);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Equals(Frac other) => P == other.P && Q == other.Q;
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override bool Equals(object? obj) => obj is Frac f && Equals(f);
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override int GetHashCode() => (int)((P + Q * 998) % 998244353);
        public override string ToString() => $"{P}/{Q}";

        public static long GCD(long x, long y)
        {
            while (x != 0 && y != 0)
                if (x > y)
                    x %= y;
                else
                    y %= x;

            return Math.Max(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public int CompareTo(Frac other)
        {
            var s = this.P * other.Q - this.Q * other.P;
            return Math.Sign(s);
        }
    }
}
