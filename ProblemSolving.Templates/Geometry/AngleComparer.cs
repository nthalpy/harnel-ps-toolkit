using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public class AngleComparer : IComparer<IntPoint2>
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

        public int Compare(IntPoint2 u, IntPoint2 v)
        {
            // y
            // ^ 4 3 2
            // | 5 0 1
            // | 6 7 8
            // +-----> x

            var uidx = Index(u);
            var vidx = Index(v);

            if (uidx != vidx)
                return uidx.CompareTo(vidx);

            var ccw = u.Y * v.X - u.X * v.Y;
            if (ccw == 0 && _tiebreakByDistance)
            {
                var uxy = Math.Abs(u.X) + Math.Abs(u.Y);
                var vxy = Math.Abs(v.X) + Math.Abs(v.Y);

                return uxy.CompareTo(vxy);
            }
            else
            {
                return ccw.CompareTo(0);
            }
        }

        private int Index(IntPoint2 v)
        {
            var sx = Math.Sign(v.X) + 1;
            var sy = Math.Sign(v.Y) + 1;
            return _signToIndex[sy * 3 + sx];
        }
    }
}
