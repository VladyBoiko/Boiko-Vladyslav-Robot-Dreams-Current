using System;
using DamageSystems;
using UnityEngine;

namespace HealthSystems
{
    public class Health : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;
        public event Action<float> OnHealthChanged01;
        public event Action OnDeath;
        public event Action<float> OnTakeDamage;
    
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected Collider[] _colliders;
        [SerializeField] protected WeakpointData[] _weakpoints;

        private int _health;
        private bool _isAlive;
        
        public HealthSystem HealthSystem;
        
        public int HealthValue
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;
                _health = value;
                OnHealthChanged?.Invoke(_health);
                OnHealthChanged01?.Invoke(_health / (float)_maxHealth);
            }
        }

        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                if (_isAlive == value)
                    return;
                _isAlive = value;
                if (!_isAlive)
                {
                    // UnregisterFromSystem();
                    OnDeath?.Invoke();
                }
            }
        }
    
        public float HealthValue01 => _maxHealth > 0 ? (HealthValue / (float)_maxHealth) : 0f;
        public int MaxHealthValue => _maxHealth;

        protected virtual void Awake()
        {
            SetHealth(MaxHealthValue);
        }

        public void InitHealth(HealthSystem healthSystem)
        {
            HealthSystem = healthSystem;
            
            for (int i = 0; i < _colliders.Length; i++)
            {
                healthSystem.AddHealthArea(_colliders[i], new HealthArea {health = this, isCritical = false, damageMultiplier = 1f});
            }
        
            for (int i = 0; i < _weakpoints.Length; i++)
            {
                healthSystem.AddHealthArea(_weakpoints[i].collider, new HealthArea {health = this, isCritical = true, damageMultiplier = _weakpoints[i].damageMultiplier});
            }
        }
    
        public void SetHealth(int health)
        {
            HealthValue = Mathf.Clamp(health, 0, _maxHealth);
            IsAlive = HealthValue > 0;
        }
    
        public void TakeDamage(int damage)
        {
            if (!IsAlive || damage <= 0) return;

            HealthValue = Mathf.Clamp(HealthValue - damage, 0, _maxHealth);
            Debug.Log($"Health: {HealthValue}, Damage: {damage}.");
            if (HealthValue <= 0)
                IsAlive = false;
        
            OnTakeDamage?.Invoke(HealthValue);
        }

        public void Heal(int heal)
        {
            if (!IsAlive || heal <= 0) return;
        
            HealthValue = Mathf.Clamp(HealthValue + heal, 0, _maxHealth);
        }
        
        // public void UnregisterFromSystem()
        // {
        //     if (_healthSystem == null) return;
        //
        //     foreach (var collider in _colliders)
        //     {
        //         if (collider != null)
        //             _healthSystem.RemoveCollider(collider);
        //     }
        //
        //     foreach (var weakpoint in _weakpoints)
        //     {
        //         if (weakpoint.collider != null)
        //             _healthSystem.RemoveCollider(weakpoint.collider);
        //     }
        // }
    }
}