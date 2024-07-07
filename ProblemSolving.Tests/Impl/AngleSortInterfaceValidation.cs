using NUnit.Framework;
using ProblemSolving.Templates.Geometry;
using ProblemSolving.TestInterfaces.AngleSort;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProblemSolving.Tests.Impl
{
    public class AngleSortInterfaceValidation : Validation<AngleSortInterface, List<IntPoint2>>
    {
        public override List<IntPoint2> Fuzz(AngleSortInterface f, int randomSeed)
        {
            var rd = new Random(randomSeed);
            return f.Fuzz(rd, 100000);
        }

        public override void Validate(List<IntPoint2> slow, List<IntPoint2> fast)
        {
            Assert.IsTrue(slow.SequenceEqual(fast));
        }

        [Test]
        public void NonGenericSumSegPointUpdateRangeSum([Range(1, 100)] int randomSeed)
            => Validate(
                Fuzz(new NaiveAngleSort(), randomSeed),
                Fuzz(new AngleComparerAngleSort(), randomSeed));
    }
}
