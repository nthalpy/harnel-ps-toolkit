using System.Numerics;

namespace ProblemSolving.Templates.SegmentTree
{
    [IncludeIfReferenced]
    public abstract class DynamicAbelianGroupSegTree<TElement, TUpdate, TDiff>
        where TElement : struct
        where TUpdate : struct
        where TDiff : struct
    {
        protected class Node
        {
            public long StIncl;
            public long EdExcl;
            public Node? Parent;
            public Node? Left;
            public Node? Right;

            public TElement Element;

            public Node(long stIncl, long edExcl, TElement id)
            {
                StIncl = stIncl;
                EdExcl = edExcl;
                Element = id;
            }

            public void MakeLeftNode(TElement id)
            {
                var mid = (StIncl + EdExcl) / 2;
                Left = new Node(StIncl, mid, id);
                Left.Parent = this;
            }
            public void MakeRightNode(TElement id)
            {
                var mid = (StIncl + EdExcl) / 2;
                Right = new Node(mid, EdExcl, id);
                Right.Parent = this;
            }

            public override string ToString() => $"Range [{StIncl}, {EdExcl}): {Element}";
        }

        protected Node _root;
        public long Size { get; protected set; }

        public DynamicAbelianGroupSegTree(long size)
        {
            _root = new Node(0, size, Identity());
            Size = size;
        }

        protected Node FindOrCreateNode(int idx)
        {
            var curr = _root;

            while (curr.StIncl != idx || curr.EdExcl != idx + 1)
            {
                var mid = (curr.StIncl + curr.EdExcl) / 2;
                if (idx < mid)
                {
                    if (curr.Left == null)
                        curr.MakeLeftNode(Identity());

                    curr = curr.Left!;
                }
                else
                {
                    if (curr.Right == null)
                        curr.MakeRightNode(Identity());

                    curr = curr.Right!;
                }
            }

            return curr;
        }

        public void UpdateValue(int idx, TUpdate val)
        {
            var curr = FindOrCreateNode(idx);

            var diff = CreateDiff(curr.Element, val);
            while (curr != null)
            {
                curr.Element = ApplyDiff(curr.Element, diff);
                curr = curr.Parent;
            }
        }
        public void UpdateDiff(int idx, TDiff diff)
        {
            var curr = FindOrCreateNode(idx);

            while (curr != null)
            {
                curr.Element = ApplyDiff(curr.Element, diff);
                curr = curr.Parent;
            }
        }

        public TElement Range(int stIncl, int edExcl)
        {
            return Range(_root, stIncl, edExcl);
        }
        protected TElement Range(Node? node, int stIncl, int edExcl)
        {
            if (node == null || node.EdExcl <= stIncl || edExcl <= node.StIncl)
                return Identity();

            if (stIncl <= node.StIncl && node.EdExcl <= edExcl)
                return node.Element;

            return Merge(Range(node.Left, stIncl, edExcl), Range(node.Right, stIncl, edExcl));
        }

        protected abstract TElement Identity();
        protected abstract TDiff CreateDiff(TElement element, TUpdate val);
        protected abstract TElement ApplyDiff(TElement element, TDiff diff);
        protected abstract TElement Merge(TElement l, TElement r);
    }
}
