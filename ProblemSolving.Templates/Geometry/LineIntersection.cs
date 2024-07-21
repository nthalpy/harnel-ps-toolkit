using System;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public static class LineIntersection
    {
        public static LineIntersectionResult Intersect((IntPoint2 p1, IntPoint2 p2) l1, (IntPoint2 p1, IntPoint2 p2) l2)
        {
            var c1 = CCW(l1, l2.p1);
            var c2 = CCW(l1, l2.p2);
            var c3 = CCW(l2, l1.p1);
            var c4 = CCW(l2, l1.p2);

            // In same side
            if (c1 * c2 > 0 || c3 * c4 > 0)
                return LineIntersectionResult.NotIntersect;

            // Intersects in one point
            if (c1 * c2 < 0 && c3 * c4 < 0)
                return LineIntersectionResult.IntersectMidpoint;

            var v1 = IsInsideLine(l1, l2.p1);
            var v2 = IsInsideLine(l1, l2.p2);
            var v3 = IsInsideLine(l2, l1.p1);
            var v4 = IsInsideLine(l2, l1.p2);

            // All 4 points are colinear
            if (c1 == 0 && c2 == 0 && c3 == 0 && c4 == 0)
            {
                if (v1 || v2 || v3 || v4)
                {
                    var xlen = Overlap(l1.p1.X, l1.p2.X, l2.p1.X, l2.p2.X);
                    var ylen = Overlap(l1.p1.Y, l1.p2.Y, l2.p1.Y, l2.p2.Y);

                    if (xlen == 0 && ylen == 0)
                        return LineIntersectionResult.IntersectEndpoint;

                    return LineIntersectionResult.Overlap;
                }

                return LineIntersectionResult.NotIntersect;
            }
            // T-shape
            else
            {
                if (v1 || v2 || v3 || v4)
                    return LineIntersectionResult.IntersectEndpoint;
                else
                    return LineIntersectionResult.NotIntersect;
            }

            throw new ArgumentException();
        }

        private static long Overlap(long x1, long x2, long x3, long x4)
        {
            var xst = Math.Max(Math.Min(x1, x2), Math.Min(x3, x4));
            var xed = Math.Min(Math.Max(x1, x2), Math.Max(x3, x4));

            return xed - xst;
        }

        private static bool IsInsideLine((IntPoint2 p1, IntPoint2 p2) l1, IntPoint2 p)
        {
            var (p1, p2) = l1;

            if (CCW(l1, p) != 0)
                return false;

            if (Math.Min(p1.X, p2.X) <= p.X && p.X <= Math.Max(p1.X, p2.X)
                && Math.Min(p1.Y, p2.Y) <= p.Y && p.Y <= Math.Max(p1.Y, p2.Y))
                return true;

            return false;
        }
        private static long CCW((IntPoint2 p1, IntPoint2 p2) l, IntPoint2 p)
        {
            return CCW(l.p1, l.p2, p);
        }
        private static long CCW(IntPoint2 p, IntPoint2 a, IntPoint2 b)
        {
            var va = a - p;
            var vb = b - p;

            return Math.Sign(va.Y * vb.X - va.X * vb.Y);
        }
    }
}
