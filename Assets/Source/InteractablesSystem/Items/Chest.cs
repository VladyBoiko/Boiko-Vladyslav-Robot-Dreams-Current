using InteractablesSystem;
using Services;
using UnityEngine;

namespace InventorySystem
{
    public class Chest : InteractableBase
    {
        [SerializeField] private InventoryService.ItemData[] _startingItems;
        
        private InventoryService _inventoryService;

        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<InventoryService>();
        }
        
        public override void Interact()
        {
            for (int i = 0; i < _startingItems.Length; i++)
            {
                InventoryService.ItemData startingItem = _startingItems[i];
                _inventoryService.Inventory.Add(startingItem.id, startingItem.count);
            }
            
            base.Interact();
        }
    }
}