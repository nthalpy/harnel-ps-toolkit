using ProblemSolving.Templates.Geometry;
using System;
using System.Collections.Generic;

namespace ProblemSolving.TestInterfaces.AngleSort
{
    public abstract class AngleSortInterface
    {
        public abstract List<IntPoint2> Sort(List<IntPoint2> vecs);

        public List<IntPoint2> Fuzz(Random rd, int size)
        {
            var min = -10000;
            var max = 10000;

            var list = new List<IntPoint2>();
            while (size-- > 0)
            {
                var v = new IntPoint2(rd.Next(min, max + 1), rd.Next(min, max + 1));
                list.Add(v);
            }

            return Sort(list);
        }
    }
}
