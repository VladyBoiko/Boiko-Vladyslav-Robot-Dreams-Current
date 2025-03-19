using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Canvas _settingsCanvas;   
    
    [SerializeField] private string _gameplaySceneName;

    private void Awake()
    {
        if (_startButton != null) _startButton.onClick.AddListener(StartGameHandler);
        if (_continueButton != null) _continueButton.onClick.AddListener(ContinueGameHandler);
        if (_settingsButton != null) _settingsButton.onClick.AddListener(SettingsGameHandler);
        if (_quitButton != null) _quitButton.onClick.AddListener(QuitGameHandler);
    }

    private void StartGameHandler()
    {
        if (string.IsNullOrEmpty(_gameplaySceneName))
        {
            Debug.LogWarning("No gameplay scene selected");
            return;
        }
        SceneManager.LoadSceneAsync(_gameplaySceneName, LoadSceneMode.Single);
    }

    private void ContinueGameHandler()
    {
        //todo: add continue game logic
    }

    private void SettingsGameHandler()
    {
        if (_settingsCanvas == null || _mainMenuCanvas == null) return;
        _settingsCanvas.gameObject.SetActive(true);
        _mainMenuCanvas.gameObject.SetActive(false);
    }

    private void QuitGameHandler()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
