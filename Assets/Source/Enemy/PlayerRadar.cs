using Player;
using UnityEngine;

namespace Enemy
{
    public class PlayerRadar : MonoBehaviour
    {
        public enum State
        {
            Scanning,
            Chasing,
            Searching,
        }

        [SerializeField] private EnemyController _enemyController;
        [SerializeField] private float _range;
        [SerializeField] private float _angle;
        [SerializeField] private PlayerController _playerController;

        [SerializeField, /*ReadOnly*/] private Collider[] _playerColliders;
        
        private float _cosine;
        
        private Transform _transform;
        
        private TargetableBase _currentTarget;
        private bool _hasTarget;
        private bool _seesTarget;
        private Vector3 _lastTargetPosition;
        
        private State _currentState;
        
        [ContextMenu("Find Player Colliders")]
        private void FindPlayerColliders()
        {
            
            _playerColliders = _playerController.GetComponentsInChildren<Collider>();
            
            Debug.Log($"Found {_playerColliders.Length} colliders for {gameObject.name}:");
            foreach (Collider collider in _playerColliders)
            {
                Debug.Log(collider.gameObject.name);
            }
        }
        
        public State CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                    return;
                _currentState = value;
                _enemyController.ComputeBehaviour();
            }
        }
        
        public TargetableBase CurrentTarget => _currentTarget;
        public bool HasTarget => _hasTarget;
        public bool SeesTarget => _seesTarget;
        public Vector3 LastTargetPosition => _lastTargetPosition;
        public PlayerController PlayerController => _playerController;
        
        private void Start()
        {
            _transform = transform;
            _cosine = Mathf.Cos(_angle * Mathf.Deg2Rad);

            _enemyController.Health.OnHealthChanged += HealthChangedHandler;
        }

        private void FixedUpdate()
        {
            switch (CurrentState)
            {
                case State.Scanning:
                    ScanningUpdate();
                    break;
                case State.Chasing:
                    ChasingUpdate();
                    break;
                case State.Searching:
                    SearchingUpdate();
                    break;
            }
        }

        private void ScanningUpdate()
        {
            // Debug.Log("[Radar] Scanning...");
            
            if (!CheckTarget(_playerController.Targetable))
                return;
            
            _currentTarget = _playerController.Targetable;
            _hasTarget = true;
            _seesTarget = true;
            CurrentState = State.Chasing;
        }

        private void ChasingUpdate()
        {
            _lastTargetPosition = _currentTarget.TargetPivot.position;
            
            if (CheckTarget(_currentTarget))
                return;
            
            _seesTarget = false;
            CurrentState = State.Searching;
        }

        private void SearchingUpdate()
        {
            if (!CheckTarget(_currentTarget))
                return;

            _seesTarget = true;
            CurrentState = State.Chasing;
        }

        public void LookAround()
        {
            if (_hasTarget)
            {
                if (CheckTarget(_currentTarget, false))
                {
                    _seesTarget = true;
                    CurrentState = State.Chasing;
                }
                else
                {
                    _seesTarget = false;
                    CurrentState = State.Scanning;
                }
            }
            else
            {
                if (CheckTarget(_playerController.Targetable, false))
                {
                    _hasTarget = true;
                    _seesTarget = true;
                    _currentTarget = _playerController.Targetable;
                    CurrentState = State.Chasing;
                }
                else
                {
                    _seesTarget = false;
                    CurrentState = State.Scanning;
                }
            }
        }
        
        private bool CheckTarget(TargetableBase targetable, bool useFov = true)
        {
            if (targetable == null)
            {
                Debug.Log("Targetable is null");
                return false;
            }
            if (targetable.TargetPivot == null)
            {
                Debug.Log("TargetPivot is null");
                return false;
            }
            
            Vector3 position = _transform.position;
            Vector3 playerPosition = targetable.TargetPivot.position;
            
            Vector3 playerDirection = Vector3.ProjectOnPlane(playerPosition - position, Vector3.up);
        
            if (playerDirection.sqrMagnitude > _range * _range)
                return false;
            
            playerDirection.Normalize();
            Vector3 forward = Vector3.ProjectOnPlane(_transform.forward, Vector3.up).normalized;
            
            if (useFov)
            {
                if (Vector3.Dot(playerDirection, forward) < _cosine)
                    return false;
            }

            if (!Physics.Raycast(position, (playerPosition - position).normalized, out RaycastHit hit, _range))
            {
                Debug.Log("Raycast hit nothing");
                return false;
            }

            // if (!hit.collider.GetComponentInParent<PlayerController>())
            // {
            //     Debug.Log($"Raycast hit: {hit.collider.name}, but not part of player.");
            //     return false;
            // }
            
            // foreach (Collider collider in _playerColliders)
            // {
            //     if (hit.collider == collider)
            //         return true;
            // }

            for (int i = 0; i < _playerColliders.Length; i++)
            {
                if(hit.collider == _playerColliders[i])
                    return true;
            }
            
            return false;
        }

        private void HealthChangedHandler(int health)
        {
            LookAround();
        }
    }
}
