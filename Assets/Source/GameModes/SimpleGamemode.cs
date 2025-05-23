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
        public event Action OnStart; 
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
        
        private int _currentScore;
        
        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Instance.AddServiceExplicit(typeof(SimpleGameMode), this);
        }
        
        private void Start()
        {
            _playerController = ServiceLocator.Instance.GetService<PlayerController>();
            _scoreSystem = ServiceLocator.Instance.GetService<ScoreSystem>();
            _scoreSystem.OnScoreUpdated += ScoreUpdatedHandler;
            _playerController.Health.OnDeath += PlayerDeathHandler;
            
            enabled = false;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Instance.RemoveServiceExplicit(typeof(SimpleGameMode), this);
            _scoreSystem.OnScoreUpdated -= ScoreUpdatedHandler;
        }
        
        public void Begin()
        {
            _currentScore = 0;
            _time = 0f;

            for (int i = 0; i < _spawners.Length; i++)
            {
                _spawners[i].enabled = true;
            }

            enabled = true;
            OnStart?.Invoke();
        }

        private void Update()
        {
            _time += UnityEngine.Time.deltaTime;
            
            if (_time >= _gameModeDuration || _currentScore >= _maxScore)
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

        private void ScoreUpdatedHandler(int score)
        {
            _currentScore += score;
            Debug.Log($"Score: {_currentScore}");
        }
        
        private void PlayerDeathHandler()
        {
            enabled = false;
            ServiceLocator.Instance.GetService<InputController>().enabled = false;
            OnComplete?.Invoke(false);
        }
    }
}