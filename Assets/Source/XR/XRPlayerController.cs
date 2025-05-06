using System;
using Enemy;
using HealthSystems;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XR
{
    public class XRPlayerController : MonoServiceBase
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _characterTransform;
        [SerializeField] private InputAction _lookAround;
        [SerializeField] private InputAction _cameraMove;
        [SerializeField] private InputAction _break;
        
        public override Type Type { get; } = typeof(XRPlayerController);
        
        public Camera Camera => _camera;
        
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private InputAction _moveAction;
        [SerializeField] private InputAction _lookAction;
        
        [SerializeField] private float _sensetivity;
        [SerializeField] private float _speed;
        
        [SerializeField] private TargetableBase _targetable;
        [SerializeField] private Health _health;
        
        public CharacterController CharacterController => _characterController;
        public TargetableBase Targetable => _targetable;
        public Health Health => _health;
        
        private float _yaw;
        
        private void Start()
        {
            _lookAround.Enable();
            _cameraMove.Enable();
            _break.Enable();
            
            _moveAction.Enable();
            _lookAction.Enable();
            
            _yaw = _characterTransform.eulerAngles.x;

            _break.performed += BreakHandler;
        }

        private void FixedUpdate()
        {
            Vector2 move = _moveAction.ReadValue<Vector2>();
            Vector2 look = _lookAction.ReadValue<Vector2>();
            
            _yaw += look.x * _sensetivity;
            
            _characterTransform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            
            Vector3 movement = (_characterTransform.forward * move.y + _characterController.transform.right * move.x) *
                               (_speed * Time.fixedDeltaTime);
            
            movement += Physics.gravity * Time.fixedDeltaTime;

            _characterController.Move(movement);
        }

        private void LateUpdate()
        {
            Quaternion cameraRotationOffset = _lookAround.ReadValue<Quaternion>();
            Vector3 cameraPosition = _cameraMove.ReadValue<Vector3>();
            _camera.transform.SetLocalPositionAndRotation(cameraPosition, cameraRotationOffset);
            // _camera.transform.rotation = cameraRotationOffset;
            // _camera.transform.position = cameraPosition;
        }

        private void BreakHandler(InputAction.CallbackContext context)
        {
            Debug.Break();
        }
    }
}