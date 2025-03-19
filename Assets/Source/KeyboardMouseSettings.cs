using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardMouseSettings : MonoBehaviour
{
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private TMP_InputField _sensitivityInput;
    
    private const string SensitivityKey = "MouseSensitivity";
    
    private void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat(SensitivityKey, 1f);
        _sensitivitySlider.value = savedSensitivity;
        _sensitivityInput.text = savedSensitivity.ToString("0.00");

        _sensitivitySlider.onValueChanged.AddListener(OnSliderValueChanged);
        _sensitivityInput.onEndEdit.AddListener(OnInputValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        _sensitivityInput.text = value.ToString("0.00");
        PlayerPrefs.SetFloat(SensitivityKey, value);
        PlayerPrefs.Save();
    }
    
    private void OnInputValueChanged(string value)
    {
        if (float.TryParse(value, out float newValue))
        {
            newValue = Mathf.Clamp(newValue, _sensitivitySlider.minValue, _sensitivitySlider.maxValue);
            _sensitivitySlider.value = newValue;
            PlayerPrefs.SetFloat(SensitivityKey, newValue);
            PlayerPrefs.Save();
        }
        else
        {
            _sensitivityInput.text = _sensitivitySlider.value.ToString("0.00");
        }
    }
}
