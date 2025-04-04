#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action<Health> OnCharacterDeath;
    
    [SerializeField] private Health[] _healths;

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

    protected virtual void Awake()
    {
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
    
    public virtual bool GetHealth(Collider collider, out HealthArea health)
    {
        return _charactersHealth.TryGetValue(collider, out health);
    }

    protected void CharacterDeathHandler(Health health)
    {
        // if (_charactersHealth.Contains(health))
        // {
        //     _charactersHealth.Remove(health); 
        // }
        
        OnCharacterDeath?.Invoke(health);
    }
}