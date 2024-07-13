namespace ProblemSolving.Templates.Geometry
{
    [IncludeIfReferenced]
    public record struct DoubleVector2(double X, double Y)
    {
        public double MagnitudeSquare => X * X + Y * Y;

        public static double Dot(DoubleVector2 l, DoubleVector2 r) => l.X * r.X + l.Y * r.Y;

        public static DoubleVector2 operator -(DoubleVector2 l) => new DoubleVector2(-l.X, -l.Y);

        public static DoubleVector2 operator +(DoubleVector2 l, DoubleVector2 r) => new DoubleVector2(l.X + r.X, l.Y + r.Y);
        public static DoubleVector2 operator -(DoubleVector2 l, DoubleVector2 r) => new DoubleVector2(l.X - r.X, l.Y - r.Y);
        public static DoubleVector2 operator *(DoubleVector2 v, double s) => new DoubleVector2(v.X * s, v.Y * s);
        public static DoubleVector2 operator *(double s, DoubleVector2 v) => new DoubleVector2(v.X * s, v.Y * s);
        public static DoubleVector2 operator /(DoubleVector2 v, double s) => new DoubleVector2(v.X / s, v.Y / s);
    }
}
