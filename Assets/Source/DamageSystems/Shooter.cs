using System;
using System.Collections;
using Player;
using UnityEngine;

namespace DamageSystems
{
    public class Shooter : MonoBehaviour
    {
        private enum ShootingMode
        {
            RayCast,
            SphereCast,
            ObjectSpawn
        }
    
        public event Action<string, Vector3, Vector3, Collider> OnHit;
        public event Action OnShot;
    
        [Header("General Properties")]
        [SerializeField] private ShootingMode _shootingMode;
        [SerializeField] private Transform _gunTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private bool _breakOnSpawn;
    
        [Header("Ray Properties")]
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _rayMask;
    
        [Header("Sphere Properties")]
        [SerializeField] protected float _shotRadius;
        [SerializeField] protected Vector3 _shotScale;
    
        [Header("ShotVisual Properties")]
        [SerializeField] protected HitscanShotAspect _shotPrefab;
        [SerializeField] protected float _shotVisualDiameter;
        [SerializeField] protected string _tilingName;
        [SerializeField] protected float _decaySpeed;
    
        [Header("ObjectSpawnShoot Properties")]
        [SerializeField] private Bullet _projectilePrefab;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileLifetime;
    
        private int _tilingId;
        private Vector3 _hitPoint;
    
        protected virtual void Start()
        {
            _tilingId = Shader.PropertyToID(_tilingName);
        }
    
        private void OnEnable()
        {
            InputController.OnShootingModeChange += ShootingModeChangeHandler;
            InputController.OnShootInput += ShootInputHandler;
            // Bullet.OnStaticHit += HandleBulletHit;
        }

        private void OnDisable()
        {
            InputController.OnShootingModeChange-= ShootingModeChangeHandler;
            InputController.OnShootInput -= ShootInputHandler;
            // Bullet.OnStaticHit -= HandleBulletHit;
        }

        private void ShootingModeChangeHandler(bool performed)
        {
            if (performed)
            {
                if (_shootingMode == ShootingMode.ObjectSpawn)
                {
                    _shootingMode = ShootingMode.RayCast;
                    return;
                }
                _shootingMode ++;
            }
        }
    
        public void ShootInputHandler(bool performed)
        {
            if (performed)
            {
                switch (_shootingMode)
                {
                    case ShootingMode.RayCast:
                        RaycastShoot();
                        break;
                    case ShootingMode.SphereCast:
                        SphereCastShoot();
                        break;
                    case ShootingMode.ObjectSpawn:
                        ObjectSpawnShoot();
                        break;
                }
            
                OnShot?.Invoke();
            
                if (_breakOnSpawn)
                    Debug.Break();
            }
        }

        private void RaycastShoot()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            _hitPoint = _cameraTransform.position + _cameraTransform.forward * _rayDistance;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _rayMask, QueryTriggerInteraction.Ignore))
            {
                _hitPoint = hitInfo.point;
                OnHit?.Invoke("Raycast", hitInfo.point, hitInfo.normal, hitInfo.collider);
            }
            DrawShot(_hitPoint);
        }

        private void SphereCastShoot()
        {
            Vector3 gunTransformPosition = _gunTransform.position;
            Vector3 gunTransformForward = _gunTransform.forward;
            Ray ray = new Ray(gunTransformPosition, gunTransformForward);
            _hitPoint = gunTransformPosition + gunTransformForward * _rayDistance;
            if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _rayDistance, _rayMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 directVector = hitInfo.point - gunTransformPosition;
                Vector3 rayVector = Vector3.Project(directVector, ray.direction);
                _hitPoint = gunTransformPosition + rayVector;
            
                OnHit?.Invoke("SphereCast", hitInfo.point, hitInfo.normal, hitInfo.collider);
            }
            DrawShot(_hitPoint);
        }
    
        // private void ObjectSpawnShoot()
        // {
        //     if (_projectilePrefab == null) return;
        //     Bullet bullet = new Bullet(_projectilePrefab, _gunTransform, _projectileSpeed, _projectileLifetime);
        // }
    
        private void ObjectSpawnShoot()
        {
            if (_projectilePrefab == null) return;

            Bullet bullet = Instantiate(_projectilePrefab, _gunTransform.position, _gunTransform.rotation);

            bullet.Initialize(_projectileSpeed, _projectileLifetime);
        }

        // private void HandleBulletHit(string shootingModeName, Collision collision)
        // {
        //     OnHit?.Invoke(shootingModeName, collision.contacts[0].point, collision.contacts[0].normal, collision.collider);
        // }
    
        private IEnumerator ShotRoutine(HitscanShotAspect shot)
        {
            float interval = _decaySpeed * Time.deltaTime;
            while (shot.distance >= interval)
            {
                EvaluateShot(shot);
                yield return null;
                shot.distance -= interval;
                interval = _decaySpeed * Time.deltaTime;
            }

            Destroy(shot.gameObject);
        }

        private void EvaluateShot(HitscanShotAspect shot)
        {
            shot.Transform.localScale = new Vector3(_shotScale.x, _shotScale.y, shot.distance * 0.5f);
            Vector4 tiling = Vector4.one;
            tiling.y = shot.distance * 0.5f / _shotVisualDiameter;
            shot.outerPropertyBlock.SetVector(_tilingId, tiling);
            shot.Outer.SetPropertyBlock(shot.outerPropertyBlock);
        }

        private void DrawShot(Vector3 hitPoint)
        {
            HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, _gunTransform.rotation);
            shot.distance = (hitPoint - _gunTransform.position).magnitude;
            shot.outerPropertyBlock = new MaterialPropertyBlock();
            StartCoroutine(ShotRoutine(shot));
            Debug.DrawLine(_gunTransform.position, _hitPoint, Color.green, 1f);
        }
    }
}
