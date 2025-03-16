using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public event Action OnDataUdpated;
        
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private DamageDealer _damageDealer;
    
    private string _playerName;
    private Vector2Int _kd;
    private int _score;
    private int _shotCount;
    private int _hitCount;
        
    public string PlayerName => _playerName;
    public Vector2Int KD => _kd;
    public int Score => _score;
    public int HitCount => _hitCount;
    public int ShotCount => _shotCount;
    public int Accuracy => _shotCount == 0f ? 0 : (int)((_hitCount / (float)_shotCount) * 100f);

    private void Start()
    {
        _damageDealer.OnHit += HitHandler;
        _damageDealer.Shooter.OnShot += ShotHandler;
        _healthSystem.OnCharacterDeath += CharacterDeathHandler;

        _playerName = _damageDealer.name;
    }

    private void HitHandler(int hits, int score)
    {
        _hitCount += hits;
        _score += score;
        OnDataUdpated?.Invoke();
    }

    private void ShotHandler()
    {
        _shotCount++;
        OnDataUdpated?.Invoke();
    }

    private void CharacterDeathHandler(Health health)
    {
        _kd.x++;
        _score += 10;
        OnDataUdpated?.Invoke();
    }
}
