using System;

namespace Enemy.BehaviourTreeSystem
{
    public class BehaviourBranch : BehaviourNode
    {
        private readonly BehaviourNode _trueNode;
        private readonly BehaviourNode _falseNode;
        private readonly Func<bool> _condition;

        public BehaviourBranch(BehaviourNode trueNode, BehaviourNode falseNode, Func<bool> condition)
        {
            _trueNode = trueNode;
            _falseNode = falseNode;
            _condition = condition;
        }
        
        public override byte GetBehaviourId()
        {
            if (_condition.Invoke())
                return _trueNode.GetBehaviourId();
            else
                return _falseNode.GetBehaviourId();
        }
    }
}