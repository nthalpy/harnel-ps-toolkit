namespace ProblemSolving.Templates.PersistenceSegmentTree
{
    [IncludeIfReferenced]
    public abstract class MonoidPersistenceSegTree<TSelf, TElement, TUpdate>
        where TSelf : MonoidPersistenceSegTree<TSelf, TElement, TUpdate>
        where TElement : struct
        where TUpdate : struct
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

        protected Node? _root;
        public long Size { get; protected set; }

        public MonoidPersistenceSegTree(long size)
        {
            Size = size;
        }
        protected MonoidPersistenceSegTree(Node? root, long size)
        {
            _root = root;
            Size = size;
        }

        public TSelf UpdateValue(int idx, TUpdate update)
        {
            var id = Identity();
            var newroot = new Node(0, Size, id);

            var curr = _root;
            var newcurr = newroot;

            while (newcurr.StIncl != idx || newcurr.EdExcl != idx + 1)
            {
                var mid = (newcurr.StIncl + newcurr.EdExcl) / 2;
                if (idx < mid)
                {
                    if (newcurr.Left == null)
                        newcurr.MakeLeftNode(id);

                    newcurr.Right = curr?.Right;
                    curr = curr?.Left;
                    newcurr = newcurr.Left!;
                }
                else
                {
                    if (newcurr.Right == null)
                        newcurr.MakeRightNode(id);

                    newcurr.Left = curr?.Left;
                    curr = curr?.Right;
                    newcurr = newcurr.Right!;
                }
            }

            newcurr.Element = UpdateElement((curr?.Element ?? Identity()), update);
            while (newcurr != null)
            {
                if (newcurr.Left != null && newcurr.Right != null)
                    newcurr.Element = Merge(newcurr.Left.Element, newcurr.Right.Element);
                else if (newcurr.Left != null)
                    newcurr.Element = newcurr.Left.Element;
                else if (newcurr.Right != null)
                    newcurr.Element = newcurr.Right.Element;

                newcurr = newcurr.Parent;
            }

            return CreateNew(newroot, Size);
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

        protected abstract TSelf CreateNew(Node root, long size);
        protected abstract TElement Identity();
        protected abstract TElement UpdateElement(TElement elem, TUpdate update);
        protected abstract TElement Merge(TElement l, TElement r);
    }
}
