using System;
using System.Diagnostics.CodeAnalysis;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public struct ModInt
    {
        public struct Mod998244353 : IModIntInterface
        {
            public long GetMod() => 998244353;
        }
        public struct Mod1000000007 : IModIntInterface
        {
            public long GetMod() => 1000000007;
        }
    }

    /// <summary>
    /// long based mod int
    /// </summary>
    [IncludeIfReferenced]
    public struct ModInt<TOp> : IEquatable<ModInt<TOp>>
        where TOp : struct, IModIntInterface
    {
        public long V;

        public ModInt()
            : this(0)
        {
        }
        public ModInt(long v)
        {
            var mod = default(TOp).GetMod();

            if (v < 0)
                v = (-v + mod - 1) / mod * mod;
            else
                v = v % mod;

            V = v;
        }

        public static implicit operator ModInt<TOp>(long val) => new ModInt<TOp>(val);
        public static ModInt<TOp> operator +(ModInt<TOp> l, ModInt<TOp> r)
        {
            var mod = default(TOp).GetMod();
            var newv = l.V + r.V;

            if (newv > mod)
                newv -= mod;

            return new ModInt<TOp>(newv);
        }
        public static ModInt<TOp> operator -(ModInt<TOp> l, ModInt<TOp> r)
        {
            var mod = default(TOp).GetMod();
            var newv = l.V - r.V;

            if (newv < 0)
                newv += mod;

            return new ModInt<TOp>(newv);
        }
        public static ModInt<TOp> operator *(ModInt<TOp> l, ModInt<TOp> r)
        {
            var mod = default(TOp).GetMod();
            var newv = l.V * r.V;

            if (newv > mod)
                newv %= mod;

            return new ModInt<TOp>(newv);
        }
        public static ModInt<TOp> operator /(ModInt<TOp> l, ModInt<TOp> r)
        {
            var rinv = r.ModInv();
            return l * rinv;
        }

        public ModInt<TOp> ModInv()
        {
            var mod = default(TOp).GetMod();
            return new ModInt<TOp>(NumberTheory.FastPow(V, mod - 2, mod));
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is ModInt<TOp> other && this.Equals(other);
        }
        public bool Equals(ModInt<TOp> other)
        {
            return this.V == other.V;
        }

        public override int GetHashCode() => V.GetHashCode();
        public override string ToString() => V.ToString();
    }
}
