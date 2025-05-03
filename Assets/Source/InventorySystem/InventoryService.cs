using System;
using Attributes;
using Data.ScriptableObjects.Inventory;
using Player;
using Services;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryService : MonoServiceBase/*GlobalMonoServiceBase*/ 
    {
        [Serializable]
        public struct ItemData
        {
            [ItemId] public string id;
            public int count;
        }
        
        [SerializeField] private ItemLibrary _itemLibrary;

        [SerializeField] private ItemData[] _startingItems;
        [SerializeField] private int _startingCurrency;
        [SerializeField] private InventoryView _inventoryView;
        
        private Inventory _inventory;
        private PlayerController _playerController;
        private InputController _inputController;

        // private bool _inventoryOpened;
        //
        // private bool InventoryOpened
        // {
        //     get => _inventoryOpened;
        //     set
        //     {
        //         _inventoryOpened = value;
        //         
        //         if (_inventoryOpened)
        //         {
        //             _inventoryView.Show();
        //         }
        //         else
        //         {
        //             _inventoryView.Hide();
        //         }
        //         
        //         // InputController inputController = ServiceLocator.Instance.GetService<InputController>();
        //         if (_inputController != null)
        //             _inputController.enabled = !_inventoryOpened;
        //     }
        // }
        
        public override Type Type { get; } = typeof(InventoryService);
        
        public Inventory Inventory => _inventory;
        public ItemLibrary ItemLibrary => _itemLibrary;
        
        protected override void Awake()
        {
            base.Awake();
            _itemLibrary.Init();
        }

        private void Start()
        {
            _inventory = new();
            for (int i = 0; i < _startingItems.Length; ++i)
            {
                ItemData itemData = _startingItems[i];
                _inventory.Add(itemData.id, itemData.count);
            }
            
            _playerController = ServiceLocator.Instance.GetService<PlayerController>();
            _playerController.SetCurrency(_startingCurrency);
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            
            HideInventory();
        }
        
        public void ShowInventory()
        {
            _inventoryView.Show();
            _inputController.OpenMenu();
            _inputController.OnCloseInventory += CloseInventoryHandler;
        }

        public void HideInventory()
        {
            // InventoryOpened = false;
            
            _inventoryView.Hide();
            _inputController.OnCloseInventory -= CloseInventoryHandler;
            _inputController.CloseMenu();
        }

        // public void ToggleInventory()
        // {
        //     InventoryOpened = !InventoryOpened;
        // }

        private void CloseInventoryHandler()
        {
            Debug.Log("Inventory Closed");

            HideInventory();
        }
    }
}