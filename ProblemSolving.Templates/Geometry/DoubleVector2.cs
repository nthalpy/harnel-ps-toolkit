namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public record struct DoubleVector2(double DeltaX, double DeltaY)
    {
        public static DoubleVector2 operator *(DoubleVector2 v, double s) => new DoubleVector2(v.DeltaX * s, v.DeltaY * s);
        public static DoubleVector2 operator *(double s, DoubleVector2 v) => new DoubleVector2(v.DeltaX * s, v.DeltaY * s);
        public static DoubleVector2 operator /(DoubleVector2 v, double s) => new DoubleVector2(v.DeltaX / s, v.DeltaY / s);
    }
}
