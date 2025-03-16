using System.Collections;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _regenerationDelayTime;
    
    private YieldInstruction _regenerationDelay;

    private void Start()
    {
        if (_health == null)
        {
            Debug.LogError($"{name} is missing a Health component!");
            enabled = false;
            return;
        }
        
        _regenerationDelay = new WaitForSeconds(_regenerationDelayTime);
        
        _health.OnDeath += DeathHandler;
        _health.OnTakeDamage += TakeDamageHandler;
    }

    private void DeathHandler()
    {
        Debug.Log($"{name} has died. Starting regeneration...");
        
        StartCoroutine(RegenerationRoutine());
    }

    private IEnumerator RegenerationRoutine()
    {
        yield return _regenerationDelay;
        _health.SetHealth(_health.MaxHealthValue);
    }

    private void TakeDamageHandler(float newHealt)
    {
        Debug.Log($"Taking damage. New health for {this.name}: {newHealt}");
    }
}