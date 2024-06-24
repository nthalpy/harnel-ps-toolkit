namespace ProblemSolving.TestInterfaces.RangeUpdateRangeQuery
{
    public class NaiveRangeUpdateRangeSum : RangeUpdateRangeSumInterface
    {
        private long[] _arr = default!;

        public override void Initialize(int size)
        {
            _arr = new long[size];
        }

        public override long RangeSum(int stIncl, int edExcl)
        {
            var sum = 0L;
            for (var idx = stIncl; idx < edExcl; idx++)
                sum += _arr[idx];

            return sum;
        }

        public override void RangeUpdate(int stIncl, int edExcl, long val)
        {
            for (var idx = stIncl; idx < edExcl; idx++)
                _arr[idx] += val;
        }
    }
}
