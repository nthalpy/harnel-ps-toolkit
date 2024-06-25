using AtCoder;
using ProblemSolving.Templates.TestInterfaces.PointUpdateRangeQuery;

namespace ProblemSolving.TestInterfaces.PointUpdateRangeQuery.Fast
{
    public class ACLSumSegPointUpdateRangeSum : PointUpdateRangeSumInterface
    {
        public struct SumSegOp : ISegtreeOperator<long>
        {
            public long Identity => 0;
            public long Operate(long x, long y) => x + y;
        }

        private Segtree<long, SumSegOp> _seg = default!;

        public override void Initialize(int size)
        {
            _seg = new Segtree<long, SumSegOp>(size);
        }

        public override void PointUpdate(int index, long val) => _seg[index] = val;
        public override long RangeSum(int stIncl, int edExcl) => _seg.Prod(stIncl, edExcl);
    }
}
