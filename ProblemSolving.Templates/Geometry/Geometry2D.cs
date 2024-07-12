using System;
using System.Drawing;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public class Geometry2D
    {
        public static double AbsPolygonArea(DoublePoint2[] points)
        {
            return Math.Abs(PolygonArea(points));
        }

        public static double AbsTriangleArea(DoublePoint2 a, DoublePoint2 b, DoublePoint2 c)
        {
            return Math.Abs(TriangleArea(a, b, c));
        }

        public static double PolygonArea(DoublePoint2[] points)
        {
            var n = points.Length;
            var sum = 0.0;

            for (var idx = 0; idx < n; idx++)
            {
                var jdx = (idx + 1) % n;
                sum += points[idx].X * points[jdx].Y - points[idx].Y * points[jdx].X;
            }

            return sum / 2;
        }

        public static double TriangleArea(DoublePoint2 a, DoublePoint2 b, DoublePoint2 c)
        {
            return (a.X * b.Y - a.Y * b.X
                + b.X * c.Y - b.Y * c.X
                + c.X * a.Y - c.Y * a.X) * 0.5;
        }
    }
}
