using ProblemSolving.Templates.Utility;
using System;
using System.Linq;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public record struct IntVector2(long X, long Y)
    {
        public static IntVector2 Parse(string s)
        {
            var (x, y) = s.Split(' ').Select(Int64.Parse).ToArray();
            return new IntVector2(x, y);
        }

        public long MagnitudeSquare => X * X + Y * Y;

        public static long Dot(IntVector2 l, IntVector2 r) => l.X * r.X + l.Y * r.Y;

        public static IntVector2 operator -(IntVector2 l) => new IntVector2(-l.X, -l.Y);

        public static IntVector2 operator +(IntVector2 l, IntVector2 r) => new IntVector2(l.X + r.X, l.Y + r.Y);
        public static IntVector2 operator -(IntVector2 l, IntVector2 r) => new IntVector2(l.X - r.X, l.Y - r.Y);
        public static IntVector2 operator *(IntVector2 v, long s) => new IntVector2(v.X * s, v.Y * s);
        public static IntVector2 operator *(long s, IntVector2 v) => new IntVector2(v.X * s, v.Y * s);
        public static IntVector2 operator /(IntVector2 v, long s) => new IntVector2(v.X / s, v.Y / s);
    }
}
