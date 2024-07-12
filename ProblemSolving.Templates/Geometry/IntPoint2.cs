using ProblemSolving.Templates.Utility;
using System;
using System.Linq;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public record struct IntPoint2(long X, long Y)
    {
        public static IntPoint2 Parse(string s)
        {
            var (x, y) = s.Split(' ').Select(Int64.Parse).ToArray();
            return new IntPoint2(x, y);
        }

        public static IntVector2 operator -(IntPoint2 to, IntPoint2 from) => new IntVector2(to.X - from.X, to.Y - from.Y);
        public static IntPoint2 operator +(IntPoint2 p, IntVector2 v) => new IntPoint2(p.X + v.X, p.Y + v.Y);
    }
}
