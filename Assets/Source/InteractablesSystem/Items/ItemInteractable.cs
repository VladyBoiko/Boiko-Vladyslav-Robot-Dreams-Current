using Attributes;
using InventorySystem;
using Services;
using UnityEngine;

namespace InteractablesSystem.Items
{
    public class ItemInteractable : InteractableBase
    {
        [SerializeField, ItemId] private string _itemId;
        [SerializeField] private int _itemAmount;
        
        public override void Interact()
        {
            ServiceLocator.Instance.GetService<InventoryService>().Inventory.Add(_itemId, _itemAmount);
            
            base.Interact();
        }
    }
}