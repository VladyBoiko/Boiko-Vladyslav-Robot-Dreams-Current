using System;
using UnityEngine;

namespace XR
{
    [Serializable]
    public struct BonePair
    {
        public Transform source;
        public Transform target;
        [Range(0f, 1f)] public float weight;

        public void Update()
        {
            source.GetPositionAndRotation(out Vector3 sourcePosition, out Quaternion sourceRotation);
            target.GetPositionAndRotation(out Vector3 targetPosition, out Quaternion targetRotation);
            target.SetPositionAndRotation(Vector3.Lerp(sourcePosition, targetPosition, 1f - weight),
                Quaternion.Lerp(sourceRotation, targetRotation, 1f - weight));
        } 
    }
}