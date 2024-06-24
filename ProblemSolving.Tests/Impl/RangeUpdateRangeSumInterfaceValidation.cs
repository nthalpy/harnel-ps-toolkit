using NUnit.Framework;
using ProblemSolving.TestInterfaces.RangeUpdateRangeQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProblemSolving.Templates.Tests.Impl
{
    public partial class RangeUpdateRangeSumInterfaceValidation : Validation<RangeUpdateRangeSumInterface, List<long>>
    {
        public override List<long> Fuzz(RangeUpdateRangeSumInterface f, int randomSeed)
        {
            var rd = new Random(randomSeed);
            return f.Fuzz(rd, 10000, 10000);
        }

        public override void Validate(List<long> slow, List<long> fast)
        {
            Assert.IsTrue(slow.SequenceEqual(fast));
        }

        [Test]
        public void LazySegRangeUpdateRangeSum([Range(1, 100)] int randomSeed)
            => Validate(
                Fuzz(new NaiveRangeUpdateRangeSum(), randomSeed),
                Fuzz(new LazySegRangeUpdateRangeSum(), randomSeed));
    }
}
