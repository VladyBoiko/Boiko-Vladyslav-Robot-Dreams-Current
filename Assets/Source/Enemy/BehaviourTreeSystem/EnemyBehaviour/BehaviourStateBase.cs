namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public abstract class BehaviourStateBase
    {
        public byte StateId { get; }
        protected readonly EnemyController enemyController;

        protected BehaviourStateBase(byte stateId, EnemyController enemyController)
        {
            StateId = stateId;
            this.enemyController = enemyController;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update(float deltaTime) { }
        public virtual void Dispose() { }
    }
}