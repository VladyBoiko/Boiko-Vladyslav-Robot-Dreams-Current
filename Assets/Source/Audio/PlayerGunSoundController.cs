using Player;
using UnityEngine;

namespace Audio
{
    public class PlayerGunSoundController : GunSoundController
    {
        [SerializeField] private AudioSource _chargeSource;
        [SerializeField] private AudioClip[] _blasterShotClips;
        [SerializeField] private AudioClip[] _chargeClips;
        [SerializeField] private Transform _chargePoint;

        protected override void Start()
        {
            base.Start();
            
            if (_shooter is PlayerShooter playerShooter)
            {
                playerShooter.OnStartedCharge += ChargeStartHandler;
                playerShooter.OnStoppedCharge += ChargeStopHandler;
            }
        }
        
        private void ChargeStartHandler()
        {
            // _chargeSource.transform.position = _chargePoint.position;
            // _chargeSource.PlayOneShot(_chargeClips[Random.Range(0, _chargeClips.Length)]);
            _chargeSource.clip = _chargeClips[Random.Range(0, _chargeClips.Length)];
            _chargeSource.transform.position = _chargePoint.position;
            _chargeSource.Play();
        }

        private void ChargeStopHandler()
        {
            _chargeSource.Stop();
        }
        
        protected override void ShotHandler(string gunName)
        {
            _shootSource.transform.position = _muzzlePoint.position;
            switch (gunName)
            {
                case "MachineGun":
                    _shootSource.PlayOneShot(_machineGunShotClips[Random.Range(0, _machineGunShotClips.Length)]);
                    break;
                case "Blaster":
                    _shootSource.PlayOneShot(_blasterShotClips[Random.Range(0, _blasterShotClips.Length)]);
                    break;
            }
        }
    }
}