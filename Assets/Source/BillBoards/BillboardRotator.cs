using UnityEngine;

namespace BillBoards
{
    public class BillboardRotator : BillboardBase
    {
        private Camera _camera;
        private Transform _transform;
        
        public override void SetCamera(Camera camera)
        {
            _transform = transform;
            _camera = camera;
        }

        private void LateUpdate()
        {
            Vector3 direction = (_camera.transform.position - _transform.position).normalized;
            _transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}

// using UnityEngine;
//
// public class BillboardRotator : MonoBehaviour
// {
//     [SerializeField] private Camera _camera;
//
//     private Transform _transform;
//     private Transform _cameraTransform;
//
//     private void Awake()
//     {
//         _transform = transform;
//
//         if (_camera == null)
//         {
//             Debug.LogError("No camera assigned");
//             enabled = false;
//             return;
//         }
//
//         if (_camera != null)
//             _cameraTransform = _camera.transform;
//         else
//             Debug.LogError($"No camera found of object {gameObject.name}");
//     }
//
//     private void LateUpdate()
//     {
//         if (!_cameraTransform) return;
//         
//         Vector3 direction = (_cameraTransform.position - _transform.position).normalized;
//         _transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
//     }
// }