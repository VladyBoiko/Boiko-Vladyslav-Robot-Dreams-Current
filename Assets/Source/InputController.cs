using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static event Action<Vector2> OnMoveInput;
    public static event Action<bool> OnJumpInput;
    public static event Action<Vector2> OnCameraMoveInput;
    public static event Action<bool> OnCameraLock;
    public static event Action<bool> OnCameraChange;

    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private string _mapName;
    [SerializeField] private string _moveName;
    [SerializeField] private string _jumpName;
    [SerializeField] private string _cameraMoveName;
    [SerializeField] private string _cameraLockName;
    [SerializeField] private string _cameraChangeName;
   
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _cameraMoveAction;
    private InputAction _cameraLockAction;
    private InputAction _cameraChangeAction;

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
    }

    private void OnEnable()
    {
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
        }
        else
        {
            Debug.LogError("Input action asset is missing.");
        }
    }
    

    private void OnDisable()
    {
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
    
    private void CameraLockPerformedHandler(InputAction.CallbackContext obj)
    {
        OnCameraLock?.Invoke(true);
    }
    private void CameraLockCanceledHandler(InputAction.CallbackContext obj)
    {
        OnCameraLock?.Invoke(false);
    }
    
    private void CameraChangePerformedHandler(InputAction.CallbackContext obj)
    {
        OnCameraChange?.Invoke(true);
    }
    
    private void CameraChangeCanceledHandler(InputAction.CallbackContext obj)
    {
        OnCameraChange?.Invoke(false);
    }
    
}
