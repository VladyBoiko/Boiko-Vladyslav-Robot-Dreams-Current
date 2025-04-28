using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects.Inventory
{
    [CreateAssetMenu(fileName = "Item Library", menuName = "Data/Item Library", order = 0)]
    public class ItemLibrary : ScriptableObject
    {
        [SerializeField] private ItemBase[] _items;

        private readonly Dictionary<string, ItemBase> _itemLookup = new();

        private string[] _itemIds;
        
        public string[] ItemIds => _itemIds;

        private void OnValidate()
        {
            Array.Resize(ref _itemIds, _items.Length);
            for (int i = 0; i < _items.Length; ++i)
            {
                _itemIds[i] = _items[i].Id;
            }
        }
        
        public void Init()
        {
            _itemLookup.Clear();
            for (int i = 0; i < _items.Length; ++i)
            {
                ItemBase item = _items[i];
                _itemLookup.Add(item.Id, item);
            }
        }
        
        public bool TryGetItem(string itemID, out ItemBase item)
        {
            return _itemLookup.TryGetValue(itemID, out item);
        }
    }
}