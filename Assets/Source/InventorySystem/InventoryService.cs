using System;
using System.Collections.Generic;
using Attributes;
using Data.ScriptableObjects.Inventory;
using Player;
using SaveSystem;
using Services;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

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
        
        [SerializeField] private int _startingCurrency;
        [SerializeField] private InventoryView _inventoryView;
        
        private Inventory _inventory;
        // private PlayerController _playerController;
        private InputController _inputController;
        private ISaveService _saveService;

        private int _currency;
        
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
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            // _playerController = ServiceLocator.Instance.GetService<PlayerController>();
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            
            _inventory = new();
            LoadInventory();
            
            HideInventory();
        }

        protected override void OnDestroy()
        {
            SaveInventory();
            
            base.OnDestroy();
        }
        
        public void ShowInventory()
        {
            _inventoryView.Show();
            _inputController.OpenMenu();
            _inputController.CursorEnable();
            _inputController.OnCloseInventory += CloseInventoryHandler;
        }

        public void HideInventory()
        {
            // InventoryOpened = false;
            
            _inventoryView.Hide();
            _inputController.OnCloseInventory -= CloseInventoryHandler;
            _inputController.CloseMenu();
            _inputController.CursorDisable();
        }

        // public void ToggleInventory()
        // {
        //     InventoryOpened = !InventoryOpened;
        // }

        private void CloseInventoryHandler()
        {
            // Debug.Log("Inventory Closed");

            HideInventory();
        }
        
        public void SaveInventory()
        {
            // List<InventoryItemData> serializedItems = new List<InventoryItemData>();
            //
            // foreach (var itemEntries in _inventory.Items)
            // {
            //     for (int j = 0; j < itemEntries.Value.Count; j++)
            //     {
            //         serializedItems.Add(new InventoryItemData
            //         {
            //             itemId = itemEntries.Value[j].Item.Id,
            //             count = itemEntries.Value[j].Count
            //         });
            //     }
            // }
            //
            // _saveService.SaveData.inventoryData.items = serializedItems;
            Dictionary<string, int> combinedItems = new Dictionary<string, int>();
            
            foreach (var itemEntries in _inventory.Items)
            {
                foreach (var entry in itemEntries.Value)
                {
                    string itemId = entry.Item.Id;
                    int count = entry.Count;
            
                    if (combinedItems.ContainsKey(itemId))
                    {
                        combinedItems[itemId] += count;
                    }
                    else
                    {
                        combinedItems[itemId] = count;
                    }
                }
            }
            
            List<InventoryItemData> serializedItems = new List<InventoryItemData>();
            foreach (var entry in combinedItems)
            {
                serializedItems.Add(new InventoryItemData
                {
                    itemId = entry.Key,
                    count = entry.Value
                });
            }
            
            _saveService.SaveData.inventoryData.items = serializedItems.ToArray();
            // Debug.Log(_playerController.Currency);
            // _saveService.SaveData.inventoryData.currency = _playerController.Currency;
        }

        private void LoadInventory()
        {
            InventorySaveData inventoryData = _saveService.SaveData.inventoryData;
            // Debug.Log(inventoryData.items.Count);

            List<InventoryItemData> serializedItems = new List<InventoryItemData>(inventoryData.items);
            
            if (serializedItems.Count != 0)
            {
                for (int i = 0; i < serializedItems.Count; i++)
                {
                    InventoryItemData itemData = serializedItems[i];
                    _inventory.Add(itemData.itemId, itemData.count);
                }
            }
            else
            {
                Debug.Log("No items found to load inventory");
                // for (int i = 0; i < _startingItems.Length; i++)
                // {
                //     ItemData startingItem = _startingItems[i];
                //     _inventory.Add(startingItem.id, startingItem.count);
                // }
            }
            // _currency = inventoryData.currency > 0 ? inventoryData.currency : _startingCurrency;
            //
            // _playerController.SetCurrency(_currency);
        }
    }
}