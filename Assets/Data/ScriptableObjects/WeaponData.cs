using UnityEngine;


[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon Data", order = 0)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private int _damage;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private int _maxCharge;
    [SerializeField] private int _chargePerShot;
    [SerializeField] private float _reloadTime;
    [SerializeField] private int _clipCount;
    
    public int Damage => _damage;
    public float CooldownTime => _cooldownTime;
    public int MaxCharge => _maxCharge;
    public int ChargePerShot => _chargePerShot;
    public float ReloadTime => _reloadTime;
    public int ClipCount => _clipCount;
}
