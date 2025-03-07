using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
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

    [SerializeField] private InputActionAsset _inputActionAsset;
    
    [SerializeField] private string _mapName;
    
    [SerializeField] private string _moveName;
    [SerializeField] private string _jumpName;
    [SerializeField] private string _cameraMoveName;
    [SerializeField] private string _cameraLockName;
    [SerializeField] private string _cameraChangeName;
    [SerializeField] private string _explosionName;
    [SerializeField] private string _shootName;
    [SerializeField] private string _zoomName;
    [SerializeField] private string _shootingModeChangeName;
   
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _cameraMoveAction;
    private InputAction _cameraLockAction;
    private InputAction _cameraChangeAction;
    private InputAction _explosionAction;
    private InputAction _shootAction;
    private InputAction _cameraZoomAction;
    private InputAction _shootingModeChangeAction;

    private bool _inputUpdated;

    private void Awake()
    {
        _inputActionAsset?.Enable();
        InputActionMap actionMap = _inputActionAsset?.FindActionMap(_mapName) 
                                   ?? _inputActionAsset?.FindActionMap("Default");
        
        _moveAction = actionMap?.FindAction(_moveName) 
                      ?? actionMap?.FindAction("Move");
        _jumpAction = actionMap?.FindAction(_jumpName) 
                      ?? actionMap?.FindAction("Jump");
        _cameraMoveAction = actionMap?.FindAction(_cameraMoveName) 
                            ?? actionMap?.FindAction("CameraMove");
        _cameraLockAction = actionMap?.FindAction(_cameraLockName)
                            ?? actionMap?.FindAction("CameraLock");
        _cameraChangeAction = actionMap?.FindAction(_cameraChangeName)
                            ?? actionMap?.FindAction("CameraChange");
        _explosionAction = actionMap?.FindAction(_explosionName)
                           ?? actionMap?.FindAction("Explosion");
        _shootAction = actionMap?.FindAction(_shootName)
                       ?? actionMap?.FindAction("Shoot");
        _cameraZoomAction = actionMap?.FindAction(_zoomName)
                        ?? actionMap?.FindAction("CameraZoom");
        _shootingModeChangeAction = actionMap?.FindAction(_shootingModeChangeName)
                                    ?? actionMap?.FindAction("ShootingModeChange");
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
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
        
        _inputActionAsset?.Disable();
    }

    private void OnDestroy()
    {
        OnMoveInput = null;
        OnCameraMoveInput = null;
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
}
