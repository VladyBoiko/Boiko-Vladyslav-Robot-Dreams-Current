using UnityEngine;

public class MaterialBlink : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _blinkSpeed;
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;

    private Material _material;
    private Color _baseEmissionColor;
    private void Start()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        
        _material = _renderer.material;
        
        _baseEmissionColor = _material.GetColor("_EmissionColor");
        
        _material.EnableKeyword("_EMISSION");
    }
    
    private void Update()
    {
        float intensity = Mathf.Lerp(_minIntensity, _maxIntensity, Mathf.PingPong(Time.time * _blinkSpeed, 1f));
        _material.SetColor("_EmissionColor", _baseEmissionColor * intensity);
    }
}
