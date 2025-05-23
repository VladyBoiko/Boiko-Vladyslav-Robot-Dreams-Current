using Attributes;
using Player;
using SaveSystem;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CanvasSystem
{
    public class ExitGameMenuCanvas : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private Button _confrimButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField, SceneDropdown] private string _lobbySceneName;
        [SerializeField] private InputController _inputController;
        
        [SerializeField] private Canvas _hudCanvas;

        public static ExitGameMenuCanvas Instance { get; private set; }
        
        private ISaveService _saveService;
        
        public bool Enabled
        {
            get => _canvas.enabled;
            set
            {
                if (_canvas.enabled == value)
                    return;
                _canvas.enabled = value;
                // _inputController.enabled = !value;
            
                if(value)
                    _inputController.Lock();
                else
                    _inputController.Unlock();
                
                _hudCanvas.enabled = !value;
                
                Cursor.visible = value;
                Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
        
        private void Awake()
        {
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            
            Instance = this;
            
            SettingsCanvas.Instance?.HideSettings(_canvas);
        
            _confrimButton.onClick.AddListener(ConfirmButtonHandler);
            _settingsButton.onClick.AddListener(SettingsButtonHandler);
            _cancelButton.onClick.AddListener(CancelButtonHandler);
        }

        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnEscapeInput += EscapeHandler;
        }

        private void EscapeHandler(bool performed)
        {
            if (performed)
            {
                Enabled = !Enabled;
            }
        }

        private void ConfirmButtonHandler()
        {
            _saveService.SaveAll();
            SceneManager.LoadSceneAsync(_lobbySceneName, LoadSceneMode.Single);
        }

        private void SettingsButtonHandler()
        {
            if (SettingsCanvas.Instance != null)
            {
                SettingsCanvas.Instance.ShowSettings(_canvas);
            }
        }
    
        private void CancelButtonHandler()
        {
            Enabled = false;
        }
    }
}
