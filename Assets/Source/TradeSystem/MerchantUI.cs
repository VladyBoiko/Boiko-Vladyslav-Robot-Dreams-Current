using InventorySystem;
using Services;
using UnityEngine;

namespace TradeSystem
{
    public class MerchantUI : MonoBehaviour
    {
        [SerializeField] private MerchantInteractable _merchantInteractable;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TradeList _buyList;
        [SerializeField] private TradeList _sellList;

        private InventoryService _inventoryService;
        
        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<InventoryService>();

            _buyList.OnInventriesUpdate += UpdateLists;
            _sellList.OnInventriesUpdate += UpdateLists;
            
            Hide();
        }

        private void UpdateLists()
        {
            _buyList.UpdateList();
            _sellList.UpdateList();
        }
        
        public void Show()
        {
            _canvas.enabled = true;
            _buyList.SetBuyMode(true);
            _buyList.Open(_merchantInteractable.TradeTable.BuyTable, _merchantInteractable.Inventory, _inventoryService.Inventory);
            _sellList.SetBuyMode(false);
            _sellList.Open(_merchantInteractable.TradeTable.SellTable, _inventoryService.Inventory, _merchantInteractable.Inventory);
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
    }
}