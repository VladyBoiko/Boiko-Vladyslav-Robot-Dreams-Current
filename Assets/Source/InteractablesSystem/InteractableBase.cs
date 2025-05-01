using System;
using BillBoards;
using CameraSystem;
using Services;
using UnityEngine;

namespace InteractablesSystem
{
    public class InteractableBase: MonoBehaviour, IInteractable
    {
        public event Action<IInteractable> onDestroy;
        
        [SerializeField] protected GameObject rootObject;
        [SerializeField] protected Collider collider;
        [SerializeField] protected BillboardBase tooltip;

        private Transform _transform;
        
        public Vector3 Position => _transform.position;
        
        private void Awake()
        {
            // Debug.Log($"Registering interactable {gameObject.name} with collider {collider.name}");
            
            ServiceLocator.Instance.GetService<InteractableService>().AddInteractable(collider, this);
            tooltip.SetCamera(ServiceLocator.Instance.GetService<CameraController>().Camera);
            Highlight(false);
            
            _transform = collider.transform;
        }

        private void OnDestroy()
        {
            Highlight(false);
            onDestroy?.Invoke(this);
            ServiceLocator.Instance?.GetService<InteractableService>()?.RemoveInteractable(collider, this);
        }

        public virtual void Interact()
        {
            Destroy(rootObject);
        }

        public virtual void Highlight(bool active)
        {
            tooltip.gameObject.SetActive(active);
        }
    }
}