using ProblemSolving.Templates.Utility;
using System;
using System.Linq;

namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public record struct IntPoint3(long X, long Y, long Z)
    {
        public static IntPoint3 Parse(string s)
        {
            var (x, y, z) = s.Split(' ').Select(Int64.Parse).ToArray();
            return new IntPoint3(x, y, z);
        }

        public static IntVector3 operator -(IntPoint3 to, IntPoint3 from) => new IntVector3(to.X - from.X, to.Y - from.Y, to.Z - from.Z);
        public static IntPoint3 operator +(IntPoint3 p, IntVector3 v) => new IntPoint3(p.X + v.DeltaX, p.Y + v.DeltaY, p.Z + v.DeltaZ);
    }
}
