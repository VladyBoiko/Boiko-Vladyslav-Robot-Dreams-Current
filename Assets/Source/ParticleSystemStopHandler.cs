using UnityEngine;

public class ParticleSystemStopHandler : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        // Debug.Log($"{transform.root.gameObject.name} destroyed");
        Destroy(transform.root.gameObject);
    }
}
