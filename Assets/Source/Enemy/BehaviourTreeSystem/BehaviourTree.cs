using Enemy.BehaviourTreeSystem;

namespace Enemy
{
    public class BehaviourTree
    {
        private readonly BehaviourNode _root;

        public BehaviourTree(BehaviourNode root)
        {
            _root = root;
        }

        public byte GetBehaviourId() => _root.GetBehaviourId();
    }
}