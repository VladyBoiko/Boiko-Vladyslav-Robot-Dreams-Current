using Services;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class PatrolBehaviour : BehaviourStateBase
    {
        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly Transform _characterTransform;

        private NavPointProviderZone _navPointProviderZone;
        public PatrolBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;
        }

        public override void Enter()
        {
            base.Enter();

            _agent.speed = enemyController.Data.PatrolSpeed;
            // _agent.SetDestination(enemyController.NavPointProvider.GetPoint());
            _navPointProviderZone = ServiceLocator.Instance.GetService<NavPointProviderZone>();
            _agent.SetDestination(_navPointProviderZone.GetPoint());
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Vector3 velocity = _agent.desiredVelocity;
            velocity.y = 0;
            Vector3 position = _characterTransform.position;

            _characterController.Move(velocity * (deltaTime * enemyController.Data.PatrolSpeed) + Physics.gravity);

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

            if (ArrivedAtDestination())
            {
                // Debug.Log("Arrived");
                enemyController.PatrolStamina = 0f;
                enemyController.ComputeBehaviour();
            }
        }

        private bool ArrivedAtDestination()
        {
            // return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
            
            bool stoppedByDistance = !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
            bool stoppedByVelocity = _agent.desiredVelocity.sqrMagnitude < 0.01f;

            return stoppedByDistance || stoppedByVelocity;
        }

        public override void Dispose()
        {
        }
    }
}
