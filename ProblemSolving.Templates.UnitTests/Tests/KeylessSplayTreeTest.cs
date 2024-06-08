using NUnit.Framework;
using ProblemSolving.Templates.Splay;
using System;
using System.Collections.Generic;

namespace ProblemSolving.Templates.UnitTests.Tests
{
    public class KeylessSplayTreeTest
    {
        public enum OperationType
        {
            Update,
            Insert,
            Index,
            Remove,
            RangeRemove,
            RangeSum,
        }

        public sealed class SumSplayTreeNode : SplayNode<SumSplayTreeNode>
        {
            public long Value;
            public long Sum;

            public SumSplayTreeNode(long val)
            {
                Value = val;
                Sum = val;
            }

            public override void UpdateTracked()
            {
                base.UpdateTracked();
                Sum = Value + (Left?.Sum ?? 0) + (Right?.Sum ?? 0);
            }

            public override string ToString() => $"{Value}, Sum {Sum}";
        }

        public class SumSplayTree : SplayTree<SumSplayTreeNode>
        {
            public void UpdateKth(int pos, int val)
            {
                var k = FindKth(pos, true)!;
                k.Value = val;
                k.UpdateTracked();
            }

            public long RangeSum(int stIncl, int edExcl)
            {
                return Gather(stIncl, edExcl)?.Sum ?? 0;
            }

            public void RangeRemove(int stIncl, int edExcl)
            {
                var g = Gather(stIncl, edExcl)!;

                if (g.Parent == null)
                {
                    _root = null;
                }
                else
                {
                    var p = g.Parent;

                    if (g.IsLeftChild)
                        p.Left = null;
                    else
                        p.Right = null;

                    Splay(p);
                }
            }
        }

        public class NaiveSolution
        {
            public int NodeCount => _list.Count;
            private List<long> _list;

            public NaiveSolution()
            {
                _list = new List<long>();
            }

            public void InsertKth(int pos, int val) => _list.Insert(pos, val);
            public long FindKth(int pos) => _list[pos];
            public void Update(int pos, int val) => _list[pos] = val;
            public void RemoveKth(int pos) => _list.RemoveAt(pos);
            public void RangeRemove(int stIncl, int edExcl) => _list.RemoveRange(stIncl, edExcl - stIncl);

            public long RangeSum(int stIncl, int edExcl)
            {
                var sum = 0L;
                for (var idx = stIncl; idx < edExcl; idx++)
                    sum += _list[idx];

                return sum;
            }
        }

        [Test]
        public void FuzzKeyedSplayTest([Range(1, 100)] int randomSeed)
        {
            var tree = new SumSplayTree();
            var naive = new NaiveSolution();
            var rd = new Random(randomSeed);

            for (var idx = 0; idx < 30000; idx++)
            {
                var type = rd.NextEnum<OperationType>();
                var count = naive.NodeCount;

                if (type == OperationType.Update)
                {
                    if (count == 0)
                        continue;

                    var pos = rd.Next(0, count);
                    var val = rd.Next();

                    tree.UpdateKth(pos, val);
                    naive.Update(pos, val);
                }
                else if (type == OperationType.Insert)
                {
                    var pos = rd.Next(0, count + 1);
                    var val = rd.Next();

                    tree.InsertKth(pos, new SumSplayTreeNode(val));
                    naive.InsertKth(pos, val);
                }
                else if (type == OperationType.Index)
                {
                    if (count == 0)
                        continue;

                    var pos = rd.Next(0, count);

                    var l = tree.FindKth(pos, true)?.Value;
                    var r = naive.FindKth(pos);

                    Assert.AreEqual(l, r);
                }
                else if (type == OperationType.Remove)
                {
                    if (count == 0)
                        continue;

                    var pos = rd.Next(0, count);

                    tree.RemoveKth(pos);
                    naive.RemoveKth(pos);
                }
                else if (type == OperationType.RangeRemove)
                {
                    if (count == 0)
                        continue;

                    var st = rd.Next(0, count);
                    var ed = rd.Next(0, count);
                    if (st > ed)
                        (st, ed) = (ed, st);

                    tree.RangeRemove(st, ed + 1);
                    naive.RangeRemove(st, ed + 1);
                }
                else if (type == OperationType.RangeSum)
                {
                    if (count == 0)
                        continue;

                    var st = rd.Next(0, count);
                    var ed = rd.Next(0, count);
                    if (st > ed)
                        (st, ed) = (ed, st);

                    var l = tree.RangeSum(st, ed + 1);
                    var r = naive.RangeSum(st, ed + 1);

                    Assert.AreEqual(l, r);
                }

                Assert.AreEqual(naive.NodeCount, tree.NodeCount);
            }
        }
    }
}
