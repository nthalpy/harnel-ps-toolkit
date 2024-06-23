namespace ProblemSolving.Templates.Splay
{
    [IncludeIfReferenced]
    public abstract class SplayNode<TSelf> where TSelf : SplayNode<TSelf>
    {
        public TSelf? Parent;
        public TSelf? Left;
        public TSelf? Right;

        public bool IsLeftChild => Parent?.Left == this;
        public bool IsRightChild => Parent?.Right == this;

        public int SubtreeSize;

        public virtual void UpdateTracked()
        {
            SubtreeSize = 1 + (Left?.SubtreeSize ?? 0) + (Right?.SubtreeSize ?? 0);
        }
    }
}
