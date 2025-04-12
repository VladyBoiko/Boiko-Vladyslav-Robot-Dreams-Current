using Enemy;
using HealthSystems;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private TargetableBase _targetable;
        [SerializeField] private Health _health;
        [SerializeField] private float _speed;
    
        private Transform _transform;
        private Vector2 _moveInput;
        
        public CharacterController CharacterController => _controller;
        public TargetableBase Targetable => _targetable;
        public Health Health => _health;

        private void Start()
        {
            InputController.OnMoveInput += MoveHandler;
            _transform = transform;
            if (_controller) return;
            Debug.LogError("No CharacterController attached");
            enabled = false;
        }

        private void FixedUpdate()
        {
            Vector3 forward = _transform.forward;
            Vector3 right = _transform.right;
            Vector3 movement = forward * _moveInput.y + right * _moveInput.x;
            _controller.SimpleMove(movement * _speed);
        }

        private void MoveHandler(Vector2 moveInput)
        {
            // Debug.Log($"Move Input: {moveInput}");
            _moveInput = moveInput.normalized;
        }
    }
}
