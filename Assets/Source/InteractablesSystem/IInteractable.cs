using System;
using UnityEngine;

namespace InteractablesSystem
{
    public interface IInteractable
    {
        event Action<IInteractable> onDestroy;
        Vector3 Position { get; }
        void Highlight(bool active);
        void Interact();
    }
}