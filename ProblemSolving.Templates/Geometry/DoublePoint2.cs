using ProblemSolving.Templates.Utility;
using System;
using System.Linq;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public record struct DoublePoint2(double X, double Y)
    {
        public static DoublePoint2 Parse(string s)
        {
            var (x, y) = s.Split(' ').Select(Double.Parse).ToArray();
            return new DoublePoint2(x, y);
        }

        public static DoubleVector2 operator -(DoublePoint2 to, DoublePoint2 from) => new DoubleVector2(to.X - from.X, to.Y - from.Y);
        public static DoublePoint2 operator +(DoublePoint2 p, DoubleVector2 v) => new DoublePoint2(p.X + v.DeltaX, p.Y + v.DeltaY);
    }
}
