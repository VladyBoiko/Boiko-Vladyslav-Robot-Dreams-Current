using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    private void Start()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
