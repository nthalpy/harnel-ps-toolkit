using System;

namespace ProblemSolving.Templates.Treap
{
    /// <summary>
    /// Credits to https://cp-algorithms.com/data_structures/treap.html
    /// </summary>
    [IncludeIfReferenced]
    public class Treap<TElement>
        where TElement : struct, IComparable<TElement>
    {
        public class TreapNode
        {
            public int Priority;
            public TElement Key;
            public TreapNode? Left;
            public TreapNode? Right;

            public TreapNode(int priority, TElement elem)
            {
                Priority = priority;
                Key = elem;
            }

            public override string ToString() => $"{Key} [Priority {Priority}]";
        }

        private Random _rd;
        private TreapNode? _root;

        public Treap()
        {
            _rd = new Random();
        }
        public Treap(int seed)
        {
            _rd = new Random(seed);
        }

        protected static void Split(TreapNode? t, TElement key, ref TreapNode? l, ref TreapNode? r)
        {
            if (t == null)
            {
                l = null;
                r = null;
            }
            else if (t.Key.CompareTo(key) <= 0)
            {
                Split(t.Right, key, ref t.Right, ref r);
                l = t;
            }
            else
            {
                Split(t.Left, key, ref l, ref t.Left);
                r = t;
            }
        }
        protected static void Merge(ref TreapNode? t, TreapNode? l, TreapNode? r)
        {
            if (l == null || r == null)
            {
                t = l ?? r;
            }
            else if (l.Priority > r.Priority)
            {
                Merge(ref l.Right, l.Right, r);
                t = l;
            }
            else
            {
                Merge(ref r.Left, l, r.Left);
                t = r;
            }
        }
        protected static void Insert(ref TreapNode? t, TreapNode it)
        {
            if (t == null)
            {
                t = it;
            }
            else if (it.Priority > t.Priority)
            {
                Split(t, it.Key, ref it.Left, ref it.Right);
                t = it;
            }
            else
            {
                if (t.Key.CompareTo(it.Key) <= 0)
                    Insert(ref t.Right, it);
                else
                    Insert(ref t.Left, it);
            }
        }
        protected static void Remove(ref TreapNode? t, TElement key)
        {
            if (t == null)
                return;

            var comp = t.Key.CompareTo(key);
            if (comp == 0)
            {
                Merge(ref t, t.Left, t.Right);
            }
            else
            {
                if (comp < 0)
                    Remove(ref t.Left, key);
                else
                    Remove(ref t.Right, key);
            }
        }

        protected TreapNode? Find(TElement elem)
        {
            var curr = _root;
            while (curr != null)
            {
                var comp = curr.Key.CompareTo(elem);

                if (comp == 0)
                    return curr;
                else
                    curr = comp < 0 ? curr.Right : curr.Left;
            }

            return null;
        }

        public void Insert(TElement elem) => Insert(ref _root, new TreapNode(_rd.Next(), elem));
        public void Remove(TElement elem) => Remove(ref _root, elem);
        public bool Contains(TElement element) => Find(element) != null;
    }
}
