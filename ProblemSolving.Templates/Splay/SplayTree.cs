namespace ProblemSolving.Templates.Splay
{
    [IncludeIfReferenced]
    public class SplayTree<TNode> where TNode : SplayNode<TNode>
    {
        protected TNode? _root;

        public int NodeCount => _root?.SubtreeSize ?? 0;

        public SplayTree()
        {
        }

        protected void Rotate(TNode x)
        {
            var p = x.Parent;
            if (p == null)
                return;

            TNode? b;
            if (x.IsLeftChild)
            {
                b = x.Right;
                (p.Left, x.Right) = (x.Right, p);
            }
            else
            {
                b = x.Left;
                (p.Right, x.Left) = (x.Left, p);
            }

            (x.Parent, p.Parent) = (p.Parent, x);

            if (b != null)
                b.Parent = p;

            if (x.Parent != null)
            {
                if (p == x.Parent.Left)
                    x.Parent.Left = x;
                else
                    x.Parent.Right = x;
            }
            else
            {
                _root = x;
            }

            p.UpdateTracked();
            x.UpdateTracked();
        }

        /// <summary>
        /// Attach x to target, when target is ancestor of x.
        /// </summary>
        protected void Splay(TNode x, TNode? target = null)
        {
            x.UpdateTracked();

            while (x.Parent != target)
            {
                var p = x.Parent;
                var g = p!.Parent;

                if (g == target)
                {
                    Rotate(x);
                    return;
                }

                if (g != null)
                    Rotate((x.IsLeftChild == p.IsLeftChild) ? p : x);

                Rotate(x);
            }

            if (target == null)
                _root = x;
        }

        /// <summary>
        /// index is 0-based.
        /// </summary>
        public void InsertKth(int index, TNode node)
        {
            if (_root == null)
            {
                _root = node;
                node.UpdateTracked();
                return;
            }

            if (index == 0)
            {
                var curr = _root;
                while (curr!.Left != null)
                    curr = curr.Left;

                (curr.Left, node.Parent) = (node, curr);
            }
            else
            {
                var k = FindKth(index - 1, false)!;

                if (k.Right == null)
                {
                    (k.Right, node.Parent) = (node, k);
                }
                else
                {
                    k = k.Right;
                    while (k.Left != null)
                        k = k.Left;

                    (k.Left, node.Parent) = (node, k);
                }
            }

            Splay(node);
        }

        /// <summary>
        /// index is 0-based.
        /// </summary>
        public TNode? FindKth(int index, bool splay)
        {
            index++;

            if (index == 0 || _root == null || _root.SubtreeSize < index)
                return null;

            var curr = _root;
            while (true)
            {
                var lsize = curr!.Left?.SubtreeSize ?? 0;

                if (index <= lsize)
                {
                    curr = curr.Left;
                }
                else if (index == lsize + 1)
                {
                    if (splay)
                        Splay(curr);

                    return curr;
                }
                else
                {
                    index -= lsize + 1;
                    curr = curr.Right;
                }
            }
        }

        /// <summary>
        /// index is 0-based.
        /// </summary>
        public void RemoveKth(int index)
        {
            var k = FindKth(index, true)!;

            if (k.Left != null && k.Right != null)
            {
                var l = k.Left;
                var r = k.Right;

                _root = l;
                _root.Parent = null;
                _root.UpdateTracked();

                while (l.Right != null)
                    l = l.Right;

                (l.Right, r.Parent) = (r, l);
                r.UpdateTracked();
                Splay(r);
            }
            else if (k.Left != null)
            {
                _root = k.Left;
                _root.Parent = null;
                _root.UpdateTracked();
            }
            else if (k.Right != null)
            {
                _root = k.Right;
                _root.Parent = null;
                _root.UpdateTracked();
            }
            else
            {
                _root = null;
            }
        }

        public TNode? Gather(int stIncl, int edExcl)
        {
            var right = FindKth(edExcl, true);
            var left = FindKth(stIncl - 1, true);

            if (right == null)
            {
                if (left == null)
                {
                    return _root;
                }
                else
                {
                    Splay(left);
                    return left.Right;
                }
            }
            else
            {
                if (left != null)
                    Splay(left);

                Splay(right, left);
                return right.Left;
            }
        }
    }
}
