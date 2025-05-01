using System;
using HealthSystems;
using UnityEngine;

namespace DamageSystems
{
    public class DamageDealer : MonoBehaviour
    {
        public event Action<int, int> OnHit;
    
        [SerializeField] private Shooter _shooter;
        [SerializeField] private int _damage;
        [SerializeField] private HealthSystem _healthSystem;
    
        [SerializeField] private DamagePopup _damagePopupPrefab;
        [SerializeField] private Camera _camera;

        public Shooter Shooter => _shooter;
    
        private void OnEnable()
        {
            if (_shooter == null || _healthSystem == null)
            {
                Debug.LogError($"{name} is missing references to Shooter or HealthSystem.");
                enabled = false;
                return;
            }
        
            _shooter.OnHit += HitInputHandler;
        }
    
        private void OnDisable()
        {
            _shooter.OnHit -= HitInputHandler;
        }
    
        private void HitInputHandler(string shootingModeName, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
        {
            int damage = _damage;
            int score = 0;
        
            if (_healthSystem.GetHealth(hitCollider, out HealthArea health))
            {
                if (health.isCritical)
                {
                    damage = (int)(damage * health.damageMultiplier);
                    score = 2;
                    // Debug.Log($"Critical Damage: {hitCollider}!");
                }
                else
                {
                    score = 1;
                }
                health.health.TakeDamage(damage);
            
                var popup = Instantiate(_damagePopupPrefab, hitPoint, Quaternion.identity);
                popup.Initialize(damage, _camera);
            
                // Debug.Log($"{shootingModeName} hit: {hitCollider.name} at {hitPoint}.");
                // OnHit?.Invoke(health.health ? 1 : 0, score);
                OnHit?.Invoke(1, score);
            }
        }
    }
}
