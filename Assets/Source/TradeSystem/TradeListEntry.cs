using System;
using Data.ScriptableObjects.Inventory;
using InventorySystem;
using Player;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TradeSystem
{
    public class TradeListEntry : MonoBehaviour, IPointerClickHandler
    {
        public const float CLICK_DELAY = 0.2f;
        
        public event Action<TradeEntry, bool> onPressed;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemCount;
        [SerializeField] private TextMeshProUGUI _itemPrice;
        
        private float _firstClickTime;
        
        private TradeEntry _tradeEntry;
        private ItemEntry _itemEntry;

        private InventoryService _inventoryService;

        private InventoryService InventoryService
        {
            get
            {
                if (_inventoryService == null)
                {
                    _inventoryService = ServiceLocator.Instance.GetService<InventoryService>();
                }
                return _inventoryService;
            }
        }

        public void SetEnabled(bool enabled)
        {
            _canvasGroup.interactable = enabled;
            _canvasGroup.alpha = enabled ? 1f : 0.25f;
        }

        public void SetItem(ItemEntry itemEntry)
        {
            _itemEntry = itemEntry;
            if (!InventoryService.ItemLibrary.TryGetItem(itemEntry.Item.Id, out ItemBase product))
            {
                Debug.LogError($"Could not find product: {itemEntry.Item.Id}");
                return;
            }
            _itemName.text = product.Name;
            _itemCount.text = _itemEntry.Count.ToString();
        }

        public void SetTrade(TradeEntry tradeEntry)
        {
            _tradeEntry = tradeEntry;
            _itemPrice.text = tradeEntry.price.ToString();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            bool isShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            
            float time = Time.time;
            if (_firstClickTime + CLICK_DELAY < time)
            {
                _firstClickTime = time;
            }
            else
            {
                onPressed?.Invoke(_tradeEntry, isShiftPressed);
            }
        }
    }
}