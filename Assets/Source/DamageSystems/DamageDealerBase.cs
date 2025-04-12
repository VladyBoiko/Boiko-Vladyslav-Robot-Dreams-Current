using System;
using Enemy;
using Enemy.BehaviourTreeSystem.EnemyBehaviour;
using HealthSystems;
using UnityEngine;

namespace DamageSystems
{
    public abstract class DamageDealerBase : MonoBehaviour
    {
        public event Action<int, int> OnHit;

        // [SerializeField] protected Shooter _shooter;
        [SerializeField] protected ShooterBase _shooter;
        [SerializeField] protected int _damage;
        [SerializeField] protected HealthSystem _healthSystem;

        public ShooterBase Shooter => _shooter;

        protected virtual void OnEnable()
        {
            if (_shooter == null || _healthSystem == null)
            {
                Debug.LogError($"{name} is missing references to Shooter or HealthSystem.");
                enabled = false;
                return;
            }

            _shooter.OnHit += HitInputHandler;
            MeleeAttackBehaviour.OnMeleeAttackHit += HitInputHandler;
        }

        protected virtual void OnDisable()
        {
            _shooter.OnHit -= HitInputHandler;
            MeleeAttackBehaviour.OnMeleeAttackHit -= HitInputHandler;
        }

        private void HitInputHandler(string mode, Vector3 point, Vector3 normal, Collider collider)
        {
            if (_healthSystem.GetHealth(collider, out HealthArea health) && 
                !(collider.transform.root == _shooter.transform.root))
            {
                Debug.Log($"Collision with: {collider.gameObject.name}");
                
                HandleDamage(health, point, collider);
            }
        }
        
        protected void InvokeHit(int damage, int score)
        {
            OnHit?.Invoke(damage, score);
        }
        
        // protected virtual void HandleDamage(HealthArea health, Vector3 hitPoint, Collider hitCollider)
        // {
        //     int damage = _damage;
        //     int score = 0;
        //
        //     if (health.isCritical)
        //     {
        //         damage = (int)(damage * health.damageMultiplier);
        //         score = 2;
        //         Debug.Log($"Critical Damage: {hitCollider}!");
        //     }
        //     else
        //     {
        //         score = 1;
        //     }
        //
        //     health.health.TakeDamage(damage);
        //     OnHit?.Invoke(1, score);
        // }

        protected abstract void HandleDamage(HealthArea health, Vector3 hitPoint, Collider hitCollider);
    }
}