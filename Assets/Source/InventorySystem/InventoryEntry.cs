using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class InventoryEntry: MonoBehaviour, IPointerDownHandler
    {
        public event Action<ItemEntry> onPressed;
        
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemCount;
        
        private ItemEntry _itemEntry;
        
        public void SetItem(ItemEntry item)
        {
            _itemEntry = item;
            _itemName.text = _itemEntry.Item.Name;
            _itemCount.text = _itemEntry.Count.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onPressed?.Invoke(_itemEntry);
        }
    }
}