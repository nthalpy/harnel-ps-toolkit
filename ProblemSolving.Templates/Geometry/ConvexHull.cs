using System.Collections.Generic;
using System.Linq;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public class ConvexHull
    {
        private static long CCW(IntPoint2 l, IntPoint2 m, IntPoint2 r)
        {
            var rdy = r.Y - m.Y;
            var rdx = r.X - m.X;
            var ldy = m.Y - l.Y;
            var ldx = m.X - l.X;

            return rdy * ldx - rdx * ldy;
        }

        public static IntPoint2[] MonotoneChain(List<IntPoint2> points, bool allowLinear)
        {
            var sorted = points
                .OrderBy(v => v.X)
                .ThenBy(v => v.Y)
                .ToArray();

            var buffer = new int[1 + points.Count];
            var inset = new bool[points.Count];
            var k = 0;

            for (var idx = 0; idx < sorted.Length; idx++)
            {
                buffer[k++] = idx;
                inset[idx] = true;

                while (k >= 3)
                {
                    var ccw = CCW(sorted[buffer[k - 3]], sorted[buffer[k - 2]], sorted[buffer[k - 1]]);

                    if (ccw < 0)
                    {
                        inset[buffer[k - 2]] = false;
                        buffer[k - 2] = idx;
                        k--;
                    }
                    else if (!allowLinear && ccw == 0)
                    {
                        buffer[k - 2] = idx;
                        k--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (var idx = sorted.Length - 1; idx >= 0; idx--)
            {
                if (idx != 0 && inset[idx])
                    continue;

                buffer[k++] = idx;
                inset[idx] = true;

                while (k >= 3)
                {
                    var ccw = CCW(sorted[buffer[k - 3]], sorted[buffer[k - 2]], sorted[buffer[k - 1]]);

                    if (ccw < 0)
                    {
                        inset[buffer[k - 2]] = false;
                        buffer[k - 2] = idx;
                        k--;
                    }
                    else if (!allowLinear && ccw == 0)
                    {
                        buffer[k - 2] = idx;
                        k--;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return buffer.Take(k - 1).Select(idx => sorted[idx]).ToArray();
        }
    }
}
