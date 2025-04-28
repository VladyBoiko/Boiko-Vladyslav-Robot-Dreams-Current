using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class PreviewRotationController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _rotationSpeed;
        
        private readonly float _minVerticalAngle = -60f;
        private readonly float _maxVerticalAngle = 60f;
        
        private bool _isDragging;
        private Vector2 _rotation = Vector2.zero;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging || _target == null)
                return;

            _rotation.x -= eventData.delta.x * _rotationSpeed;
            _rotation.y -= eventData.delta.y * _rotationSpeed;
            _rotation.y = Mathf.Clamp(_rotation.y, _minVerticalAngle, _maxVerticalAngle);
            _target.rotation = Quaternion.Euler(_rotation.y, _rotation.x, 0);
        }
    }
}