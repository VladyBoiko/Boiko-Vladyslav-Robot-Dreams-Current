using UnityEngine;

public class CameraLocker : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Transform _regularYawAnchor;
    [SerializeField] private Transform _lockedYawAnchor;

    private void Start()
    {
        InputController.OnCameraLockInput += CameraLockHandler;
    }

    private void CameraLockHandler(bool cameraLocked)
    {
        _cameraController.SetYawAnchor(cameraLocked ? _lockedYawAnchor : _regularYawAnchor);
    }
}
