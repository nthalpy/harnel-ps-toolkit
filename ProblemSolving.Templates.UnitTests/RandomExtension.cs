using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolving.Templates.UnitTests
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
    }
}
