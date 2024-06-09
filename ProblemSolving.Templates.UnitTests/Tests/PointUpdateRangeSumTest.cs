using NUnit.Framework;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;
using System;
using System.Linq;

namespace ProblemSolving.Templates.UnitTests.Tests
{
    public class PointUpdateRangeSumTest
    {
        [Test]
        public void SumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
        {
            var rd1 = new Random(randomSeed);
            var rd2 = new Random(randomSeed);

            var naive = new NaivePointUpdateRangeSum();
            var l = naive.Fuzz(rd1, 10000, 10000);

            var sumseg = new SumSegPointUpdateRangeSum();
            var r = sumseg.Fuzz(rd2, 10000, 10000);

            Assert.IsTrue(l.SequenceEqual(r));
        }

        [Test]
        public void GroupGenericSumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
        {
            var rd1 = new Random(randomSeed);
            var rd2 = new Random(randomSeed);

            var naive = new NaivePointUpdateRangeSum();
            var l = naive.Fuzz(rd1, 10000, 10000);

            var sumseg = new GroupGenericSumSegPointUpdateRangeSum();
            var r = sumseg.Fuzz(rd2, 10000, 10000);

            Assert.IsTrue(l.SequenceEqual(r));
        }

        [Test]
        public void GenericSumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
        {
            var rd1 = new Random(randomSeed);
            var rd2 = new Random(randomSeed);

            var naive = new NaivePointUpdateRangeSum();
            var l = naive.Fuzz(rd1, 10000, 10000);

            var sumseg = new GenericSumSegPointUpdateRangeSum();
            var r = sumseg.Fuzz(rd2, 10000, 10000);

            Assert.IsTrue(l.SequenceEqual(r));
        }
    }
}
