using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private Shooter _shooter;
    private void OnEnable()
    {
        _shooter.OnHit += HitInputHandler;
    }

    private void OnDisable()
    {
        _shooter.OnHit -= HitInputHandler;
    }

    private void HitInputHandler(string shootingModeName, Collider hitCollider)
    {
        if (hitCollider == null)
        {
            Debug.Log("No collider");
            return;
        }
        Debug.Log($"{shootingModeName} hit: {hitCollider.name}");
    }
}
