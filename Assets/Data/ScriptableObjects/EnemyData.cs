using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Vector2 _idleDuration;
    [SerializeField] private float _maxPatrolStamina;
    [SerializeField] private float _patrolSpeed;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _lookAroundDistance;
    [SerializeField] private float _healthBarDelayTime;
    [SerializeField] private AnimationCurve _fallCurve;
    [SerializeField] private float _aimTime;
    [SerializeField] private float _inaccuracy;
    [SerializeField] private float _accuracyPerShot;
    [SerializeField] private float _minInaccuracy;
    [SerializeField] private float _preferredRange;
    [SerializeField] private float _innacuracyFactor;
    [SerializeField] private float _shotDelay;
    
    public Vector2 IdleDuration => _idleDuration;
    public float MaxPatrolStamina => _maxPatrolStamina;
    public float PatrolSpeed => _patrolSpeed;
    public float ChaseSpeed => _chaseSpeed;
    public float LookAroundDistance => _lookAroundDistance;
    public float HealthBarDelayTime => _healthBarDelayTime;
    public AnimationCurve FallCurve => _fallCurve;
    public float AimTime => _aimTime;
    public float Inaccuracy => _inaccuracy;
    public float AccuracyPerShot => _accuracyPerShot;
    public float MinInaccuracy => _minInaccuracy;
    public float PreferredRange => _preferredRange;
    public float InaccuracyFactor => _innacuracyFactor;
    public float ShotDelay => _shotDelay;
}
