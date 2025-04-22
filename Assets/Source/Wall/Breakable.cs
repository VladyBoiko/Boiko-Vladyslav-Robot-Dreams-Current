using System.Collections;
using DamageSystems;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject _wholeObject;
    [SerializeField] private GameObject[] _objectBrakingStages;
    [SerializeField] private GameObject _brokenStage;

    private int _hits = 0;
    private void Awake()
    {
        if(!_brokenStage || !_wholeObject) 
            Debug.LogError("Breakable is missing the broken stage or whole object");
        
        _brokenStage.SetActive(false);
        
        for (int i = 1; i < _objectBrakingStages.Length; ++i)
        {
            _objectBrakingStages[i].SetActive(false);
        }
        
        _wholeObject.SetActive(true);
        _objectBrakingStages[0].SetActive(true);
    }

    private void OnEnable()
    {
        ShootVisualHandler.OnGlassHit += GlassHitHandler;
    }

    private void OnDisable()
    {
        ShootVisualHandler.OnGlassHit -= GlassHitHandler;
    }

    private void GlassHitHandler(GameObject hitObject)
    {
        if (hitObject != _wholeObject) return;
        
        _objectBrakingStages[_hits].SetActive(false);
        _hits++;
        // Debug.Log(_hits);
        
        if (_hits < _objectBrakingStages.Length)
        {
            _objectBrakingStages[_hits].SetActive(true);
        }
        else
        {
            _wholeObject.SetActive(false);
            _brokenStage.SetActive(true);

            foreach (Transform t in _brokenStage.transform)
            {
                StartCoroutine(Shrink(t, 1.5f));
            }
        }
    }

    IEnumerator Shrink(Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Vector3 scale = t.localScale;

        while (scale.x > 0)
        {
            scale -= new Vector3(1f, 1f, 1f);
            
            t.localScale = scale;
            yield return new WaitForSeconds(0.05f);
        }
        
        Destroy(gameObject);
    }
}