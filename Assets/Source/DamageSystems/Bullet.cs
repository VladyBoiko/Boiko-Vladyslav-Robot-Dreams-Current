using System;
using UnityEngine;

namespace DamageSystems
{
    public class Bullet : MonoBehaviour
    {
        public static event Action<string, Collision> OnStaticHit;
    
        [SerializeField] private Rigidbody _rigidbody;
    
        // private void Awake()
        // {
        //     _rigidbody = GetComponent<Rigidbody>();
        // }
    
        // public Bullet(Rigidbody bulletPrefab, Transform spawnPoint, float speed, float lifetime)
        // {
        //     _rigidbody = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        //     
        //     _rigidbody.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);
        //     
        //     Destroy(_rigidbody.gameObject, lifetime);
        // }

        public void Initialize(float speed, float lifetime)
        {
            _rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
            Destroy(gameObject, lifetime);
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            OnStaticHit?.Invoke("ObjectSpawn", collision);
            Destroy(_rigidbody.gameObject);
        }
    }
}