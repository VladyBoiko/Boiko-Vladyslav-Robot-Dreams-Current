using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class SearchBehaviour : BehaviourStateBase
    {
        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly Transform _characterTransform;

        public SearchBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;
        }

        public override void Enter()
        {
            base.Enter();

            _agent.speed = enemyController.Data.ChaseSpeed;
            _agent.SetDestination(enemyController.Playerdar.LastTargetPosition);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Vector3 velocity = _agent.desiredVelocity;
            velocity.y = 0;
            Vector3 position = _characterTransform.position;

            _characterController.Move(velocity * (deltaTime * enemyController.Data.ChaseSpeed) + Physics.gravity);

            Vector3 newPosition = _characterTransform.position;
            Vector3 direction = newPosition - position;
            float distance = direction.magnitude;

            _agent.nextPosition = newPosition;
            enemyController.PatrolStamina -= distance;

            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            if (!Mathf.Approximately(direction.magnitude, 0f))
            {
                _characterTransform.rotation = Quaternion.LookRotation(direction);
            }

            if (_agent.remainingDistance <= enemyController.Data.LookAroundDistance)
            {
                enemyController.Playerdar.LookAround();
            }

            if (ArrivedAtDestination())
            {
                enemyController.ComputeBehaviour();
            }
        }

        private bool ArrivedAtDestination()
        {
            return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
        }

        public override void Dispose()
        {
        }
    }
}
