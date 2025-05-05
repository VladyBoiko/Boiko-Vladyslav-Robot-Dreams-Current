using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    public class ObjectPoolSystem<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Transform _poolParent;
        
        private readonly int _maxPoolSize;
        
        private readonly Queue<T> _pool = new();
        
        private int _currentCount;
        
        public ObjectPoolSystem(T prefab, Transform poolParent, int initialSize, int maxPoolSize)
        {
            _prefab = prefab;
            _poolParent = poolParent;
            _maxPoolSize = maxPoolSize;
            
            for (int i = 0; i < initialSize; i++)
                CreateObject();
        }

        private T CreateObject()
        {
            var obj = Object.Instantiate(_prefab, _poolParent);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
            _currentCount++;
            return obj;
        }
        
        public T Get()
        {
            if (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                if (obj.gameObject.activeSelf)
                {
                    // Debug.LogWarning($"[ObjectPool] Trying to get active object: {typeof(T)}");
                    return null;
                }
                obj.gameObject.SetActive(true);
                return obj;
            }
            
            if (_currentCount < _maxPoolSize)
            {
                return CreateObject();
            }

            // Debug.LogWarning($"[ObjectPool] Pool exhausted for type {typeof(T)}.");
            return null;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_poolParent);
            _pool.Enqueue(obj);
        }
    }
}
