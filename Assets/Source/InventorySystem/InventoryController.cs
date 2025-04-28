using Player;
using Services;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        private InputController _inputController;
        private InventoryService _inventoryService;
        
        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inventoryService = ServiceLocator.Instance.GetService<InventoryService>();

            _inputController.OnInventory += InventoryHandler;
        }

        private void InventoryHandler()
        {
            _inventoryService.ToggleInventory();
        }
    }
}