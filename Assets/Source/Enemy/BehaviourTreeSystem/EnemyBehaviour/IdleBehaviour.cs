using UnityEngine;

namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class IdleBehaviour : BehaviourStateBase
    {
        
        private float _time;
        private float _duration;

        public IdleBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _time = 0f;
            Vector2 durationBounds = enemyController.Data.IdleDuration;
            _duration = Random.Range(durationBounds.x, durationBounds.y);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            _time += deltaTime;
            
            if (_time >= _duration)
            {
                enemyController.RestorePatrolStamina();
                enemyController.ComputeBehaviour();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Dispose()
        {
        }
    }
}