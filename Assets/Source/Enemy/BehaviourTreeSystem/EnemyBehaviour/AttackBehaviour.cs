using UnityEngine;

namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class AttackBehaviour : BehaviourStateBase
    {
        private BehaviourStateBase _currentAttack;
        private readonly MeleeAttackBehaviour _meleeAttack;
        private readonly ShootBehaviour _shootAttack;

        private const float MeleeDistance = 3f;
        private const float MeleeToRangedThreshold = 5f;

        public AttackBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
            _meleeAttack = new MeleeAttackBehaviour(stateId, enemyController);
            _shootAttack = new ShootBehaviour(stateId, enemyController);
        }

        public override void Enter()
        {
            base.Enter();
            UpdateAttackType();
            _currentAttack?.Enter();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            UpdateAttackType();
            _currentAttack?.Update(deltaTime);
        }

        private void UpdateAttackType()
        {
            var target = enemyController.Playerdar.CurrentTarget;
            if (target == null) return;

            float distance = Vector3.Distance(
                enemyController.CharacterTransform.position,
                target.TargetPivot.position);

            BehaviourStateBase nextAttack = distance <= MeleeDistance
                ? _meleeAttack
                : distance >= MeleeToRangedThreshold
                    ? _shootAttack
                    : _currentAttack;

            if (nextAttack != _currentAttack)
            {
                _currentAttack?.Exit();
                _currentAttack = nextAttack;
                _currentAttack.Enter();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _currentAttack?.Exit();
        }

        public override void Dispose()
        {
            _meleeAttack?.Dispose();
            _shootAttack?.Dispose();
        }
    }
}