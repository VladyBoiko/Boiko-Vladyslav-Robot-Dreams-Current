using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvas : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    
    [SerializeField] private Canvas _settingsCanvas;

    private Canvas _previousCanvas;
    
    public static SettingsCanvas Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        if (_backButton != null) 
            _backButton.onClick.AddListener(BackHandler);
    }
    
    public void ShowSettings(Canvas previousCanvas)
    {
        _previousCanvas = previousCanvas; 
        _previousCanvas.gameObject.SetActive(false);
        _settingsCanvas.gameObject.SetActive(true);
    }
    
    private void BackHandler()
    {
        if (_previousCanvas != null)
        {
            _previousCanvas.gameObject.SetActive(true);
            _settingsCanvas.gameObject.SetActive(false);
        }
    }
}
