namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public record struct IntVector3(long DeltaX, long DeltaY, long DeltaZ)
    {
        public long MagnitudeSquare => DeltaX * DeltaX + DeltaY * DeltaY + DeltaZ * DeltaZ;

        public static IntVector3 operator *(IntVector3 v, long s) => new IntVector3(v.DeltaX * s, v.DeltaY * s, v.DeltaZ * s);
        public static IntVector3 operator *(long s, IntVector3 v) => new IntVector3(v.DeltaX * s, v.DeltaY * s, v.DeltaZ * s);
        public static IntVector3 operator /(IntVector3 v, long s) => new IntVector3(v.DeltaX / s, v.DeltaY / s, v.DeltaZ / s);
    }
}
