using UnityEngine;
using System;
using Services;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class NavPointProviderZone : MonoServiceBase, INavPointProvider
    {
        [SerializeField] private Vector3 _zoneSize = new Vector3(10f, 0f, 10f);
        [SerializeField] private Vector3 _zoneOffset = Vector3.zero;
        
        public Vector3 ZoneSize => _zoneSize;
        public Vector3 ZoneOffset => _zoneOffset;

        public override Type Type { get; } = typeof(NavPointProviderZone);
        
        public Vector3 GetPoint()
        {
            Vector3 center = transform.position + _zoneOffset;
            float x = Random.Range(center.x - _zoneSize.x / 2, center.x + _zoneSize.x / 2);
            float z = Random.Range(center.z - _zoneSize.z / 2, center.z + _zoneSize.z / 2);
            float y = center.y;

            return new Vector3(x, y, z);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 center = transform.position + _zoneOffset;
            Gizmos.DrawWireCube(center, _zoneSize);
        }
    }
}