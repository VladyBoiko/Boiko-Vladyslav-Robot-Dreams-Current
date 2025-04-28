using System;
using Attributes;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputController : MonoServiceBase
    {
        public static event Action<Vector2> OnMoveInput;
        public static event Action<bool> OnJumpInput;
        public static event Action<Vector2> OnCameraMoveInput;
        public static event Action<bool> OnCameraLockInput;
        public static event Action<bool> OnCameraChangeInput;
        public static event Action<bool> OnExplosionInput;
        public static event Action<bool> OnShootInput;
        public static event Action<bool> OnCameraZoomInput;
        public static event Action<bool> OnShootingModeChange;
        public static event Action<bool> OnScoreInput;
        public event Action<bool> OnEscapeInput;
        public event Action OnInteract;
        public event Action OnInventory;
        public event Action OnCloseInteractable;

        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField, ActionMapDropdown] private string _mapName;
        [SerializeField, ActionMapDropdown] private string _UIMapName;
    
        [SerializeField, ActionInputDropdown] private string _moveName;
        [SerializeField, ActionInputDropdown] private string _jumpName;
        [SerializeField, ActionInputDropdown] private string _cameraMoveName;
        [SerializeField, ActionInputDropdown] private string _cameraLockName;
        [SerializeField, ActionInputDropdown] private string _cameraChangeName;
        [SerializeField, ActionInputDropdown] private string _explosionName;
        [SerializeField, ActionInputDropdown] private string _shootName;
        [SerializeField, ActionInputDropdown] private string _zoomName;
        [SerializeField, ActionInputDropdown] private string _shootingModeChangeName;
        [SerializeField, ActionInputDropdown] private string _scoreName;
        [SerializeField, ActionInputDropdown] private string _escapeName;
        [SerializeField, ActionInputDropdown] private string _interactName;
        [SerializeField, ActionInputDropdown] private string _inventoryName;
        [SerializeField, ActionInputDropdown] private string _closeInteractableName;
   
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _cameraMoveAction;
        private InputAction _cameraLockAction;
        private InputAction _cameraChangeAction;
        private InputAction _explosionAction;
        private InputAction _shootAction;
        private InputAction _cameraZoomAction;
        private InputAction _shootingModeChangeAction;
        private InputAction _scoreAction;
        private InputAction _escapeAction;
        private InputAction _interactAction;
        private InputAction _inventoryAction;
        private InputAction _closeInteractableAction;

        private bool _inputUpdated;

        private InputActionMap _actionMap;
        private InputActionMap _UIActionMap;

        public override Type Type { get; } = typeof(InputController);
        
        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        
            _inputActionAsset.Enable();
        
            _actionMap = _inputActionAsset?.FindActionMap(_mapName);
            _UIActionMap = _inputActionAsset?.FindActionMap(_UIMapName);
        
            _moveAction = _actionMap?.FindAction(_moveName);
            _jumpAction = _actionMap?.FindAction(_jumpName);
            _cameraMoveAction = _actionMap?.FindAction(_cameraMoveName);
            _cameraLockAction = _actionMap?.FindAction(_cameraLockName);
            _cameraChangeAction = _actionMap?.FindAction(_cameraChangeName);
            _explosionAction = _actionMap?.FindAction(_explosionName);
            _shootAction = _actionMap?.FindAction(_shootName);
            _cameraZoomAction = _actionMap?.FindAction(_zoomName);
            _shootingModeChangeAction = _actionMap?.FindAction(_shootingModeChangeName);
            _scoreAction = _actionMap?.FindAction(_scoreName);
            _interactAction = _actionMap?.FindAction(_interactName);
            _inventoryAction = _UIActionMap?.FindAction(_inventoryName);
            _escapeAction = _UIActionMap?.FindAction(_escapeName);
            _closeInteractableAction = _UIActionMap?.FindAction(_closeInteractableName);
            
            if (_inputActionAsset)
            {
                _moveAction.performed += MovePerformedHandler;
                _moveAction.canceled += MoveCanceledHandler;

                _cameraMoveAction.performed += CameraMovePerformedHandler;

                _jumpAction.performed += JumpPerformedHandler;
                _jumpAction.canceled += JumpCanceledHandler;

                _cameraLockAction.performed += CameraLockPerformedHandler;
                _cameraLockAction.canceled += CameraLockCanceledHandler;

                _cameraChangeAction.performed += CameraChangePerformedHandler;
                _cameraChangeAction.canceled += CameraChangeCanceledHandler;

                _explosionAction.performed += ExplosionPerformedHandler;
                _explosionAction.canceled += ExplosionCanceledHandler;

                _shootAction.performed += ShootPerformedHandler;
                _shootAction.canceled += ShootCanceledHandler;

                _cameraZoomAction.performed += CameraZoomPerformedHandler;
                _cameraZoomAction.canceled += CameraZoomCanceledHandler;

                _shootingModeChangeAction.performed += ShootingModeChangePerformedHandler;
                _shootingModeChangeAction.canceled += ShootingModeChangeCanceledHandler;

                _scoreAction.performed += ScorePerformedHandler;
                _scoreAction.canceled += ScoreCanceledHandler;

                _escapeAction.performed += EscapePerformedHandler;

                _interactAction.performed += InteractPerformedHandler;

                _inventoryAction.performed += InventoryPerformedHandler;

                _closeInteractableAction.performed += CloseInteractableHandler;
            }
            else
            {
                Debug.LogError("Input action asset is missing.");
            }
        }
    

        private void OnDisable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        
            _actionMap.Disable();
            // _UIActionMap.Disable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _moveAction.performed -= MovePerformedHandler;
            _moveAction.canceled -= MoveCanceledHandler;

            _cameraMoveAction.performed -= CameraMovePerformedHandler;

            _jumpAction.performed -= JumpPerformedHandler;
            _jumpAction.canceled -= JumpCanceledHandler;

            _cameraLockAction.performed -= CameraLockPerformedHandler;
            _cameraLockAction.canceled -= CameraLockCanceledHandler;

            _cameraChangeAction.performed -= CameraChangePerformedHandler;
            _cameraChangeAction.canceled -= CameraChangeCanceledHandler;

            _explosionAction.performed -= ExplosionPerformedHandler;
            _explosionAction.canceled -= ExplosionCanceledHandler;

            _shootAction.performed -= ShootPerformedHandler;
            _shootAction.canceled -= ShootCanceledHandler;

            _cameraZoomAction.performed -= CameraZoomPerformedHandler;
            _cameraZoomAction.canceled -= CameraZoomCanceledHandler;

            _shootingModeChangeAction.performed -= ShootingModeChangePerformedHandler;
            _shootingModeChangeAction.canceled -= ShootingModeChangeCanceledHandler;

            _scoreAction.performed -= ScorePerformedHandler;
            _scoreAction.canceled -= ScoreCanceledHandler;

            _escapeAction.performed -= EscapePerformedHandler;

            _interactAction.performed -= InteractPerformedHandler;
            
            _inventoryAction.performed -= InventoryPerformedHandler;

            _closeInteractableAction.performed -= CloseInteractableHandler;
        
            OnMoveInput = null;
            OnJumpInput = null;
            OnCameraMoveInput = null;
            OnCameraLockInput = null;
            OnCameraChangeInput = null;
            OnExplosionInput = null;
            OnShootInput = null;
            OnCameraZoomInput = null;
            OnShootingModeChange = null;
            OnScoreInput = null;
            OnEscapeInput = null;
            OnInteract = null;
            OnInventory = null;
            OnCloseInteractable = null;
        }
        
        
        private void MovePerformedHandler(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }
        private void MoveCanceledHandler(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }
    
        private void JumpPerformedHandler(InputAction.CallbackContext context)
        {
            OnJumpInput?.Invoke(true);
            // OnJumpInput?.Invoke(context.ReadValueAsButton());
        }
        private void JumpCanceledHandler(InputAction.CallbackContext context)
        {
            OnJumpInput?.Invoke(false);
            // OnJumpInput?.Invoke(context.ReadValueAsButton());
        }
    
        private void CameraMovePerformedHandler(InputAction.CallbackContext context)
        {
            OnCameraMoveInput?.Invoke(context.ReadValue<Vector2>());
        }
    
        private void CameraLockPerformedHandler(InputAction.CallbackContext context)
        {
            OnCameraLockInput?.Invoke(true);
        }
        private void CameraLockCanceledHandler(InputAction.CallbackContext context)
        {
            OnCameraLockInput?.Invoke(false);
        }
    
        private void CameraChangePerformedHandler(InputAction.CallbackContext context)
        {
            OnCameraChangeInput?.Invoke(true);
        }
        private void CameraChangeCanceledHandler(InputAction.CallbackContext context)
        {
            OnCameraChangeInput?.Invoke(false);
        }

        private void ExplosionPerformedHandler(InputAction.CallbackContext context)
        {
            OnExplosionInput?.Invoke(true);
        }
        private void ExplosionCanceledHandler(InputAction.CallbackContext context)
        {
            OnExplosionInput?.Invoke(false);
        }

        private void ShootPerformedHandler(InputAction.CallbackContext context)
        {
            OnShootInput?.Invoke(true);
        }
        private void ShootCanceledHandler(InputAction.CallbackContext context)
        {
            OnShootInput?.Invoke(false);
        }

        private void CameraZoomPerformedHandler(InputAction.CallbackContext context)
        {
            OnCameraZoomInput?.Invoke(true);
        }
        private void CameraZoomCanceledHandler(InputAction.CallbackContext context)
        {
            OnCameraZoomInput?.Invoke(false);
        }

        private void ShootingModeChangePerformedHandler(InputAction.CallbackContext context)
        {
            OnShootingModeChange?.Invoke(true);
        }
        private void ShootingModeChangeCanceledHandler(InputAction.CallbackContext context)
        {
            OnShootingModeChange?.Invoke(false);
        }

        private void ScorePerformedHandler(InputAction.CallbackContext context)
        {
            OnScoreInput?.Invoke(true);
        }
        private void ScoreCanceledHandler(InputAction.CallbackContext context)
        {
            OnScoreInput?.Invoke(false);
        }

        private void EscapePerformedHandler(InputAction.CallbackContext context)
        {
            OnEscapeInput?.Invoke(true);
        }

        private void InteractPerformedHandler(InputAction.CallbackContext context)
        {
            OnInteract?.Invoke();
        }

        private void InventoryPerformedHandler(InputAction.CallbackContext context)
        {
            OnInventory?.Invoke();
        }

        private void CloseInteractableHandler(InputAction.CallbackContext context)
        {
            OnCloseInteractable?.Invoke();
        }
    }
}
