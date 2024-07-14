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
            public Node? Parent;
            public Node? Left;
            public Node? Right;
            public TElement Element;

            public Node(TElement id)
            {
                Element = id;
            }

            public void MakeLeftNode(TElement id)
            {
                Left = new Node(id);
                Left.Parent = this;
            }
            public void MakeRightNode(TElement id)
            {
                Right = new Node(id);
                Right.Parent = this;
            }
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
            var newroot = new Node(id);

            var curr = _root;
            var newcurrStIncl = 0L;
            var newcurrEdExcl = Size;
            var newcurr = newroot;

            while (newcurrEdExcl - newcurrStIncl > 1)
            {
                var mid = (newcurrStIncl + newcurrEdExcl) >> 1;
                if (idx < mid)
                {
                    if (newcurr.Left == null)
                        newcurr.MakeLeftNode(id);

                    newcurrEdExcl = mid;
                    newcurr.Right = curr?.Right;
                    curr = curr?.Left;
                    newcurr = newcurr.Left!;
                }
                else
                {
                    if (newcurr.Right == null)
                        newcurr.MakeRightNode(id);

                    newcurrStIncl = mid;
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

        public TElement Range(long stIncl, long edExcl)
        {
            return Range(_root, 0, Size, stIncl, edExcl);
        }
        protected TElement Range(
            Node? node,
            long nodeStIncl, long nodeEdExcl,
            long stIncl, long edExcl)
        {
            if (node == null || nodeEdExcl <= stIncl || edExcl <= nodeStIncl)
                return Identity();

            if (stIncl <= nodeStIncl && nodeEdExcl <= edExcl)
                return node.Element;

            var nodeMid = (nodeStIncl + nodeEdExcl) >> 1;

            return Merge(
                Range(node.Left, nodeStIncl, nodeMid, stIncl, edExcl),
                Range(node.Right, nodeMid, nodeEdExcl, stIncl, edExcl));
        }

        protected abstract TSelf CreateNew(Node root, long size);
        protected abstract TElement Identity();
        protected abstract TElement UpdateElement(TElement elem, TUpdate update);
        protected abstract TElement Merge(TElement l, TElement r);
    }
}
