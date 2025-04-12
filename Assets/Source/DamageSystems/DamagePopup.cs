using TMPro;
using UnityEngine;

namespace DamageSystems
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _lifetime = 1f;
        [SerializeField] private Vector2 _randomOffsetRange = new Vector2(-0.5f, 0.5f);
        [SerializeField] private TextMeshPro _textMesh;
    
        private float _timer;
        private Transform _cameraTransform;

        public void Initialize(int damageAmount, Camera camera)
        {
            _textMesh.text = damageAmount.ToString();
        
            transform.position += new Vector3(
                Random.Range(_randomOffsetRange.x, _randomOffsetRange.y),
                Random.Range(_randomOffsetRange.x, _randomOffsetRange.y),
                0f
            );
        
            _cameraTransform = camera.transform;
            _timer = _lifetime;
        }

        private void Update()
        {
            transform.LookAt(_cameraTransform);
            transform.rotation = Quaternion.LookRotation(_cameraTransform.forward);
        
            transform.position += Vector3.up * (_moveSpeed * Time.deltaTime);

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}