using System.Collections.Generic;
using Player;
using UnityEngine;

namespace DamageSystems
{
    public class ExplosionController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _rayMask;
        [SerializeField] private LayerMask _explsionMask;
    
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _verticalOffset;
    
        [SerializeField] private GameObject _explosionVisualization;
    
        private float _radiusReciprocal;
    
        private void Start()
        {
            InputController.OnExplosionInput += ExplosionHandler;
        }

        private void OnEnable()
        {
            _radiusReciprocal = 1f / _explosionRadius;
        }
    
        private void ExplosionHandler(bool performed)
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            Vector3 _hitPoint = _cameraTransform.position + _cameraTransform.forward * _rayDistance;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _rayMask, QueryTriggerInteraction.Ignore)
                && performed)
            {
                _hitPoint = hitInfo.point;

                if (_explosionVisualization != null)
                {
                    GameObject explosionEffect = Instantiate(_explosionVisualization, _hitPoint, Quaternion.identity);
                    explosionEffect.transform.localScale = Vector3.one * _explosionRadius;
                    Destroy(explosionEffect, 1.0f);
                }
            
                Collider[] colliders = Physics.OverlapSphere(_hitPoint, _explosionRadius, _explsionMask);
            
                HashSet<Rigidbody> _targets = new HashSet<Rigidbody>();
                
                for (int i = 0; i < colliders.Length; ++i)
                {
                    Rigidbody rigidbody = colliders[i].attachedRigidbody;
                    _ = _targets.Add(rigidbody);
                }
        
                foreach (Rigidbody rigidbody in _targets)
                {
                    if (rigidbody == null)
                        continue;
                    Vector3 direction = rigidbody.position - (_hitPoint + Vector3.up * _verticalOffset);
                    rigidbody.AddForce(
                        direction.normalized * _explosionForce * Mathf.Clamp01(1f - direction.magnitude * _radiusReciprocal),
                        ForceMode.Impulse);
                }
            }
        }
    }
}
