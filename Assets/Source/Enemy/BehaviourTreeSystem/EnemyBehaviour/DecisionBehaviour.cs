namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class DecisionBehaviour : BehaviourStateBase
    {
        public DecisionBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemyController.ComputeBehaviour();
        }

        public override void Dispose()
        {
        }
    }
}