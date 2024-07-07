using ProblemSolving.Templates.Geometry;
using System.Collections.Generic;

namespace ProblemSolving.TestInterfaces.AngleSort
{
    public class AngleComparerAngleSort : AngleSortInterface
    {
        public override List<IntPoint2> Sort(List<IntPoint2> vecs)
        {
            vecs.Sort(AngleComparer.CompareAngleThenByDistance);
            return vecs;
        }
    }
}
