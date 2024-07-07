namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public record struct IntVector2(long DeltaX, long DeltaY)
    {
        public static IntVector2 operator *(IntVector2 v, long s) => new IntVector2(v.DeltaX * s, v.DeltaY * s);
        public static IntVector2 operator *(long s, IntVector2 v) => new IntVector2(v.DeltaX * s, v.DeltaY * s);
        public static IntVector2 operator /(IntVector2 v, long s) => new IntVector2(v.DeltaX / s, v.DeltaY / s);
    }
}
