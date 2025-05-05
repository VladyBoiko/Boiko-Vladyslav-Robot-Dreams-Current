using DamageSystems;
using UnityEngine;

namespace Audio
{
    public class ExplosionSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _explosionSources;
        [SerializeField] private AudioClip[] _explosionClips;
        [SerializeField] private ExplosionController _explosionController;

        private void Start()
        {
            _explosionController.OnExplode += ExplosionHandler;
        }

        private void ExplosionHandler(Vector3 position)
        {
            _explosionSources.transform.position = position;
            _explosionSources.PlayOneShot(_explosionClips[Random.Range(0, _explosionClips.Length)]);
        }
    }
}