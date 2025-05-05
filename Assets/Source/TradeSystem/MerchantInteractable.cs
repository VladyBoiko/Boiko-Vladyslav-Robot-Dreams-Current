using InteractablesSystem;
using InventorySystem;
using Player;
using Services;
using UnityEngine;

namespace TradeSystem
{
    public class MerchantInteractable : InteractableBase
    {

        [SerializeField] private TradeTable _tradeTable;
        [SerializeField] private InventoryService.ItemData[] _startingItems;
        [SerializeField] private MerchantUI _merchantUI;
        
        private Inventory _inventory;
        
        private bool _active = false;

        private InputController _inputController;
        private InventoryService _inventoryService;

        public Inventory Inventory => _inventory;
        public TradeTable TradeTable => _tradeTable;
        
        private void Start()
        {
            _inventory = new();
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inventoryService = ServiceLocator.Instance.GetService<InventoryService>();
            
            for (int i = 0; i < _startingItems.Length; ++i)
            {
                InventoryService.ItemData itemData = _startingItems[i];
                _inventory.Add(itemData.id, itemData.count);
            }
            
            _tradeTable.Init();
        }

        public override void Interact()
        {
            _active = !_active;
            if (_active)
            {
                _merchantUI.Show();
                _inputController.OnCloseInteractMenu += CloseInteractMenu;
                // _inputController.MenuLock();
                _inputController.OpenMenu();
            }
            else
            {
                CloseInteractMenu();
            }
            tooltip.gameObject.SetActive(!_active);
        }

        // public override void Highlight(bool active)
        // {
        //     if (active)
        //     {
        //         base.Highlight(active);
        //     }
        //     else
        //     {
        //         _active = false;
        //         _merchantUI.Hide();
        //         tooltip.gameObject.SetActive(false);
        //     }
        // }

        private void CloseInteractMenu()
        {
            // Debug.Log("CloseInteractMenu");
            
            _active = false;
            _inputController.OnCloseInteractMenu -= CloseInteractMenu;
            _merchantUI.Hide();
            Highlight(true);
            // _inputController.MenuUnlock();
            // _inputController.Unlock();
            _inputController.CloseMenu();
        }
    }
}