using System;
using System.Collections;
using HealthSystems;
using Services;
using UnityEngine;

namespace Player.Animation
{
    public class AnimatedPlayerDeath : MonoBehaviour
    {
        public event Action<Health> OnPlayerDeath; 
        
        [SerializeField] private Health _health;
        [SerializeField] private GameObject _logicalPlayer;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private HandsIK _handsIK;
        [SerializeField] private float _crossFadeTime  = 0.125f;

        [SerializeField] private string _deathName;
        
        public Health Health => _health;
        
        private void Start()
        {
            _health.OnDeath += DeathHandler;
        }

        private void OnDestroy()
        {
            _health.OnDeath -= DeathHandler;
        }

        private void DeathHandler()
        {
            StartCoroutine(DeathRoutine());
            OnPlayerDeath?.Invoke(_health);
        }

        private IEnumerator DeathRoutine()
        {
            InputController inputController = ServiceLocator.Instance.GetService<InputController>();
            if (inputController)
                inputController.enabled = false;
            _logicalPlayer.SetActive(false);
            yield return null;
            
            _animator.CrossFadeInFixedTime(_deathName, _crossFadeTime);
            _handsIK.DisableIK();
        }
    }
}