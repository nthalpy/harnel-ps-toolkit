using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public abstract class PointUpdateRangeSumInterface
    {
        public abstract void Initialize(int size);
        public abstract void PointUpdate(int index, long val);
        public abstract long RangeSum(int stIncl, int edExcl);

        private enum OperationType
        {
            PointUpdate,
            RangeSum,
        }

        public List<long> Fuzz(Random rd, int size, int queryCount)
        {
            var results = new List<long>();
            Initialize(size);

            for (var queryIndex = 0; queryIndex < queryCount; queryIndex++)
            {
                var type = rd.NextEnum<OperationType>();
                if (type == OperationType.PointUpdate)
                {
                    var idx = rd.Next(size);
                    var val = rd.NextInt64();

                    PointUpdate(idx, val);
                }
                else if (type == OperationType.RangeSum)
                {
                    var (st, ed) = rd.NextRange(size);
                    results.Add(RangeSum(st, ed));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return results;
        }
    }
}
