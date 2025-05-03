using System;
using System.Collections;
using Attributes;
using Gamemodes;
using Player;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CanvasSystem
{
    public class EndScreen : MonoBehaviour
    {
        [Serializable] private struct ScreenUI
        {
            public Canvas Canvas;
            public CanvasGroup CanvasGroup;
            public CanvasGroup ButtonGroup;
            public Button ExitButton;
            public Transform RootTransform;
        }
        
        [SerializeField] private ScreenUI _winScreen;
        [SerializeField] private ScreenUI _loseScreen;
        [SerializeField] private float _delay = 1f;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private float _scaleDuration = 0.5f;
        
        // [SerializeField] private Canvas[] _hudCanvas;

        [SerializeField, SceneDropdown] private string _lobbySceneName;
        
        private PlayerController _playerController;
        private InputController _inputController;
        
        private void Awake()
        {
            _winScreen.Canvas.enabled = false;
            _loseScreen.Canvas.enabled = false;

            _winScreen.ExitButton.onClick.AddListener(ExitButtonHandler);
            _loseScreen.ExitButton.onClick.AddListener(ExitButtonHandler);

            if(String.IsNullOrEmpty(_lobbySceneName)) 
                _lobbySceneName = "Main menu";
            
            _playerController = ServiceLocator.Instance.GetService<PlayerController>();  
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            
            ServiceLocator.Instance.GetService<IModeService>().OnComplete += ModeCompletedHandler;
        }

        private void ModeCompletedHandler(bool success)
        {
            // for(int i = 0; i < _hudCanvas.Length; i++)
            //     _hudCanvas[i].enabled = false;

            for (var i = 0; i < ClosableCanvasRegistry.Registered.Count; i++)
            {
                var canvas = ClosableCanvasRegistry.Registered[i];
                canvas.enabled = false;
            }

            if (_playerController != null)
            {
                // Debug.Log(_playerController);
                _playerController.enabled = false;
            }

            if (_inputController != null)
            {
                _inputController.FullLock();

                _inputController.CursorEnable();
            }

            if (success)
                Show(_winScreen);
            else
                Show(_loseScreen);
        }

        private void Show(ScreenUI screen)
        {
            screen.Canvas.enabled = true;
            screen.CanvasGroup.alpha = 0f;
            screen.CanvasGroup.interactable = false;
            screen.RootTransform.localScale = Vector3.zero;
            
            screen.ButtonGroup.alpha = 0f;
            screen.ButtonGroup.interactable = false;
            
            StartCoroutine(FadeIn(screen));
            
        }

        private IEnumerator FadeIn(ScreenUI screen)
        {
            yield return new WaitForSeconds(_delay);

            float time = 0f;
            while (time < _scaleDuration)
            {
                float progress = time / _scaleDuration;
                screen.RootTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);
                screen.CanvasGroup.alpha = progress;
                yield return null;
                time += Time.deltaTime;
            }

            screen.RootTransform.localScale = Vector3.one;
            screen.CanvasGroup.alpha = 1f;

            screen.CanvasGroup.interactable = true;
            
            yield return new WaitForSeconds(_delay);

            time = 0f;
            while (time < _fadeDuration)
            {
                float progress = time / _fadeDuration;
                screen.ButtonGroup.alpha = progress;
                yield return null;
                time += Time.deltaTime;
            }

            screen.ButtonGroup.alpha = 1f;
            screen.ButtonGroup.interactable = true;
        }

        private void ExitButtonHandler()
        {
            if(!String.IsNullOrEmpty(_lobbySceneName))
                SceneManager.LoadSceneAsync(_lobbySceneName, LoadSceneMode.Single);
        }
    }
}