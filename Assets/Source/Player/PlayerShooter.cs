using System;
using System.Collections;
using DamageSystems;
using UnityEngine;

namespace Player
{
    public class PlayerShooter : ShooterBase
    {
        public event Action OnStartedCharge;
        public event Action OnStoppedCharge;
        
        private enum ShootingMode
        {
            RayCast,
            SphereCast,
            ObjectSpawn
        }

        [SerializeField] private ShootingMode _shootingMode;
        [SerializeField] private Transform _cameraTransform;

        [Header("Ray/Sphere Properties")]
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _rayMask;
        [SerializeField] private float _shotRadius;
        [SerializeField] private Vector3 _shotScale;

        [Header("Visual")]
        [SerializeField] private HitscanShotAspect _shotPrefab;
        [SerializeField] private float _shotVisualDiameter;
        [SerializeField] private string _tilingName;
        [SerializeField] private float _decaySpeed;
        
        [Header("Railgun Charge Settings")]
        [SerializeField] private float _maxChargeTime = 2f;
        [SerializeField] private float _chargeRate = 1f;

        private int _tilingId;
        private Vector3 _hitPoint;

        private bool _isCharging;
        private float _currentCharge;
        
        public float CurrentChargeNormalized => Mathf.Clamp01(_currentCharge / _maxChargeTime);
        
        protected void OnEnable()
        {
            _tilingId = Shader.PropertyToID(_tilingName);

            InputController.OnShootingModeChange += ShootingModeChangeHandler;
            InputController.OnShootInput += ShootInputHandler;
        }

        protected void OnDisable()
        {
            InputController.OnShootingModeChange -= ShootingModeChangeHandler;
            InputController.OnShootInput -= ShootInputHandler;
        }

        private void Update()
        {
            if (_isCharging)
            {
                ChargeWeapon();
            }
        }

        
        private void ShootingModeChangeHandler(bool performed)
        {
            if (performed)
            {
                _shootingMode = _shootingMode == ShootingMode.ObjectSpawn ? ShootingMode.RayCast : _shootingMode + 1;
            }
        }

        // private void ShootInputHandler(bool performed)
        // {
        //     if (!performed) return;
        //
        //     switch (_shootingMode)
        //     {
        //         case ShootingMode.RayCast: RaycastShoot(); break;
        //         case ShootingMode.SphereCast: SphereCastShoot(); break;
        //         case ShootingMode.ObjectSpawn: ObjectSpawnShoot(); break;
        //     }
        //
        //     InvokeShot();
        // }
        
        private void ShootInputHandler(bool performed)
        {
            Shot(performed);
        }

        public void Shot(bool performed)
        {
            switch (_shootingMode)
            {
                case ShootingMode.RayCast:
                case ShootingMode.SphereCast:
                    HandleChargeInput(performed);
                    break;
                case ShootingMode.ObjectSpawn:
                    if (performed)
                    {
                        Shoot();
                    }
                    break;
            }
        }
        
        private void HandleChargeInput(bool performed)
        {
            if (performed)
            {
                OnStartedCharge?.Invoke();
                _isCharging = true;
                
            }
            else
            {
                if (_currentCharge < _maxChargeTime)
                {
                    ResetCharge();
                }
            }
        }

        private void ChargeWeapon()
        {
            _currentCharge += _chargeRate * Time.deltaTime;

            if (_currentCharge >= _maxChargeTime)
            {
                Shoot();
                ResetCharge();
            }
        }

        private void ResetCharge()
        {
            OnStoppedCharge?.Invoke();
            
            _currentCharge = 0f;
            _isCharging = false;
        }
        
        private void Shoot()
        {
            switch (_shootingMode)
            {
                case ShootingMode.RayCast:
                    RaycastShoot();
                    InvokeShot("Blaster");
                    break;
                case ShootingMode.SphereCast:
                    SphereCastShoot();
                    InvokeShot("Blaster");
                    break;
                case ShootingMode.ObjectSpawn:
                    ObjectSpawnShoot();
                    InvokeShot("MachineGun");
                    break;
            }
        }

        
        private void RaycastShoot()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            _hitPoint = _cameraTransform.position + _cameraTransform.forward * _rayDistance;
            
            if (Physics.Raycast(ray, out var hitInfo, _rayDistance, _rayMask, QueryTriggerInteraction.Ignore))
            {
                _hitPoint = hitInfo.point;
                InvokeHit("Raycast", hitInfo.point, hitInfo.normal, hitInfo.collider);
                // InvokeHitPointHit(hitInfo.point, "Blaster");
            }
            
            DrawShot(_hitPoint);
        }

        private void SphereCastShoot()
        {
            Ray ray = new Ray(_gunTransform.position, _gunTransform.forward);
            _hitPoint = _gunTransform.position + _gunTransform.forward * _rayDistance;

            if (Physics.SphereCast(ray, _shotRadius, out var hitInfo, _rayDistance, _rayMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 direct = hitInfo.point - _gunTransform.position;
                Vector3 projected = Vector3.Project(direct, ray.direction);
                _hitPoint = _gunTransform.position + projected;
                InvokeHit("SphereCast", hitInfo.point, hitInfo.normal, hitInfo.collider);
                // InvokeHitPointHit(hitInfo.point, "Blaster");
            }
            
            DrawShot(_hitPoint);
        }

        private void DrawShot(Vector3 hitPoint)
        {
            HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, _gunTransform.rotation);
            shot.distance = (hitPoint - _gunTransform.position).magnitude;
            shot.outerPropertyBlock = new MaterialPropertyBlock();
            StartCoroutine(ShotRoutine(shot));
            Debug.DrawLine(_gunTransform.position, hitPoint, Color.green, 1f);
        }

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
    }
}
