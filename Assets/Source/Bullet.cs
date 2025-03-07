using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action<string, Collider> OnStaticHit;
    
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public Bullet(Rigidbody bulletPrefab, Transform spawnPoint, float speed, float lifetime)
    {
        _rigidbody = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        
        _rigidbody.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);
        
        Destroy(_rigidbody.gameObject, lifetime);
    }
    
    
    private void OnCollisionEnter(Collision collision)
    {
        OnStaticHit?.Invoke("ObjectSpawn", collision.collider);
        Destroy(_rigidbody.gameObject);
    }
}