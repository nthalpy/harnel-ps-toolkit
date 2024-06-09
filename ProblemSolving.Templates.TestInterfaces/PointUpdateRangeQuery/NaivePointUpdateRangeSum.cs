namespace ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery
{
    public class NaivePointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        private long[] _arr = default!;

        public override void Initialize(int size)
        {
            _arr = new long[size];
        }

        public override void PointUpdate(int index, long val) => _arr[index] = val;
        public override long RangeSum(int stIncl, int edExcl)
        {
            var sum = 0L;
            for (var idx = stIncl; idx < edExcl; idx++)
                sum += _arr[idx];

            return sum;
        }
    }
}
