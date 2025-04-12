using UnityEngine;

namespace LightSystem
{
    public class FlashingLight : MonoBehaviour
    {
        [SerializeField] private Light[] _lightSource; 
        [SerializeField] private float _speed;  
        [SerializeField] private float _minIntensity;
        [SerializeField] private float _maxIntensity;

        void Awake()
        {
            if (_lightSource == null || _lightSource.Length == 0)
            {
                Debug.LogWarning($"{gameObject.name}: LightSource is null");
                enabled = false;
            }
        }
    
        void Update()
        {
            float lightIntensity = Mathf.PingPong(Time.time * _speed, _maxIntensity - _minIntensity) + _minIntensity;
        
            for(int i = 0; i < _lightSource.Length; i++)
            {
                _lightSource[i].intensity = lightIntensity;
            }
        }
    }
}
