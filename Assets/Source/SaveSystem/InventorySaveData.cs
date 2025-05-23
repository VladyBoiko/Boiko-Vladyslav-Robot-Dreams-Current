using System;
using System.Collections.Generic;
using Data.ScriptableObjects.Inventory;
using InventorySystem;

namespace SaveSystem
{
    [Serializable]
    public struct InventorySaveData
    {
        public InventoryItemData[] items;
        public int currency;
    }
    
    [Serializable]
    public struct InventoryItemData
    {
        public string itemId;
        public int count;
    }
}