using System;

namespace ProblemSolving.Templates.TestInterfaces
{
    public static class RandomExtension
    {
        private static class EnumValueHolder<TEnum>
            where TEnum : struct, Enum
        {
            public static TEnum[] Values { get; } = Enum.GetValues<TEnum>();
        }

        public static TEnum NextEnum<TEnum>(this Random rd)
            where TEnum : struct, Enum
        {
            var vals = EnumValueHolder<TEnum>.Values;
            return vals[rd.Next(0, vals.Length)];
        }

        public static (int stIncl, int edExcl) NextRange(this Random rd, int maxEdExcl)
        {
            var stIncl = rd.Next(0, maxEdExcl);
            var edExcl = 1 + rd.Next(stIncl, maxEdExcl);

            return (stIncl, edExcl);
        }
    }
}
