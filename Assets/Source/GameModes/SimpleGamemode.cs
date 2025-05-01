using System;
using CanvasSystem.Score;
using Player;
using Services;
using SpawnSystem;
using UnityEngine;

namespace Gamemodes
{
    public class SimpleGameMode: MonoServiceBase, IModeService
    {
        public event Action<bool> OnComplete;

        [SerializeField] private float _gameModeDuration;
        [SerializeField] private Spawner[] _spawners;
        [SerializeField] private int _maxScore;
        
        private ScoreSystem _scoreSystem;
        private PlayerController _playerController;

        private float _time;
        
        public override Type Type { get; } = typeof(IModeService);
        
        public float GameModeDuration => _gameModeDuration;
        
        public float Time => _time;
        
        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Instance.AddServiceExplicit(typeof(SimpleGameMode), this);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Instance.RemoveServiceExplicit(typeof(SimpleGameMode), this);
        }
        
        private void Start()
        {
            _playerController = ServiceLocator.Instance.GetService<PlayerController>();
            _scoreSystem = ServiceLocator.Instance.GetService<ScoreSystem>();
            
            _playerController.Health.OnDeath += PlayerDeathHandler;
            
            enabled = false;
        }
        
        public void Begin()
        {
            _time = 0f;

            for (int i = 0; i < _spawners.Length; i++)
            {
                _spawners[i].enabled = true;
            }

            enabled = true;
        }

        private void Update()
        {
            _time += UnityEngine.Time.deltaTime;
            
            if (_time >= _gameModeDuration || _scoreSystem.Score >= _maxScore)
            {
                enabled = false;
                
                for(int i = 0; i < _spawners.Length; i++)
                    _spawners[i].enabled = false;
                
                _playerController.Health.HealthSystem.
                    RemoveHealthArea(_playerController.Health);
                
                OnComplete?.Invoke(true);
                // Debug.Log("Game Over");
                return;
            }
        }
        
        private void PlayerDeathHandler()
        {
            enabled = false;
            ServiceLocator.Instance.GetService<InputController>().enabled = false;
            OnComplete?.Invoke(false);
        }
    }
}