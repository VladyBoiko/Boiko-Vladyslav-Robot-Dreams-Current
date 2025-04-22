using Gamemodes;
using Services;
using UnityEngine;

namespace InteractablesSystem
{
    public class GameModeInteractable : InteractableBase
    {
        public override void Interact()
        {
            ServiceLocator.Instance.GetService<IModeService>().Begin();
            Destroy(this);
        }
    }
}