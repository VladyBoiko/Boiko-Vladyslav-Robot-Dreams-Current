using System;
using BillBoards;
using CameraSystem;
using Services;
using UnityEngine;
using XR;

namespace InteractablesSystem
{
    public class InteractableBase: MonoBehaviour, IInteractable
    {
        public event Action<IInteractable> onDestroy;
        
        [SerializeField] protected GameObject rootObject;
        [SerializeField] protected Collider collider;
        [SerializeField] protected BillboardBase tooltip;
        [SerializeField] private InteractableType _type  = InteractableType.Undefined;

        private Transform _transform;
        
        public Vector3 Position => _transform.position;
        public InteractableType Type => _type;
        
        private void Awake()
        {
            // Debug.Log($"Registering interactable {gameObject.name} with collider {collider.name}");
            
            ServiceLocator.Instance.GetService<InteractableService>().AddInteractable(collider, this);
            // tooltip.SetCamera(ServiceLocator.Instance.GetService<CameraController>().Camera);
            tooltip.SetCamera(ServiceLocator.Instance.GetService<XRPlayerController>().Camera);
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