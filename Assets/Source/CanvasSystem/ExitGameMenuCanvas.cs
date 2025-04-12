using Player;
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
        [SerializeField] private string _lobbySceneName;
        [SerializeField] private InputController _inputController;

        public bool Enabled
        {
            get => _canvas.enabled;
            set
            {
                if (_canvas.enabled == value)
                    return;
                _canvas.enabled = value;
                _inputController.enabled = !value;
            
                Cursor.visible = value;
                Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
        
        private void Awake()
        {
            SettingsCanvas.Instance?.HideSettings(_canvas);
        
            _confrimButton.onClick.AddListener(ConfirmButtonHandler);
            _settingsButton.onClick.AddListener(SettingsButtonHandler);
            _cancelButton.onClick.AddListener(CancelButtonHandler);
        }

        private void Start()
        {
            InputController.OnEscapeInput += EscapeHandler;
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
