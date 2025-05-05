using Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CanvasSystem
{
    public class SettingsCanvas : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
    
        [SerializeField] private Canvas _settingsCanvas;
    
        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField, ActionMapDropdown] private string _UIMapName;

        private InputActionMap _UIActionMap;
    
        private Canvas _previousCanvas;
    
        public static SettingsCanvas Instance { get; private set; }
    
        private void Awake()
        {
            _inputActionAsset.Enable();
        
            _UIActionMap = _inputActionAsset?.FindActionMap(_UIMapName);
        
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
            _UIActionMap.Disable();
        }
    
    
        public void HideSettings(Canvas previousCanvas)
        {
            _previousCanvas = previousCanvas; 
            _previousCanvas.gameObject.SetActive(true);
            _settingsCanvas.gameObject.SetActive(false);
            _UIActionMap.Enable();
        }
    
        private void BackHandler()
        {
            if (_previousCanvas != null)
            {
                _previousCanvas.gameObject.SetActive(true);
                _settingsCanvas.gameObject.SetActive(false);
                _UIActionMap.Enable();
            }
        }
    }
}
