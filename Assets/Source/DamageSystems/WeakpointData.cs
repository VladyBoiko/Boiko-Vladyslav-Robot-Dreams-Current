using System;
using UnityEngine;

namespace DamageSystems
{
    [Serializable]
    public struct WeakpointData
    {
        public Collider collider;
        public float damageMultiplier;
    }
}