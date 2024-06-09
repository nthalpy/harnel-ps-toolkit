using ProblemSolving.Templates.Merger;
using System;

namespace ProblemSolving.Templates.Splay
{
    [IncludeIfReferenced]
    public abstract class KeyedSplayNode<TKey, TSelf> : SplayNode<TSelf>
        where TKey : IComparable<TKey>
        where TSelf : KeyedSplayNode<TKey, TSelf>
    {
        public TKey Key;

        public KeyedSplayNode(TKey key)
        {
            Key = key;
        }
    }
}
