using Enemy;
using HealthSystems;
using Services;
using UnityEngine;
using System;
using SaveSystem;

namespace Player
{
    public class PlayerController : MonoServiceBase
    {
        
        [SerializeField] private CharacterController _controller;
        [SerializeField] private TargetableBase _targetable;
        [SerializeField] private Health _health;
        [SerializeField] private float _speed;
    
        [SerializeField] private HealthSystem _healthSystem;
        
        public override Type Type { get; } = typeof(PlayerController);
        
        private Transform _transform;
        private Vector2 _moveInput;

        private int _currency;
        
        private InputController _inputController;
        private ISaveService _saveService;
        
        public CharacterController CharacterController => _controller;
        public TargetableBase Targetable => _targetable;
        public Health Health => _health;
        public int Currency => _currency;
        
        private void Start()
        {
            _healthSystem = ServiceLocator.Instance.GetService<HealthSystem>();
            _healthSystem.OnCharacterDeath += CharacterDeathHandler;
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnMoveInput += MoveHandler;
            
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            LoadPlayerInfo();
            
            _transform = transform;
            if (_controller) return;
                Debug.LogError("No CharacterController attached");
            enabled = false;
        }

        private void OnDisable()
        {
            SavePlayerInfo();
        }

        private void FixedUpdate()
        {
            Vector3 forward = _transform.forward;
            Vector3 right = _transform.right;
            Vector3 movement = forward * _moveInput.y + right * _moveInput.x;
            _controller.SimpleMove(movement * _speed);
        }

        private void CharacterDeathHandler(Health health)
        {
            if (health != _health)
            {
                _currency += 10;
                SavePlayerInfo();
            }
        }
        
        private void MoveHandler(Vector2 moveInput)
        {
            // Debug.Log($"Move Input: {moveInput}");
            _moveInput = moveInput.normalized;
        }

        private void SetCurrency(int amount)
        {
            _currency = amount;
        }

        public void AddCurrency(int amount)
        {
            _currency += amount;
            SavePlayerInfo();
        }

        public void RemoveCurrency(int amount)
        {
            _currency -= amount;
            SavePlayerInfo();
        }

        private void LoadPlayerInfo()
        {
            Debug.Log($"Loading Player Info. Currency: {_saveService.SaveData.inventoryData.currency}");
            SetCurrency(_saveService.SaveData.inventoryData.currency);
        }

        private void SavePlayerInfo()
        {
            _saveService.SaveData.inventoryData.currency = _currency;
        }
    }
}
