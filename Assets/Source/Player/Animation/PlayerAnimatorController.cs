using System;
using System.Collections.Generic;
using HealthSystems;
using InteractablesSystem;
using Services;
using UnityEngine;

namespace Player.Animation
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private enum PlayerAnimState
        {
            Idle,
            Movement,
            Jump,
            Interact,
            Hit
        }
        
        [SerializeField] private Animator _animator;
        [SerializeField] private float _dampTime = 0.125f;
        [SerializeField] private float _crossFadeDuration = 0.125f;
        [SerializeField] private Interactor _interactor;
        [SerializeField] private HandsIK _handsIK;
        [SerializeField] private Health _health;
        
        [Space, Header("States")]
        [SerializeField] private string _idleName;
        [SerializeField] private string _movementName;
        [SerializeField] private string _jumpName;
        [SerializeField] private string _interactName;
        [SerializeField] private string _hitName;
        
        [Space, Header("Parameters")]
        [SerializeField] private string _horizontalName;
        [SerializeField] private string _verticalName;
        
        private int _idleId;
        private int _movementId;
        private int _jumpId;
        private int _interactId;
        private int _hitId;
        
        private int _horizontalId;
        private int _verticalId;
        
        private InputController _inputController;
        private PlayerJumper _playerJumper;
        
        private Vector2 _inputValue;
        private bool _isJumping;
        private bool _isInteracting;
        private bool _isHitted;
        
        private Dictionary<PlayerAnimState, int> _stateNames;
        private List<AnimationCondition> _conditions;
        private PlayerAnimState _currentState = PlayerAnimState.Idle;
        
        private class AnimationCondition
        {
            public Func<bool> Condition;
            public PlayerAnimState State;

            public AnimationCondition(Func<bool> condition, PlayerAnimState state)
            {
                Condition = condition;
                State = state;
            }
        }
        
        private void Start()
        {
            _idleId = Animator.StringToHash(_idleName);
            _movementId = Animator.StringToHash(_movementName);
            _jumpId = Animator.StringToHash(_jumpName);
            _interactId = Animator.StringToHash(_interactName);
            _hitId = Animator.StringToHash(_hitName);
            
            _horizontalId = Animator.StringToHash(_horizontalName);
            _verticalId = Animator.StringToHash(_verticalName);
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnMoveInput += MoveHandler;
            _inputController.OnJumpInput += JumpHandler;
            
            _playerJumper = ServiceLocator.Instance.GetService<PlayerJumper>();
            _playerJumper.OnJumpEnded += JumpEndHandler;

            _interactor.OnInteract += InteractHandler;

            _health.OnTakeDamage += TakeDamageHandler;
            
            InitStateNames();
            InitConditions();
            
            _inputController.Lock();
            _handsIK.DisableIK();
        }

        private void OnDestroy()
        {
            if (_inputController != null)
            {
                _inputController.OnMoveInput -= MoveHandler;
                _inputController.OnJumpInput -= JumpHandler;
            }
            if(_playerJumper != null)
                _playerJumper.OnJumpEnded -= JumpEndHandler;
            if(_interactor != null)
                _interactor.OnInteract -= InteractHandler;
            if(_health != null)
                _health.OnTakeDamage -= TakeDamageHandler;
        }
        
        private void Update()
        {
            _animator.SetFloat(_horizontalId, _inputValue.x, _dampTime, Time.deltaTime);
            _animator.SetFloat(_verticalId, _inputValue.y, _dampTime, Time.deltaTime);
            
            HandleStateChange();
        }
        
        private void MoveHandler(Vector2 moveInput)
        {
            _inputValue = moveInput;
        }

        private void JumpHandler(bool performed)
        {
            if (performed && !_isJumping)
            {
                _isJumping = true;
            }
        }

        private void JumpEndHandler()
        {
            if(_isJumping)
                _isJumping = false;
        }
        
        private void InteractHandler(IInteractable iInteractable)
        {
            _handsIK.DisableIK();
            _isInteracting = true;
            _inputController.Lock();
        }

        public void InteractionEnded()
        {
            _handsIK.EnableIK();
            _isInteracting = false;
            _inputController.Unlock();
        }

        public void CharacterRevived()
        {
            _inputController.Unlock();
            _handsIK.EnableIK();
        }

        private void TakeDamageHandler(float damageTaken)
        {
            _isHitted = true;
        }

        public void HitEnd()
        {
            _isHitted = false;
        }
        
        private void HandleStateChange()
        {
            for (var i = 0; i < _conditions.Count; i++)
            {
                var condition = _conditions[i];
                if (condition.Condition())
                {
                    if (_currentState != condition.State)
                    {
                        _currentState = condition.State;
                        _animator.CrossFadeInFixedTime(_stateNames[_currentState], _crossFadeDuration);
                    }

                    break;
                }
            }
        }
        
        private void InitStateNames()
        {
            _stateNames = new Dictionary<PlayerAnimState, int>
            {
                { PlayerAnimState.Idle, _idleId },
                { PlayerAnimState.Movement, _movementId },
                { PlayerAnimState.Jump, _jumpId },
                { PlayerAnimState.Interact , _interactId},
                { PlayerAnimState.Hit , _hitId},
            };
        }
        
        private void InitConditions()
        {
            _conditions = new List<AnimationCondition>
            {
                new(()=> _isHitted, PlayerAnimState.Hit),
                new(() => _isJumping, PlayerAnimState.Jump),
                new(() => _inputValue.sqrMagnitude > 0.01f, PlayerAnimState.Movement),
                new(() => _isInteracting, PlayerAnimState.Interact),
                
                new(() => true, PlayerAnimState.Idle),
            };
        }
    }
}