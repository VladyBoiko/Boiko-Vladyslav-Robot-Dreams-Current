using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraYawAnchor;
    [SerializeField] private Transform _cameraPitchAnchor;
    [SerializeField] private float _sensitivity;
    [SerializeField] private Transform _blaster;
    [SerializeField] private CinemachineMixingCamera _mixingCamera;
    [SerializeField] private float _aimSpeed;

    private bool _isFirstPerson;
    private float _aimValue;
    private float _targetAimValue;
    private float _cameraSwitchValue;

    private float _yaw;
    private float _pitch = 30f;

    private void Start()
    {
        _sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        
        InputController.OnCameraMoveInput += CameraMoveHandler;
        InputController.OnCameraChangeInput += CameraChangeHandler;
        InputController.OnCameraZoomInput += CameraZoomInputHandler;

        KeyboardMouseSettings.OnSettingsChanged += SettingsChangedHandler;
    }

    private void Update()
    {
        _aimValue = Mathf.MoveTowards(_aimValue, _targetAimValue, _aimSpeed * Time.deltaTime);
        
        float targetCameraValue = _isFirstPerson ? 1f : 0f;
        _cameraSwitchValue = Mathf.MoveTowards(_cameraSwitchValue, targetCameraValue, _aimSpeed * Time.deltaTime);
        
        _mixingCamera.m_Weight0 = (1f - _aimValue) * (1f - _cameraSwitchValue);
        _mixingCamera.m_Weight1 = _aimValue * (1f - _cameraSwitchValue);
        _mixingCamera.m_Weight2 = _cameraSwitchValue;
    }

    private void LateUpdate()
    {
        _cameraPitchAnchor.localRotation = Quaternion.Euler(Mathf.Clamp(_pitch, -15f, 90f), 0f, 0f);
        _cameraYawAnchor.rotation = Quaternion.Euler(0f, _yaw, 0f);

        _blaster.localRotation = _cameraPitchAnchor.localRotation;
    }

    private void CameraMoveHandler(Vector2 cameraMoveInput)
    {
        cameraMoveInput *= _sensitivity;
        _pitch -= cameraMoveInput.y;
        _yaw += cameraMoveInput.x;
    }

    private void CameraChangeHandler(bool performed)
    {
        if (performed)
        {
            _isFirstPerson = !_isFirstPerson;
        }
    }

    private void CameraZoomInputHandler(bool performed)
    {
        _targetAimValue = performed ? 1f : 0f;
    }

    private void SettingsChangedHandler()
    {
        _sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
    }
    
    public void SetYawAnchor(Transform cameraYawAnchor)
    {
        _cameraYawAnchor.rotation = cameraYawAnchor.rotation = Quaternion.Euler(0f, _yaw, 0f);
        _cameraYawAnchor = cameraYawAnchor;
    }
}
