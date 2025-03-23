using UnityEngine;

public class DayNightChanger : MonoBehaviour
{
    [SerializeField] private float _dayDuration;
    [SerializeField] private AnimationCurve _sunIntensityCurve;
    [SerializeField] private AnimationCurve _moonIntensityCurve;
    [SerializeField] private Gradient _sunColorGradient;
    [SerializeField] private Gradient _moonColorGradient;
    [SerializeField] private Light _sunLight;
    [SerializeField] private Light _moonLight;

    // private float _currentRotation;
    private float _currentSunRotation;
    private float _currentMoonRotation;
    private float _sunHeight;
    private float _moonHeight;
    private bool _isDay;
    private float _timeMultiplier;

    private const float TransitionAngle = 15f;

    private float _time;

    private void Start()
    {
        float correctedDuration = _dayDuration * (180f / (180f - TransitionAngle));
        _timeMultiplier = 180f / correctedDuration;
        
        _isDay = true;
        _sunLight.enabled = true;
        // _moonLight.enabled = false;
        _moonLight.enabled = true;
        _moonLight.intensity = 0.001f;
    }

    private void Update()
    {
        Debug.Log(Time.time);
        
        if (_isDay)
        {
            SunChange();
            
            if (_currentSunRotation >= 180f - TransitionAngle)
            {
                // if (!_moonLight.enabled) _moonLight.enabled = true;
                MoonChange();
            }

            if (_currentSunRotation >= 180f)
            {
                _isDay = false;
                // _sunLight.enabled = false;
                _sunLight.intensity = 0.001f;
                _currentSunRotation = 0f;
            }
        }
        
        else
        {
            MoonChange();
            
            if (_currentMoonRotation >= 180f - TransitionAngle)
            {
                // if(!_sunLight.enabled) _sunLight.enabled = true;
                SunChange();
            }
            
            if (_currentMoonRotation >= 180f)
            {
                _isDay = true;
                // _moonLight.enabled = false;
                _moonLight.intensity = 0.001f;
                _currentMoonRotation = 0f;
            }
        }
    }

    private void SunChange()
    {
        _currentSunRotation += Time.deltaTime * _timeMultiplier;
        
        _sunLight.transform.rotation = Quaternion.Euler(_currentSunRotation, 0f, 0f);
        _sunHeight = Mathf.Clamp01(_currentSunRotation / 180f);
            
        _sunLight.intensity = _sunIntensityCurve.Evaluate(_sunHeight);
        _sunLight.color = _sunColorGradient.Evaluate(_sunHeight);
    }

    private void MoonChange()
    {
        _currentMoonRotation += Time.deltaTime * _timeMultiplier;
        
        _moonLight.transform.rotation = Quaternion.Euler(_currentMoonRotation, 0f, 0f);
        _moonHeight = Mathf.Clamp01(_currentMoonRotation / 180f);
        
        _moonLight.intensity = _moonIntensityCurve.Evaluate(_moonHeight);
        _moonLight.color = _moonColorGradient.Evaluate(_moonHeight);
    }
}
