using UnityEngine;

public class HitscanShotAspect : MonoBehaviour
{
    public MaterialPropertyBlock outerPropertyBlock;

    [HideInInspector] public float distance;
        
    [SerializeField] private Transform _transform;
    [SerializeField] private MeshRenderer _outer;
    [SerializeField] private MeshRenderer _inner;
        
    public Transform Transform => _transform;
    public MeshRenderer Outer => _outer;
    public MeshRenderer Inner => _inner;
}
