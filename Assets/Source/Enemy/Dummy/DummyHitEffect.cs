using DamageSystems;
using UnityEngine;

namespace Enemy.Dummy
{
    public class DummyHitEffect : MonoBehaviour, IHitEffectProvider
    {
        [SerializeField] private Material _dummyMaterial;
        public Material GetHitEffectMaterial() => _dummyMaterial;
    }
}