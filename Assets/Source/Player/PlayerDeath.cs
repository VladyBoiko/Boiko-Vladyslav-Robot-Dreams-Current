using System;
using System.Collections;
using HealthSystems;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private Health _health;               
        [SerializeField] private Transform _bodyTransform;    
        [SerializeField] private Transform _fallMark;          
        [SerializeField] private AnimationCurve _fallCurve;   
        [SerializeField] private GameObject _rootObject;       
        [SerializeField] private float _healthBarDelayTime;
        
        private Vector3 _fallMarkPosition;                      
        private Quaternion _fallMarkRotation;                   

        private void Start()
        {
            _health.OnDeath += DeathHandler;                    
            _fallMark.GetLocalPositionAndRotation(out _fallMarkPosition, out _fallMarkRotation);
        }

        private void DeathHandler()
        {
            StartCoroutine(DelayedDestroy());
        }
        
        private IEnumerator DelayedDestroy()
        {
            float time = 0f;
            float reciprocal = 1f / _healthBarDelayTime;

            while (time < _healthBarDelayTime)
            {
                EvaluateFall(time * reciprocal);
                yield return null;
                time += Time.deltaTime;
            }
            
            EvaluateFall(1f);
            
            // Destroy(_rootObject);
            // _rootObject.SetActive(false);
            
            // SceneManager.LoadSceneAsync(_lobbySceneName, LoadSceneMode.Single);
        }
        
        private void EvaluateFall(float progress)
        {
            float curveFactor = _fallCurve.Evaluate(progress);
            Vector3 position = Vector3.Lerp(Vector3.zero, _fallMarkPosition, curveFactor);
            Quaternion rotation = Quaternion.Slerp(Quaternion.identity, _fallMarkRotation, curveFactor);
            _bodyTransform.SetLocalPositionAndRotation(position, rotation);
        }
    }
}
