using DamageSystems;
using Player;
using UnityEngine;

namespace Audio
{
    public class GunSoundController : MonoBehaviour
    {
        [SerializeField] protected AudioSource _shootSource;
        [SerializeField] protected AudioSource _hitSource;
        [SerializeField] protected AudioClip[] _machineGunShotClips;
        [SerializeField] protected AudioClip[] _hitClips;
        [SerializeField] protected ShooterBase _shooter;
        [SerializeField] protected Transform _muzzlePoint;

        protected virtual void Start()
        {
            _shooter.OnShot += ShotHandler;
            _shooter.OnHitPointHit += HitPointHitHandler;
        }
        
        protected virtual void ShotHandler(string gunName)
        {
            _shootSource.transform.position = _muzzlePoint.position;
            _shootSource.PlayOneShot(_machineGunShotClips[Random.Range(0, _machineGunShotClips.Length)]);
        }

        private void HitPointHitHandler(Vector3 hitPoint, string gunName)
        {
            if(gunName != "MachineGun") return;
            
            _hitSource.transform.position = hitPoint;
            _hitSource.PlayOneShot(_hitClips[Random.Range(0, _hitClips.Length)]);
        }
    }
}