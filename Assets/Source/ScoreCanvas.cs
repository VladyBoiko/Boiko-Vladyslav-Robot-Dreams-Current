using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreCanvas : MonoBehaviour
{
    [SerializeField] private ScoreSystem _scoreSystem;
        
    [SerializeField] private CanvasGroup _canvasGroup;
        
    [SerializeField] private TextMeshProUGUI _name;
    
    [SerializeField] private TextMeshProUGUI _kills;
    [SerializeField] private TextMeshProUGUI _deaths;
    [SerializeField] private TextMeshProUGUI _score;

    [SerializeField] private TextMeshProUGUI _shots;
    [SerializeField] private TextMeshProUGUI _hits;
    [SerializeField] private TextMeshProUGUI _accuracy;

    [SerializeField] private float _fadeDuration = 0.5f;
    
    private bool _updateRequested;
        
    private void Start()
    {
        _name.text = _scoreSystem.PlayerName;
        
        _scoreSystem.OnDataUdpated += DataUpdateHandler;
        InputController.OnScoreInput += ScoreInputHandler;
            
        _updateRequested = true;
    }

    private void DataUpdateHandler() => _updateRequested = true;

    private void LateUpdate()
    {
        if (!_updateRequested)
            return;
        _updateRequested = false;

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        _kills.text = _scoreSystem.KD.x.ToString();
        _deaths.text = _scoreSystem.KD.y.ToString();
        
        _score.text = _scoreSystem.Score.ToString();
        
        _shots.text = _scoreSystem.ShotCount.ToString();
        _hits.text = _scoreSystem.HitCount.ToString();
        _accuracy.text = _scoreSystem.Accuracy.ToString();
    }
    
    private void ScoreInputHandler(bool show)
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(show));
        // _canvasGroup.alpha = show ? 1f : 0f;
    }
    
    private IEnumerator FadeCanvas(bool show)
    {
        float startAlpha = _canvasGroup.alpha;
        float endAlpha = show ? 1f : 0f;
        float timer = 0f;

        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / _fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = endAlpha;
    }
}
