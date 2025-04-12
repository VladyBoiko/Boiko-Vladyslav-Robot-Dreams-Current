namespace Enemy.BehaviourTreeSystem
{
    public class BehaviourLeaf : BehaviourNode
    {
        private readonly byte _behaviourId;

        public BehaviourLeaf(byte behaviourId)
        {
            _behaviourId = behaviourId;
        }
        
        public override byte GetBehaviourId()
        {
            return _behaviourId;
        }
    }
}