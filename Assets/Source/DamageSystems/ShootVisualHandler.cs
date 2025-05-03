using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DamageSystems
{
    public class ShootVisualHandler : MonoBehaviour
    {
        public static event Action<GameObject> OnGlassHit;
    
        // [SerializeField] private Shooter _shooter;
        [SerializeField] private ShooterBase _shooter;
        
        [Header("Muzzle Flash Settings")]
        [SerializeField] private GameObject _muzzleFlashPrefab;
        [SerializeField] private Transform _muzzleFlashPoint;
        // [SerializeField] private float _muzzleFlashDuration = 0.1f;
        // [SerializeField] private ParticleSystem _muzzleFlash;

        [Header("Hit Effect Settings")]
        [SerializeField] private DecalProjector _hitEffectPrefab;
        [SerializeField] private float _decalLifetime = 5f;
        [SerializeField] private Material _woodMaterial;
        [SerializeField] private Material _bodyMaterial;
        [SerializeField] private Material _defaultMaterial;
    
        private void OnEnable()
        {
            _shooter.OnShot += HandleShot;
            // _shooter.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            _shooter.OnShot -= HandleShot;
            // _shooter.OnHit -= HandleHit;
        }

        private void HandleShot()
        {
            // Debug.Log("Shot called");
            
            if (!_muzzleFlashPrefab || !_muzzleFlashPoint) return;
            
            Instantiate(_muzzleFlashPrefab, _muzzleFlashPoint.position, 
                     _muzzleFlashPoint.rotation);
            
            // GameObject muzzleFlash = 
            //     Instantiate(_muzzleFlashPrefab, _muzzleFlashPoint.position, 
            //         _muzzleFlashPoint.rotation, _muzzleFlashPoint);
            
            // Destroy(muzzleFlash, 0.3f);
            
            // StartCoroutine(Flash(muzzleFlash, _muzzleFlashDuration));
            
            // _muzzleFlash.Play();
        }

        private void OnParticleSystemStopped()
        {
            Debug.Log("Particle System Stopped");
        }

        private void HandleHit(string shootingMode, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
        {
            string hitLayerName = LayerMask.LayerToName(hitCollider.gameObject.layer);
        
            if (_hitEffectPrefab && hitLayerName != "Glass")
            {
                DecalProjector decal = Instantiate(_hitEffectPrefab, hitPoint+hitNormal* 0.01f, Quaternion.LookRotation(-hitNormal), hitCollider.transform);
            
                if (hitCollider.TryGetComponent(out IHitEffectProvider hitEffectProvider))
                {
                    decal.material = hitEffectProvider.GetHitEffectMaterial();
                }
                else
                {
                    switch (hitLayerName)
                    {
                        case "Wood": decal.material = _woodMaterial; break;
                        case "Body": decal.material = _bodyMaterial; break;
                        default: decal.material = _defaultMaterial; break;
                    }
                }

                Destroy(decal.gameObject, _decalLifetime);
            }
        
            if(hitLayerName == "Glass")
                OnGlassHit?.Invoke(hitCollider.gameObject);
        }

        private IEnumerator Flash(GameObject effect, float flashDuration)
        {
            float elapsedTime = 0f;
        
            while (elapsedTime < flashDuration)
            {
                float scale = Mathf.Lerp(0.1f, 0.3f, elapsedTime / flashDuration);
                effect.transform.localScale = new Vector3(scale, scale, scale);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            Destroy(effect);
        }
    }
}