using System;

namespace ProblemSolving.Templates.Splay
{
    [IncludeIfReferenced]
    public class KeyedSplayTree<TKey, TNode> : SplayTree<TNode>
        where TKey : IComparable<TKey>
        where TNode : KeyedSplayNode<TKey, TNode>
    {
        protected void Insert(TNode newnode)
        {
            var curr = _root;

            if (curr == null)
            {
                _root = newnode;
                newnode.UpdateTracked();
                return;
            }

            while (true)
            {
                var comp = newnode.Key.CompareTo(curr.Key);

                if (comp == 0)
                    throw new InvalidOperationException($"Key duplication has occured: {newnode.Key}");

                if (comp < 0)
                {
                    if (curr.Left == null)
                    {
                        (curr.Left, newnode.Parent) = (newnode, curr);
                        break;
                    }

                    curr = curr.Left;
                }
                else
                {
                    if (curr.Right == null)
                    {
                        (curr.Right, newnode.Parent) = (newnode, curr);
                        break;
                    }

                    curr = curr.Right;
                }
            }

            Splay(newnode);
        }

        public TNode? FindMin(TKey minIncl)
        {
            if (_root == null)
                return null;

            var curr = _root;
            var minNode = default(TNode?);

            while (true)
            {
                if (minIncl.CompareTo(curr.Key) <= 0
                    && (minNode == null || curr.Key.CompareTo(minNode.Key) < 0))
                {
                    minNode = curr;
                }

                var comp = minIncl.CompareTo(curr.Key);

                if (comp < 0)
                {
                    if (curr.Left == null)
                    {
                        Splay(curr);
                        return minNode;
                    }

                    curr = curr.Left;
                }
                else
                {
                    if (curr.Right == null)
                    {
                        Splay(curr);
                        return minNode;
                    }

                    curr = curr.Right;
                }
            }
        }
        public TNode? FindMax(TKey maxExcl)
        {
            if (_root == null)
                return null;

            var curr = _root;
            var maxNode = default(TNode?);

            while (true)
            {
                if (curr.Key.CompareTo(maxExcl) < 0
                    && (maxNode == null || curr.Key.CompareTo(maxNode.Key) > 0))
                {
                    maxNode = curr;
                }

                var comp = maxExcl.CompareTo(curr.Key);

                if (comp < 0)
                {
                    if (curr.Left == null)
                    {
                        Splay(curr);
                        return maxNode;
                    }

                    curr = curr.Left;
                }
                else
                {
                    if (curr.Right == null)
                    {
                        Splay(curr);
                        return maxNode;
                    }

                    curr = curr.Right;
                }
            }
        }

        /// <summary>
        /// Collect all node in range [minIncl, maxExcl) in one subtree.
        /// Returns null if there are no node falls in specified range.
        /// </summary>
        public TNode? Gather(TKey minIncl, TKey maxExcl)
        {
            var right = FindMin(maxExcl);
            var left = FindMax(minIncl);

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
