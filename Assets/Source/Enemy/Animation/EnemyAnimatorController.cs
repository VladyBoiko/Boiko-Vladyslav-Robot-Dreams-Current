using System;
using Enemy.BehaviourTreeSystem.EnemyBehaviour;
using HealthSystems;
using Services;
using UnityEngine;

namespace Enemy.Animation
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private float _crossFadeTime = 0.125f;
        [SerializeField] private float _dampTime = 0.125f;
        
        [Space, Header("States")]
        [SerializeField] private string _idleName;
        [SerializeField] private string _movementName;
        [SerializeField] private string _deathName;
        [SerializeField] private string _meleeAttackName;
        
        [Space, Header("Parameters")]
        [SerializeField] private string _horizontalName;
        [SerializeField] private string _verticalName;

        private int _idleId;
        private int _movementId;
        private int _deathId;
        private int _meleeAttackId;
        
        private int _horizontalId;
        private int _verticalId;
        
        private Vector2 _movementValue;

        // private MeleeAttackBehaviour _meleeAttackBehaviour;
        
        private void Awake()
        {
            _idleId = Animator.StringToHash(_idleName);
            _movementId = Animator.StringToHash(_movementName);
            _deathId = Animator.StringToHash(_deathName);
            _meleeAttackId = Animator.StringToHash(_meleeAttackName);
            
            _horizontalId = Animator.StringToHash(_horizontalName);
            _verticalId = Animator.StringToHash(_verticalName);

            _enemyController.onBehaviourChanged += BehaviourStateHandler;

            // if (_meleeAttackBehaviour != null)
            // {
            //     _meleeAttackBehaviour.OnMeleeAttackStarted += MeleeAttackStartedHandler;
            //     _meleeAttackBehaviour.OnMeleeAttackEndedToIdle += MeleeAttackEndedToIdleHandler;
            //     _meleeAttackBehaviour.OnMeleeAttackEndedToMovement += MeleeAttackEndedToMovementHandler;
            // }
        }

        private void OnDestroy()
        {
            _enemyController.onBehaviourChanged -= BehaviourStateHandler;
            
            // if (_meleeAttackBehaviour != null)
            // {
            //     _meleeAttackBehaviour.OnMeleeAttackStarted -= MeleeAttackStartedHandler;
            //     _meleeAttackBehaviour.OnMeleeAttackEndedToIdle -= MeleeAttackEndedToIdleHandler;
            //     _meleeAttackBehaviour.OnMeleeAttackEndedToMovement -= MeleeAttackEndedToMovementHandler;
            // }
        }
        
        private void BehaviourStateHandler(EnemyBehaviour behaviour)
        {
            switch (behaviour)
            {
                case EnemyBehaviour.Idle:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    break;
                case EnemyBehaviour.Patrol:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.up;
                    break;
                case EnemyBehaviour.Search:
                    _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
                    _movementValue = Vector2.up;
                    break;
                case EnemyBehaviour.Attack:
                    _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    break;
                case EnemyBehaviour.Death:
                    _animator.CrossFadeInFixedTime(_deathId, _crossFadeTime);
                    _movementValue = Vector2.zero;
                    break;
            }
        }
        
        public void CharacterRevived()
        {
            _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
            _movementValue = Vector2.zero;
        }
        
        public void TriggerMeleeAttack()
        {
            _animator.CrossFadeInFixedTime(_meleeAttackId, _crossFadeTime);
            _movementValue = Vector2.zero;
        }
        
        public void TriggerIdle()
        {
            _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
            _movementValue = Vector2.zero;
        }
        
        public void TriggerMovement()
        {
            _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
            _movementValue = Vector2.up;
        }

        // private void MeleeAttackStartedHandler()
        // {
        //     _animator.CrossFadeInFixedTime(_meleeAttackId, _crossFadeTime);
        //     _movementValue = Vector2.zero;
        // }
        //
        // private void MeleeAttackEndedToIdleHandler()
        // {
        //     _animator.CrossFadeInFixedTime(_idleId, _crossFadeTime);
        //     _movementValue = Vector2.zero;
        // }
        //
        // private void MeleeAttackEndedToMovementHandler()
        // {
        //     _animator.CrossFadeInFixedTime(_movementId, _crossFadeTime);
        //     _movementValue = Vector2.up;
        // }
        //
        // public void SetMeleeAttackBehaviour(MeleeAttackBehaviour meleeAttackBehaviour)
        // {
        //     _meleeAttackBehaviour = meleeAttackBehaviour;
        // }
        
        private void Update()
        {
            _animator.SetFloat(_horizontalId, _movementValue.x, _dampTime, Time.deltaTime);
            _animator.SetFloat(_verticalId, _movementValue.y, _dampTime, Time.deltaTime);
        }
    }
}