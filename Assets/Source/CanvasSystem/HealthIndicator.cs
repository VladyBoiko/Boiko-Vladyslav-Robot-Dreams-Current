using Gamemodes;
using HealthSystems;
using Player;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasSystem
{
    public class HealthIndicator : MonoBehaviour
    {
        [SerializeReference] private Health _health;
        [SerializeField] private RectTransform _healthValue;
        [SerializeField] private RectTransform _damageValue;
        [SerializeField] private Vector2 _referenceSize;
        [SerializeField] private float _damageDecaySpeed;
        [SerializeField] private float _regenerationSpeed;

        [SerializeField] private TextMeshProUGUI _timer;
        
        [SerializeField] private RectTransform _chargeValue;
        [SerializeField] private PlayerShooter _playerShooter;
        
        private float _targetHealth;
        private float _displayedHealth;
        private float _displayedDamage;

        private SimpleGameMode _gameMode;
        
        private void Awake()
        {
            _timer.enabled = false;
            
            _gameMode = ServiceLocator.Instance.GetService<SimpleGameMode>();
        }
        
        private void Start()
        {
            ForceHealth(_health.HealthValue01);
            _health.OnHealthChanged01 += HealthChangedHandler;
        }

        private void Update()
        {
            if (_gameMode.enabled)
                _timer.enabled = true;
            
            if(_timer.enabled)
                _timer.SetText($"Time: {Mathf.Round(_gameMode.GameModeDuration - _gameMode.Time)}");
            
            if (_targetHealth < _displayedHealth)
                _displayedHealth = _targetHealth;
            else
            {
                _displayedHealth =
                    Mathf.MoveTowards(_displayedHealth, _targetHealth,
                    _regenerationSpeed * Time.deltaTime);
            }

            if (_displayedDamage > _displayedHealth)
            {
                _displayedDamage =
                    Mathf.MoveTowards(_displayedDamage, _displayedHealth,
                        _damageDecaySpeed * Time.deltaTime);
            }
            else
                _displayedDamage = _displayedHealth;
            
            _healthValue.sizeDelta = new Vector2(_referenceSize.x * _displayedHealth, _referenceSize.y);
            _damageValue.sizeDelta = new Vector2(_referenceSize.x * _displayedDamage, _referenceSize.y);
            
            UpdateChargeUI();
        }

        private void HealthChangedHandler(float health) => SetHealth(health);

        private void SetHealth(float health)
        {
            _targetHealth = health;
        }

        private void ForceHealth(float health)
        {
            _displayedDamage = _displayedHealth = _targetHealth = health;
        }
        
        private void UpdateChargeUI()
        {
            
            if (_playerShooter == null) return;

            float chargeRatio = _playerShooter.CurrentChargeNormalized;
    
            _chargeValue.sizeDelta = new Vector2(_referenceSize.x * chargeRatio, _referenceSize.y);
        }
    }
}