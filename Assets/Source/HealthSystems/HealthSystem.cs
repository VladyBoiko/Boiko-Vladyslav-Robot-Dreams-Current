using System;
using System.Collections.Generic;
using Services;
using UnityEditor;
using UnityEngine;

namespace HealthSystems
{
    public class HealthSystem : MonoServiceBase
    {
        public event Action<Health> OnCharacterDeath;
    
        [SerializeField] private Health[] _healths;
        
        public override Type Type { get; } = typeof(HealthSystem);
        
        protected Dictionary<Collider, HealthArea> _charactersHealth = new();
        
        // /// <summary>
        // /// Editor only method
        // /// </summary>
        [ContextMenu("Find Healths")]
        private void FindHealths()
        {
#if UNITY_EDITOR
            _healths = FindObjectsOfType<Health>();
            EditorUtility.SetDirty(this);
#endif
        }

        protected override void Awake()
        {
            base.Awake();
            
            _charactersHealth.Clear();

            if (_healths == null || _healths.Length == 0)
            {
                Debug.LogWarning("[HealthSystem] No health components found in the scene.");
                return;
            }
        
            for(int i = 0; i < _healths.Length; i++)
            {
                Health health = _healths[i];
                if (health == null) continue;

                health.InitHealth(this);
            
                health.OnDeath += () => CharacterDeathHandler(health);
            }
        }

        public void AddHealthArea(Collider collider, HealthArea healthArea)
        {
            _charactersHealth.Add(collider, healthArea);
        }
    
        public void RemoveHealthArea(Health health)
        {
            var toRemove = new List<Collider>();

            foreach (var pair in _charactersHealth)
            {
                if (pair.Value.health == health)
                {
                    toRemove.Add(pair.Key);
                }
            }

            foreach (Collider col in toRemove)
            {
                _charactersHealth.Remove(col);
            }
        }
        
        public virtual bool GetHealth(Collider collider, out HealthArea health)
        {
            return _charactersHealth.TryGetValue(collider, out health);
        }

        public void CharacterDeathHandler(Health health)
        {
            // Debug.Log("Enemy died");
            RemoveHealthArea(health);
            InvokeCharacterDeath(health);
        }

        public void InvokeCharacterDeath(Health health)
        {
            OnCharacterDeath?.Invoke(health);
        }
        
        // public void RemoveCollider(Collider collider)
        // {
        //     _charactersHealth.Remove(collider);
        // }
    }
}