using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraYawAnchor;
    [SerializeField] private Transform _cameraPitchAnchor;
    
    [SerializeField] private float _sensitivity;

    [SerializeField] private Transform _blaster;
    
    private float _yaw = 0f;
    private float _pitch = 30f;
    
    private Vector2 _cameraMoveInput;

    private void Start()
    {
        InputController.OnCameraMoveInput += CameraMoveHandler;
        InputController.OnCameraChange += CameraChangeHandler;
    }

    private void LateUpdate()
    {
        _cameraPitchAnchor.localRotation = Quaternion.Euler(Mathf.Clamp(_pitch, -15f, 90f), 0f, 0f);
        _cameraYawAnchor.rotation = Quaternion.Euler(0f, _yaw, 0f);

        _blaster.localRotation = _cameraPitchAnchor.localRotation * Quaternion.Euler(0f, 90f, 0f);
    }
    
    private void CameraMoveHandler(Vector2 cameraMoveInput)
    {
        cameraMoveInput *= _sensitivity;
        _pitch -= cameraMoveInput.y;
        _yaw += cameraMoveInput.x;
    }

    private void CameraChangeHandler(bool isFirstView)
    {
        /*TODO: Write method for CameraChangeHandler*/
        return;
    }
    
    public void SetYawAnchor(Transform cameraYawAnchor)
    {
        _cameraYawAnchor.rotation = cameraYawAnchor.rotation = Quaternion.Euler(0f, _yaw, 0f);
        _cameraYawAnchor = cameraYawAnchor;
    }
}
