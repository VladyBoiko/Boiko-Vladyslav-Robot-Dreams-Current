using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float _initialForce;
    [SerializeField] private float _forceRangeMin;
    [SerializeField] private float _forceRangeMax;
    
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb != null)
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(_forceRangeMin, _forceRangeMax), 
                Random.Range(_forceRangeMin, _forceRangeMax),
                Random.Range(_forceRangeMin, _forceRangeMax)
                );
            randomDirection.Normalize();
            
            _rb.AddForce(randomDirection * _initialForce, ForceMode.Impulse);
        }
    }
}
