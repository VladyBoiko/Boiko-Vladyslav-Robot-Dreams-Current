using Gamemodes;
using UnityEngine;
using System.Collections;

namespace Audio
{
    public class GameModeSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _gameModeSource;
        [SerializeField] private AudioClip _gameModeSound;
        [SerializeField] private SimpleGameMode _gameMode;

        [SerializeField] private float _fadeDuration = 3f;

        private Coroutine _fadeOutCoroutine;
        
        private void Start()
        {
            _gameMode.OnStart += StartHandler;
            _gameMode.OnComplete += CompleteHandler;
        }

        private void StartHandler()
        {
            if (_fadeOutCoroutine != null)
            {
                StopCoroutine(_fadeOutCoroutine);
                _gameModeSource.volume = 1f;
            }
            
            _gameModeSource.clip = _gameModeSound;
            _gameModeSource.Play();
        }

        private void CompleteHandler(bool success)
        {
            if (_fadeOutCoroutine != null)
                StopCoroutine(_fadeOutCoroutine);

            _fadeOutCoroutine = StartCoroutine(FadeOutAudio(_fadeDuration));
        }
        
        private IEnumerator FadeOutAudio(float duration)
        {
            float startVolume = _gameModeSource.volume;

            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                _gameModeSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                yield return null;
            }

            _gameModeSource.Stop();
            _gameModeSource.volume = startVolume;
        }
    }
}