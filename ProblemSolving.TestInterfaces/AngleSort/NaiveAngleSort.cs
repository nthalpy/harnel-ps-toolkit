using ProblemSolving.Templates.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProblemSolving.TestInterfaces.AngleSort
{
    public class NaiveAngleSort : AngleSortInterface
    {
        public override List<IntPoint2> Sort(List<IntPoint2> vecs)
        {
            return vecs
                .OrderBy(v =>
                {
                    var atan = Math.Atan2(v.Y, v.X);
                    if (atan < 0)
                        return 2 * Math.PI + atan;
                    else
                        return atan;
                })
                .ThenBy(v => v.Y * v.Y + v.X * v.X)
                .ToList();
        }
    }
}
