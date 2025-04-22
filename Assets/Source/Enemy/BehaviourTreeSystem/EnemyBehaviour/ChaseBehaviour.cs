using Player;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class ChaseBehaviour : BehaviourStateBase
    {
        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly Transform _characterTransform;
        private readonly PlayerController _playerController;

        public ChaseBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;
            _playerController = ServiceLocator.Instance.GetService<PlayerController>();
        }

        public override void Enter()
        {
            base.Enter();
            _agent.speed = enemyController.Data.PatrolSpeed;
            UpdateDestination();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (_playerController == null) return;

            UpdateDestination();

            Vector3 velocity = _agent.desiredVelocity;
            velocity.y = 0;
            Vector3 position = _characterTransform.position;

            _characterController.Move(velocity * (deltaTime * enemyController.Data.PatrolSpeed) + Physics.gravity);

            Vector3 newPosition = _characterTransform.position;
            Vector3 direction = newPosition - position;

            _agent.nextPosition = newPosition;

            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            if (!Mathf.Approximately(direction.magnitude, 0f))
            {
                _characterTransform.rotation = Quaternion.LookRotation(direction);
            }
        }

        private void UpdateDestination()
        {
            if (_playerController != null)
            {
                _agent.SetDestination(_playerController.gameObject.transform.position);
            }
        }

        public override void Dispose()
        {
        }
    }
}
