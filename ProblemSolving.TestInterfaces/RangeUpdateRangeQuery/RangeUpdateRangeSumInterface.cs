using ProblemSolving.Templates.TestInterfaces;
using System;
using System.Collections.Generic;

namespace ProblemSolving.TestInterfaces.RangeUpdateRangeQuery
{
    public abstract class RangeUpdateRangeSumInterface
    {
        public abstract void Initialize(int size);
        public abstract void RangeUpdate(int stIncl, int edExcl, long val);
        public abstract long RangeSum(int stIncl, int edExcl);

        private enum OperationType
        {
            RangeUpdate,
            RangeSum,
        }

        public List<long> Fuzz(Random rd, int size, int queryCount)
        {
            var results = new List<long>();
            Initialize(size);

            for (var queryIndex = 0; queryIndex < queryCount; queryIndex++)
            {
                var type = rd.NextEnum<OperationType>();
                if (type == OperationType.RangeUpdate)
                {
                    var (st, ed) = rd.NextRange(size);
                    var val = rd.NextInt64() % 100;

                    RangeUpdate(st, ed, val);
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
