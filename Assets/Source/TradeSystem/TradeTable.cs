using System.Collections.Generic;
using Data.ScriptableObjects.Inventory;
using InventorySystem;
using Services;
using UnityEngine;

namespace TradeSystem
{
    [CreateAssetMenu(fileName = "TradeTable", menuName = "Data/Items/Trade Table", order = 0)]
    public class TradeTable : ScriptableObject
    {
        [SerializeField] private List<TradeEntry> buyList;
        [SerializeField] private List<TradeEntry> sellList;

        public List<TradeEntry> BuyList => buyList;
        public List<TradeEntry> SellList => sellList;

        private readonly Dictionary<ItemBase, TradeEntry> _buyTable = new();
        private readonly Dictionary<ItemBase, TradeEntry> _sellTable = new();

        private InventoryService _inventoryService;

        public Dictionary<ItemBase, TradeEntry> BuyTable => _buyTable;
        public Dictionary<ItemBase, TradeEntry> SellTable => _sellTable;

        public void Init()
        {
            _inventoryService = ServiceLocator.Instance?.GetService<InventoryService>();
            if (_inventoryService == null)
                return;
            InitList(buyList, _buyTable);
            InitList(sellList, _sellTable);
        }

        private void InitList(List<TradeEntry> list, Dictionary<ItemBase, TradeEntry> table)
        {
            table.Clear();

            for (int i = 0; i < list.Count; ++i)
            {
                TradeEntry entry = list[i];

                if (!_inventoryService.ItemLibrary.TryGetItem(entry.productId, out ItemBase product))
                {
                    Debug.LogError($"Item {entry.productId} not found");
                }
                
                entry.product = product;

                table.Add(product, entry);
            }
        }

        public bool TryGetBuyTrade(ItemBase item, out TradeEntry trade)
        {
            return _buyTable.TryGetValue(item, out trade);
        }

        public bool TryGetSellTrade(ItemBase item, out TradeEntry trade)
        {
            return _sellTable.TryGetValue(item, out trade);
        }
    }
}