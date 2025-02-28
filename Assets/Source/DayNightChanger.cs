using UnityEngine;

public class DayNightChanger : MonoBehaviour
{
    [SerializeField] private float _dayDuration;
    
    private Light _sunLight;
    private float _timeMultiplayer;
    void Start()
    {
        _sunLight = gameObject.GetComponent<Light>();
        
        _timeMultiplayer = 360f / _dayDuration;
    }
    
    void Update()
    {
        _sunLight.transform.Rotate(Vector3.right * (Time.deltaTime * _timeMultiplayer));
    }
    
}
