using System;
using System.Collections.Generic;
using BillBoards;
using DamageSystems;
using Enemy.BehaviourTreeSystem;
using Enemy.BehaviourTreeSystem.EnemyBehaviour;
using HealthSystems;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Attributes;
using Enemy.Animation;
using Services;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public event Action<EnemyBehaviour> onBehaviourChanged;
        
        [SerializeField] private HealthIndicator _healthIndicator;
        [SerializeField] private EnemyData _data;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private Health _health;
        [SerializeField] private GameObject _rootObject;
        [SerializeField] private Transform _meshRendererTransform;
        [SerializeField] private Transform _fallMark;
        [SerializeField] private Transform _weaponTransform;
        // [SerializeField] private Shooter _shooter;
        [SerializeField] private EnemyShooter _shooter;
        [SerializeField] private DamageDealerBase _damageDealer;
        [SerializeField] private WeaponData _weaponData;

        [SerializeField] private PlayerRadar _playerRadar;
        
        [SerializeField] private BillboardBase _billboard;
        
        [SerializeField, ReadOnly] private float _patrolStamina;
        [SerializeField, ReadOnly] private EnemyBehaviour _currentBehaviour;
        
        protected BehaviourTree _behaviourTree;
        // protected StateMachine _behaviourMachine;
        
        // public StateMachine BehaviourMachine => _behaviourMachine;
        
        private Dictionary<byte, BehaviourStateBase> _behaviourStates;
        private BehaviourStateBase _currentState;
        
        [SerializeField] private EnemyAnimatorController _animatorController; 
        
        public float PatrolStamina
        {
            get => _patrolStamina;
            set => _patrolStamina = Mathf.Clamp(value, 0, _data.MaxPatrolStamina); 
        }
        
        public EnemyData Data => _data;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        // public INavPointProvider NavPointProvider => _navPointProvider;
        public CharacterController CharacterController => _characterController;
        public Transform CharacterTransform => _characterTransform;
        public Health Health => _health;
        public PlayerRadar Playerdar => _playerRadar;
        public Transform FallMark => _fallMark;
        public GameObject RootObject => _rootObject;
        public Transform MeshRendererTransform => _meshRendererTransform;
        public Transform WeaponTransform => _weaponTransform;
        // public Shooter Shooter => _shooter;
        public EnemyShooter Shooter => _shooter;
        public DamageDealerBase DamageDealer => _damageDealer;
        public WeaponData WeaponData => _weaponData;
        
        public BillboardBase Billboard => _billboard;

        public EnemyAnimatorController AnimatorController => _animatorController;
        
        private void Awake()
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.avoidancePriority = Random.Range(0, 100);
        
            /*InitStateMachine();
            _behaviourMachine.OnStateChange += StateChangeHandler;*/
            // RestorePatrolStamina();
            
            InitStates();
            // Initialize(_navPointProvider, Camera.main);
        }

        private void OnEnable()
        {
            _health.OnDeath += HealthDeathHandler;
            
            _characterController.enabled = true;
            _playerRadar.ResetRadar();
            _patrolStamina = 0;
            InitBehaviourTree();
        }

        private void OnDisable()
        {
            _health.OnDeath -= HealthDeathHandler;
        }

        private void FixedUpdate()
        {
            ComputeBehaviour();
            _currentState?.Update(Time.fixedDeltaTime);
        }
        
        // protected virtual void InitStateMachine()
        // {
        //     _behaviourMachine = new StateMachine();
        //
        //     _behaviourMachine.AddState((byte)EnemyBehaviour.Deciding,
        //         new DecisionBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Deciding, this));
        //     _behaviourMachine.AddState((byte)EnemyBehaviour.Idle,
        //         new IdleBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Idle, this));
        //     _behaviourMachine.AddState((byte)EnemyBehaviour.Patrol,
        //         new PatrolBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Patrol, this));
        //     _behaviourMachine.AddState((byte)EnemyBehaviour.Search,
        //         new SearchBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Search, this));
        //     
        //     /*_behaviourMachine.AddState((byte)EnemyBehaviour.Attack,
        //         new AttackBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));*/
        //     _behaviourMachine.AddState((byte)EnemyBehaviour.Attack,
        //         new ShootBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Attack, this));
        //     
        //     _behaviourMachine.AddState((byte)EnemyBehaviour.Death,
        //         new DeathBehaviour(_behaviourMachine, (byte)EnemyBehaviour.Death, this));
        // }
        
        private void InitStates()
        {
            _behaviourStates = new Dictionary<byte, BehaviourStateBase>
            {
                { (byte)EnemyBehaviour.Deciding, 
                    new DecisionBehaviour((byte)EnemyBehaviour.Deciding, this) },
                { (byte)EnemyBehaviour.Idle, 
                    new IdleBehaviour((byte)EnemyBehaviour.Idle, this) },
                // { (byte)EnemyBehaviour.Patrol, 
                //     new PatrolBehaviour((byte)EnemyBehaviour.Patrol, this) },
                { (byte)EnemyBehaviour.Patrol, 
                    new ChaseBehaviour((byte)EnemyBehaviour.Patrol, this) },
                { (byte)EnemyBehaviour.Search, 
                    new SearchBehaviour((byte)EnemyBehaviour.Search, this) },
                { (byte)EnemyBehaviour.Attack, 
                    new AttackBehaviour((byte)EnemyBehaviour.Attack, this) },
                // { (byte)EnemyBehaviour.Attack,
                //     new ShootBehaviour((byte)EnemyBehaviour.Attack, this) },
                { (byte)EnemyBehaviour.Death, 
                    new DeathBehaviour((byte)EnemyBehaviour.Death, this) }
            };
        }

        public void ComputeBehaviour()
        {
            if (_behaviourTree == null) return;
            
            byte newBehaviourId = _behaviourTree.GetBehaviourId();
            // Debug.Log($"Current: {_currentState?.StateId}, New: {newBehaviourId}");
            // Debug.Log($"HasTarget: {_playerRadar.HasTarget}, SeesTarget: {_playerRadar.SeesTarget}");

            if (_currentState == null || _currentState.StateId != newBehaviourId)
            {
                // Debug.Log($"[EnemyController] Switching state from {_currentState?.StateId} to {newBehaviourId}");
                _currentState?.Exit();
                _currentState = _behaviourStates[newBehaviourId];
                _currentBehaviour = (EnemyBehaviour)newBehaviourId;
                _currentState.Enter();
                
                onBehaviourChanged?.Invoke(_currentBehaviour);
            }
        }
        
        protected virtual void InitBehaviourTree()
        {
            BehaviourLeaf idleLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Idle);
            BehaviourLeaf patrolLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Patrol);
        
            BehaviourBranch patrolBranch = new BehaviourBranch(patrolLeaf, idleLeaf, PatrolStaminaCondition);
        
            BehaviourLeaf attackLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Attack);
            BehaviourLeaf searchLeaf = new BehaviourLeaf((byte)EnemyBehaviour.Search);
            
            BehaviourBranch seesTarget = new BehaviourBranch(attackLeaf, searchLeaf, SeesTargetCondition);
        
            BehaviourBranch hasTarget = new BehaviourBranch(seesTarget, patrolBranch, HasTargetCondition);
        
            _behaviourTree = new BehaviourTree(hasTarget);
        
            ComputeBehaviour();
        }
        
        // private void FixedUpdate()
        // {
        //     _behaviourMachine.Update(Time.fixedDeltaTime);
        // }
        
        // private void StateChangeHandler(byte stateId)
        // {
        //     _currentBehaviour = (EnemyBehaviour)stateId;
        // }
        
        // public void Initialize(INavPointProvider navPointProvider, Camera camera)
        // {
        //     _navPointProvider = navPointProvider;
        //     _healthIndicator.Billboard.SetCamera(camera);
        // }
        
        // public void ComputeBehaviour()
        // {
        //     if (_behaviourTree == null)
        //         return;
        //     // _behaviourMachine.SetState(_behaviourTree.GetBehaviourId());
        // }
        
        public void RestorePatrolStamina()
        {
            _patrolStamina = _data.MaxPatrolStamina;
        }
        
        protected bool PatrolStaminaCondition()
        {
            return _patrolStamina > 0;
        }
        
        protected bool HasTargetCondition()
        {
            return _playerRadar.HasTarget;
        }
        
        protected bool SeesTargetCondition()
        {
            return _playerRadar.SeesTarget;
        }
        
        // protected void HealthDeathHandler()
        // {
        //     _behaviourTree = null;
        //     
        //     Debug.Log($"[EnemyController] HealthDeathHandler: {name} died! Alive = {_health.IsAlive}");
        //     
        //     if (_behaviourStates.TryGetValue((byte)EnemyBehaviour.Death, out var deathState))
        //     {
        //         _currentState?.Exit();
        //         _currentState = deathState;
        //         _currentState.Enter();
        //     }
        //     
        //     _health.HealthSystem.InvokeCharacterDeath(_health);
        //     
        //     // _health.UnregisterFromSystem();
        //     //     _behaviourMachine.ForceState((byte)EnemyBehaviour.Death);
        //     //     ServiceLocator.Instance.GetService<IHealthService>().RemoveCharacter(_health);
        // }
        
        protected void HealthDeathHandler()
        {
            _behaviourTree = null;
    
            Debug.Log($"[EnemyController] HealthDeathHandler: {name} died! Alive = {_health.IsAlive}");

            if (_currentState != null)
                _currentState.Exit();

            if (_behaviourStates.TryGetValue((byte)EnemyBehaviour.Death, out var deathState))
            {
                _currentState = deathState;
                _currentBehaviour = EnemyBehaviour.Death;
                _currentState.Enter();
                onBehaviourChanged?.Invoke(_currentBehaviour);
            }
            
            _health.HealthSystem.InvokeCharacterDeath(_health);
        }
    }
}