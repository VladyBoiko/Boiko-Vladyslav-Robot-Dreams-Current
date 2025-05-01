using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class MeleeAttackBehaviour : BehaviourStateBase
    {
        public event Action OnMeleeAttackStarted;
        public event Action OnMeleeAttackEndedToIdle;
        public event Action OnMeleeAttackEndedToMovement;
        public event Action<string, Vector3, Vector3, Collider> OnMeleeAttackHit;
        
        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly Transform _characterTransform;
        private readonly Transform _weaponTransform;
        
        private readonly float _attackCooldown = 0.5f;
        private float _cooldownTimer;

        private const float MeleeRange = 1.5f;
        private const float DamageDelay = 1.0f;
        private bool _isAttacking;
        private float _attackAnimationTimer;
        
        private Vector3 _weaponInitialPosition;
        private Quaternion _weaponInitialRotation;
        private Vector3 _weaponTargetPosition;
        private Quaternion _weaponTargetRotation;
        
        public MeleeAttackBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;
            _weaponTransform = enemyController.WeaponTransform;
            
            _weaponInitialPosition = _weaponTransform.localPosition;
            _weaponInitialRotation = _weaponTransform.localRotation;
            
            _weaponTargetPosition = _weaponInitialPosition + Vector3.forward * 0.75f;
            _weaponTargetRotation = _weaponInitialRotation * Quaternion.Euler(0f, -105f, 0f);
        }

        public override void Enter()
        {
            base.Enter();
            _cooldownTimer = 0f;
            _isAttacking = false;
            _agent.isStopped = false;
            
            enemyController.AnimatorController.TriggerMovement();
            
            // Debug.Log("[MeleeAttack] Entered Melee Attack Behaviour");
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var target = enemyController.Playerdar.CurrentTarget;
            if (target == null) return;

            Vector3 targetPosition = target.TargetPivot.position;
            float distance = Vector3.Distance(_characterTransform.position, targetPosition);

            if (_isAttacking)
            {
                _attackAnimationTimer -= deltaTime;

                // AttackAnimation();
                
                if (_attackAnimationTimer <= 0f)
                {
                    if (distance <= MeleeRange)
                    {
                        ExecuteAttack();
                    }
                    else
                    {
                        Debug.Log("[MeleeAttack] Player dodged the attack. Resuming pursuit.");
                    }
                    ResetAttackState();
                }
                return;
            }
            
            if (distance > MeleeRange)
            {
                MoveToTarget(targetPosition, deltaTime);
                return;
            }

            _agent.ResetPath();

            _cooldownTimer -= deltaTime;
            if (_cooldownTimer <= 0f)
            {
                StartAttack();
                _cooldownTimer = _attackCooldown;
            }
        }

        private void MoveToTarget(Vector3 targetPosition, float deltaTime)
        {
            _agent.SetDestination(targetPosition);

            Vector3 velocity = _agent.desiredVelocity;
            velocity.y = 0;
            Vector3 position = _characterTransform.position;

            _characterController.Move(velocity * (deltaTime * enemyController.Data.ChaseSpeed) + Physics.gravity);

            Vector3 newPosition = _characterTransform.position;
            Vector3 direction = newPosition - position;
            _agent.nextPosition = newPosition;

            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            if (!Mathf.Approximately(direction.magnitude, 0f))
                _characterTransform.rotation = Quaternion.LookRotation(direction);
        }
        
        private void StartAttack()
        {
            _isAttacking = true;
            _attackAnimationTimer = DamageDelay;
            
            enemyController.AnimatorController.TriggerMeleeAttack();
            
            // OnMeleeAttackStarted?.Invoke();
            
            // Debug.Log("[MeleeAttack] Started attack");
        }

        private void AttackAnimation()
        {
            float time = 1f - (_attackAnimationTimer / DamageDelay);
            time = Mathf.Clamp01(time);
                
            float anim = time <= 0.5f ? time * 2f : (1f - time) * 2f;
                
            _weaponTransform.localPosition = Vector3.Lerp(_weaponInitialPosition, _weaponTargetPosition, anim);
            _weaponTransform.localRotation = Quaternion.Slerp(_weaponInitialRotation, _weaponTargetRotation, anim);
        }
        
        private void ExecuteAttack()
        {
            // Debug.Log("[MeleeAttack] Damage dealt to player!");
            // enemyController.Playerdar.PlayerController.Health.TakeDamage(enemyController.WeaponData.Damage);
            
            var target = enemyController.Playerdar.CurrentTarget;
            if (target == null) return;
            
            Vector3 hitPoint = target.TargetPivot.position;
            Vector3 direction = (hitPoint - _characterTransform.position).normalized;
            Vector3 normal = -direction;
            // Collider hitCollider = target.TargetPivot.GetComponentInParent<CharacterController>();
            Collider hitCollider = enemyController.Playerdar.PlayerController.CharacterController;

            OnMeleeAttackHit?.Invoke("MeleeAttack", hitPoint, normal, hitCollider);
        }
        
        private void ResetAttackState()
        {
            _isAttacking = false;
            _attackAnimationTimer = 0f;
            
            enemyController.AnimatorController.TriggerMovement();
            
            // OnMeleeAttackEndedToMovement?.Invoke();
            
            _weaponTransform.localPosition = _weaponInitialPosition;
            _weaponTransform.localRotation = _weaponInitialRotation;
        }

        public override void Exit()
        {
            base.Exit();
            
            enemyController.AnimatorController.TriggerIdle();
            
            // OnMeleeAttackEndedToIdle?.Invoke();
            
            _agent.ResetPath();
            
            _weaponTransform.localPosition = _weaponInitialPosition;
            _weaponTransform.localRotation = _weaponInitialRotation;
            
            // Debug.Log("[MeleeAttack] Exited Melee Attack Behaviour");
        }
    }
}
