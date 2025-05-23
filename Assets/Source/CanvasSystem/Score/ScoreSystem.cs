using System;
using DamageSystems;
using HealthSystems;
using Player;
using Player.Animation;
using SaveSystem;
using Services;
using UnityEngine;

namespace CanvasSystem.Score
{
    public class ScoreSystem : MonoServiceBase /*GlobalMonoServiceBase*/
    {
        public event Action OnDataUdpated;
        
        public override Type Type { get; } = typeof(ScoreSystem);
        
        [SerializeField] private HealthSystem _healthSystem;
        // [SerializeField] private DamageDealer _damageDealer;
        [SerializeField] private DamageDealerBase _damageDealer; 
        [SerializeField] private AnimatedPlayerDeath _animatedPlayerDeath;
        
        private ISaveService _saveService;
        
        private string _playerName;
        private Vector2Int _kd;
        private int _score;
        private int _shotCount;
        private int _hitCount;
        
        public string PlayerName => _playerName;
        public Vector2Int Kd => _kd;
        public int Score => _score;
        public int HitCount => _hitCount;
        public int ShotCount => _shotCount;
        public int Accuracy => _shotCount == 0f ? 0 : (int)((_hitCount / (float)_shotCount) * 100f);

        private void Start()
        {
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();

            ScoreSaveData scoreData = _saveService.SaveData.scoreData;
            
            _score = scoreData.score;
            _hitCount = scoreData.hitCount;
            _shotCount = scoreData.shotCount;
            _kd = scoreData.kd;
            
            _damageDealer.OnHit += HitHandler;
            _damageDealer.Shooter.OnShot += ShotHandler;
            _healthSystem.OnCharacterDeath += CharacterDeathHandler;
            _animatedPlayerDeath.OnPlayerDeath += PlayerDeathHandler;

            _playerName = _damageDealer.name;
        }

        protected override void OnDestroy()
        {
            SaveScore();
            
            _damageDealer.OnHit -= HitHandler;
            _damageDealer.Shooter.OnShot -= ShotHandler;
            _healthSystem.OnCharacterDeath -= CharacterDeathHandler;
            _animatedPlayerDeath.OnPlayerDeath -= PlayerDeathHandler;
            
            base.OnDestroy();
        }

        private void SaveScore()
        {
            _saveService.SaveData.scoreData.score = _score;
            _saveService.SaveData.scoreData.hitCount = _hitCount;
            _saveService.SaveData.scoreData.shotCount = _shotCount;
            _saveService.SaveData.scoreData.kd = _kd;
        }
        
        private void HitHandler(int hits, int score)
        {
            _hitCount += hits;
            SaveScore();
            // _score += score;
            OnDataUdpated?.Invoke();
        }

        private void ShotHandler(string gunName)
        {
            _shotCount++;
            SaveScore();
            OnDataUdpated?.Invoke();
        }

        private void CharacterDeathHandler(Health health)
        {
            if(health == _animatedPlayerDeath.Health)
                return;
            _kd.x++;
            _score += 10;
            SaveScore();
            OnDataUdpated?.Invoke();
        }

        private void PlayerDeathHandler(Health health)
        {
            if (health != _animatedPlayerDeath.Health)
                return;
            _kd.y++;
            SaveScore();
            OnDataUdpated?.Invoke();
        }
    }
}
