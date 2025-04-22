using System;
using System.Collections.Generic;
using BillBoards;
using Enemy;
using HealthSystems;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace SpawnSystem
{
    public class Spawner : MonoBehaviour, INavPointProvider
    {
        public event Action<int> OnEnemyDeath; 
        
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnRadius = 5f;
        [SerializeField] private Vector2Int _spawnCount = new(1, 3);
        [SerializeField] private Vector2 _periodBounds = new(5f, 10f);
        [SerializeField] private Vector3 _offset;
        
        [SerializeField] private EnemyController _enemyPrefab;
        
        private ObjectPoolSystem<EnemyController> _enemyPool;
        private List<EnemyController> _enemies = new();
        
        private Vector3 _point;
        private NavMeshHit _hit;
        
        private float _time;
        private float _spawnDelay;

        private HealthSystem _healthSystem;
        private BillBoardSystem _billBoardSystem;
        
        public int EnemyCount => _enemies.Count;
        
        public EnemyController EnemyController => _enemyPrefab;
        
        private void Awake()
        {
            _healthSystem = ServiceLocator.Instance.GetService<HealthSystem>();
            _billBoardSystem = ServiceLocator.Instance.GetService<BillBoardSystem>();
            
            _enemyPool = new ObjectPoolSystem<EnemyController>(_enemyPrefab, _spawnPoint, _spawnCount.x, _spawnCount.y);
        }

        private void OnEnable()
        {
            _time = 0f;
            SpawnEnemies(UnityEngine.Random.Range(_spawnCount.x, _spawnCount.y));
            _spawnDelay = UnityEngine.Random.Range(_periodBounds.x, _periodBounds.y);
        }

        private void Update()
        {
            if (_time < _spawnDelay)
            {
                _time += Time.deltaTime;
                return;
            }

            _time = 0f;
            SpawnEnemies(UnityEngine.Random.Range(_spawnCount.x, _spawnCount.y));
            _spawnDelay = UnityEngine.Random.Range(_periodBounds.x, _periodBounds.y);
        }

        private void SpawnEnemies(int count)
        {
            Debug.Log("Enemy spawned");
            
            for (int i = 0; i < count; ++i)
                SpawnEnemy();
        }
        
        private void SpawnEnemy()
        {
            EnemyController enemy = _enemyPool.Get();
            if (enemy == null)
                return;
            
            GetPointInternal();

            int depth = 0;
            while (!_hit.hit)
            {
                GetPointInternal();
                depth++;
                if (depth > 100000)
                {
                    Debug.LogError("Point sampling reached 100000 iterations, aborting");
                    return;
                }
            }
            
            enemy.transform.position = _hit.position;
            enemy.transform.rotation = Quaternion.identity;
            enemy.gameObject.SetActive(true);

            enemy.Health.OnDeath += () => EnemyDeathHandler(enemy);
            
            // enemy.Health.InitHealth(_healthSystem);
            if (!enemy.Health.IsInitialized)
            {
                enemy.Health.InitHealth(_healthSystem);
            }
            _billBoardSystem.AddBillboard(enemy.Billboard);
            _enemies.Add(enemy);
        }
        
        private void EnemyDeathHandler(EnemyController enemy)
        {
            _enemies.Remove(enemy);
            OnEnemyDeath?.Invoke(_enemies.Count);
    
            StartCoroutine(DelayedReturn(enemy));
        }

        private System.Collections.IEnumerator DelayedReturn(EnemyController enemy)
        {
            yield return new WaitForSeconds(enemy.Data.HealthBarDelayTime);
    
            _enemyPool.Return(enemy);
        }

        private void GetPointInternal()
        {
            Vector3 center = transform.position + _offset;
            Vector2 randomInCircle = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            _point.x = randomInCircle.x + center.x;
            _point.y = center.y;
            _point.z = randomInCircle.y + center.z;
            NavMesh.SamplePosition(_point, out _hit, 1.0f, NavMesh.AllAreas);
        }

        public Vector3 GetPoint()
        {
            GetPointInternal();
            return _hit.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Vector3 center = transform.position + _offset;
            
            Gizmos.DrawWireSphere(center, _spawnRadius);
            
            Gizmos.color = _hit.hit ? Color.blue : Color.red;
            
            Gizmos.DrawSphere(_hit.hit ? _hit.position : _point, 0.33f);
        }
    }
}
