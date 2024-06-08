using NUnit.Framework;
using ProblemSolving.Templates.Splay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProblemSolving.Templates.UnitTests.Tests
{
    public class KeyedSplayTreeTest
    {
        public enum OperationType
        {
            InsertOrUpdate,
            Remove,
            FindMinXInRange,
            FindMaxXInRange,
            RangeSum,
        }

        public sealed class SumSplayTreeNode : KeyedSplayNode<long, SumSplayTreeNode>
        {
            public long Value;
            public long Sum;

            public SumSplayTreeNode(long key, long val) : base(key)
            {
                Value = val;
                Sum = val;
            }

            public override void UpdateTracked()
            {
                base.UpdateTracked();
                Sum = Value + (Left?.Sum ?? 0) + (Right?.Sum ?? 0);
            }

            public override string ToString() => $"{Key}: {Value}, Sum {Sum}";
        }

        public class SumSplayTree : KeyedSplayTree<long, SumSplayTreeNode>
        {
            private Dictionary<long, SumSplayTreeNode> _nodeDict;

            public SumSplayTree()
            {
                _nodeDict = new Dictionary<long, SumSplayTreeNode>();
            }

            public void InsertOrUpdate(long x, long val)
            {
                if (_nodeDict.TryGetValue(x, out var node))
                {
                    Splay(node);
                }
                else
                {
                    Insert(new SumSplayTreeNode(x, val));
                }
            }

            public long RangeSum(long minIncl, long maxExcl)
            {
                return Gather(minIncl, maxExcl)?.Sum ?? 0;
            }
        }

        public class NaiveSolution
        {
            private Dictionary<long, long> _dict;

            public NaiveSolution()
            {
                _dict = new Dictionary<long, long>();
            }

            public void InsertOrUpdate(long x, long val) => _dict[x] = val;
            public void Remove(long x) => _dict.Remove(x);
            public long RangeSum(long stIncl, long edExcl)
            {
                var sum = 0L;
                foreach (var (k, v) in _dict)
                    if (stIncl <= k && k < edExcl)
                        sum += v;

                return sum;
            }

            public bool TryFindMin(long minIncl, out long min)
            {
                min = default;
                var matched = _dict.Keys.Where(k => k >= minIncl).ToArray();

                if (matched.Length == 0)
                    return false;

                min = matched.Min();
                return true;
            }
            public bool TryFindMax(long maxExcl, out long max)
            {
                max = default;
                var matched = _dict.Keys.Where(k => k < maxExcl).ToArray();

                if (matched.Length == 0)
                    return false;

                max = matched.Max();
                return true;
            }
        }

        [Test]
        public void FuzzKeyedSplayTest([Range(1, 100)] int randomSeed)
        {
            var tree = new SumSplayTree();
            var naive = new NaiveSolution();
            var rd = new Random(randomSeed);

            for (var idx = 0; idx < 10000; idx++)
            {
                var type = rd.NextEnum<OperationType>();

                if (type == OperationType.InsertOrUpdate)
                {
                    var x = rd.NextInt64();
                    var val = rd.Next();

                    tree.InsertOrUpdate(x, val);
                    naive.InsertOrUpdate(x, val);
                }
                else if (type == OperationType.RangeSum)
                {
                    var stIncl = rd.NextInt64();
                    var edExcl = rd.NextInt64();

                    if (stIncl > edExcl)
                        (stIncl, edExcl) = (edExcl, stIncl);

                    var l = naive.RangeSum(stIncl, edExcl);
                    var r = tree.RangeSum(stIncl, edExcl);

                    Assert.AreEqual(l, r);
                }
                else if (type == OperationType.FindMinXInRange)
                {
                    var minIncl = rd.NextInt64();

                    var found = naive.TryFindMin(minIncl, out var l);
                    var r = tree.FindMin(minIncl);

                    if (!found)
                    {
                        Assert.IsNull(r);
                    }
                    else
                    {
                        Assert.IsNotNull(r);
                        Assert.AreEqual(l, r.Key);
                    }
                }
                else if (type == OperationType.FindMaxXInRange)
                {
                    var maxExcl = rd.NextInt64();

                    var found = naive.TryFindMax(maxExcl, out var l);
                    var r = tree.FindMax(maxExcl);

                    if (!found)
                    {
                        Assert.IsNull(r);
                    }
                    else
                    {
                        Assert.IsNotNull(r);
                        Assert.AreEqual(l, r.Key);
                    }
                }
            }
        }
    }
}
