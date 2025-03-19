using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvas : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Canvas _settingsCanvas;

    private void Awake()
    {
        if (_backButton != null) 
            _backButton.onClick.AddListener(BackHandler);
    }
    
    private void BackHandler()
    {
        if (_settingsCanvas == null || _mainMenuCanvas == null) return;
        _mainMenuCanvas.gameObject.SetActive(true);
        _settingsCanvas.gameObject.SetActive(false);
    }
}
