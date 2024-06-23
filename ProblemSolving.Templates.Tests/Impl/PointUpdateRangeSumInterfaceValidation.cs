using NUnit.Framework;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProblemSolving.Templates.Tests.Impl
{
    public partial class PointUpdateRangeSumInterfaceValidation : Validation<PointUpdateRangeSumInterface, List<long>>
    {
        public override List<long> Fuzz(PointUpdateRangeSumInterface f, int randomSeed)
        {
            var rd = new Random(randomSeed);
            return f.Fuzz(rd, 10000, 10000);
        }

        public override void Validate(List<long> slow, List<long> fast)
        {
            Assert.IsTrue(slow.SequenceEqual(fast));
        }

        [Test]
        public void SumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
            => Validate(
                Fuzz(new NaivePointUpdateRangeSum(), randomSeed),
                Fuzz(new SumSegPointUpdateRangeSum(), randomSeed));

        [Test]
        public void OldGroupGenericSumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
            => Validate(
                Fuzz(new NaivePointUpdateRangeSum(), randomSeed),
                Fuzz(new OldGroupGenericSumSegPointUpdateRangeSum(), randomSeed));

        [Test]
        public void GroupGenericSumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
            => Validate(
                Fuzz(new NaivePointUpdateRangeSum(), randomSeed),
                Fuzz(new GroupGenericSumSegPointUpdateRangeSum(), randomSeed));

        [Test]
        public void OldGenericSumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
            => Validate(
                Fuzz(new NaivePointUpdateRangeSum(), randomSeed),
                Fuzz(new OldGenericSumSegPointUpdateRangeSum(), randomSeed));

        [Test]
        public void GenericSumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
            => Validate(
                Fuzz(new NaivePointUpdateRangeSum(), randomSeed),
                Fuzz(new GenericSumSegPointUpdateRangeSum(), randomSeed));
    }
}
