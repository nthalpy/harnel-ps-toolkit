using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public class AngleComparer : IComparer<IntPoint2>, IComparer<IntVector2>
    {
        public readonly static AngleComparer CompareAngle = new AngleComparer(false);
        public readonly static AngleComparer CompareAngleThenByDistance = new AngleComparer(true);

        private static int[] _signToIndex = new int[]
        {
            6, 7, 8,
            5, 0, 1,
            4, 3, 2,
        };

        private bool _tiebreakByDistance;

        public AngleComparer(bool tiebreakWithDistance)
        {
            _tiebreakByDistance = tiebreakWithDistance;
        }

        public int Compare(IntPoint2 u, IntPoint2 v) => Compare(u.X, u.Y, v.X, v.Y);
        public int Compare(IntVector2 u, IntVector2 v) => Compare(u.X, u.Y, v.X, v.Y);

        public int Compare(long ux, long uy, long vx, long vy)
        {
            // y
            // ^ 4 3 2
            // | 5 0 1
            // | 6 7 8
            // +-----> x

            var uidx = Index(ux, uy);
            var vidx = Index(vx, vy);

            if (uidx != vidx)
                return uidx.CompareTo(vidx);

            var ccw = uy * vx - ux * vy;
            if (ccw == 0 && _tiebreakByDistance)
            {
                var uxy = Math.Abs(ux) + Math.Abs(uy);
                var vxy = Math.Abs(vx) + Math.Abs(vy);

                return uxy.CompareTo(vxy);
            }
            else
            {
                return ccw.CompareTo(0);
            }
        }

        private int Index(long x, long y)
        {
            var sx = Math.Sign(x) + 1;
            var sy = Math.Sign(y) + 1;
            return _signToIndex[sy * 3 + sx];
        }
    }
}
