using System;
using UnityEngine;

namespace DamageSystems
{
    public abstract class ShooterBase : MonoBehaviour
    {
        public event Action<string, Vector3, Vector3, Collider> OnHit;
        public event Action<Vector3, string> OnHitPointHit;
        public event Action<string> OnShot;

        [SerializeField] protected Transform _gunTransform;

        [Header("ObjectSpawnShoot Properties")]
        [SerializeField] protected Bullet _projectilePrefab;
        [SerializeField] protected float _projectileSpeed;
        [SerializeField] protected float _projectileLifetime;

        public void ObjectSpawnShoot()
        {
            if (_projectilePrefab == null) return;

            Bullet bullet = Instantiate(_projectilePrefab, _gunTransform.position, _gunTransform.rotation);
            bullet.Initialize(_projectileSpeed, _projectileLifetime);
            
            bullet.OnStaticHit += HandleBulletHit;
            bullet.OnDestroyed += HandleBulletDestroyed;
        }

        private void HandleBulletHit(string shootingModeName, Collision collision)
        {
            OnHit?.Invoke(shootingModeName, collision.contacts[0].point, collision.contacts[0].normal, collision.collider);
            OnHitPointHit?.Invoke(collision.transform.position, "MachineGun");
        }
        
        private void HandleBulletDestroyed(Bullet bullet)
        {
            bullet.OnStaticHit -= HandleBulletHit;
            bullet.OnDestroyed -= HandleBulletDestroyed;
        }

        protected void InvokeShot(string gunName) => OnShot?.Invoke(gunName);

        // protected void InvokeHitPointHit(Vector3 hitPoint, string gunName)
        // {
        //     OnHitPointHit?.Invoke(hitPoint, gunName);
        // }

        protected void InvokeHit(string mode, Vector3 point, Vector3 normal, Collider col)
        {
            OnHit?.Invoke(mode, point, normal, col);
        }
    }
}