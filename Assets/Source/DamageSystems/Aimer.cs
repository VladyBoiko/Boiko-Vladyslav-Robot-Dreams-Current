using UnityEngine;

namespace DamageSystems
{
    public class Aimer : MonoBehaviour
    {
        [SerializeField] private Transform _gunTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _rayMask;
    
        private Vector3 _hitPoint;
        
        public Vector3 HitPoint => _hitPoint;
    
        private void FixedUpdate()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            _hitPoint = _cameraTransform.position + _cameraTransform.forward * _rayDistance;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _rayMask, QueryTriggerInteraction.Ignore))
                _hitPoint = hitInfo.point;
            _gunTransform.LookAt(_hitPoint);
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_gunTransform.position, _hitPoint);
        }
    }
}
