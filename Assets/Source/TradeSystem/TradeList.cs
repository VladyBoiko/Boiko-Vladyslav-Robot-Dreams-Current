using System;
using System.Collections.Generic;
using Data.ScriptableObjects.Inventory;
using InventorySystem;
using Player;
using Services;
using UnityEngine;

namespace TradeSystem
{
    public class TradeList : MonoBehaviour
    {
        public event Action OnInventriesUpdate;
        
        [SerializeField] private TradeListEntry _tradeListEntryPrefab;
        [SerializeField] private Transform _content;
        
        [SerializeField] private bool _isBuyList = true;
        
        private readonly List<TradeListEntry> _buyListEntries = new();

        private Dictionary<ItemBase, TradeEntry> _tradeTable;
        private Inventory _merchantInventory;
        private Inventory _buyerInventory;
        
        private InventoryService _inventoryService;
        private PlayerController _playerController;

        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<InventoryService>();
            _playerController = ServiceLocator.Instance.GetService<PlayerController>();
        }

        public void Open(Dictionary<ItemBase, TradeEntry> tradeTable,
            Inventory merchantInventory, Inventory buyerInventory)
        {
            _tradeTable = tradeTable;
            _merchantInventory = merchantInventory;
            _buyerInventory = buyerInventory;
            
            UpdateList();
        }

        public void UpdateList()
        {
            for (int i = 0; i < _buyListEntries.Count; ++i)
            {
                // _buyListEntries[i].onPressed -= CompleteTrade;
                Destroy(_buyListEntries[i].gameObject);
            }
            _buyListEntries.Clear();
            
            List<ItemEntry> sortedEntries = new List<ItemEntry>();
            
            for (int i = 0; i < _merchantInventory.Count; ++i)
            {
                sortedEntries.Add(_merchantInventory[i]);
            }
            
            sortedEntries.Sort((a, b) =>
            {
                int nameCompare = string.Compare(a.Item.Name, b.Item.Name, StringComparison.Ordinal);
                if (nameCompare != 0)
                    return nameCompare;
                return b.Count.CompareTo(a.Count);
            });

            foreach (ItemEntry entry in sortedEntries)
            {
                Debug.Log(entry.Item.Name);
            }
            
            for (int i = 0; i < sortedEntries.Count; ++i)
            {
                ItemEntry item = sortedEntries[i];
                TradeListEntry tradeListEntry = Instantiate(_tradeListEntryPrefab, _content);
                tradeListEntry.gameObject.SetActive(true);
                if (_tradeTable.TryGetValue(item.Item, out TradeEntry tradeEntry))
                {
                    tradeListEntry.SetItem(item);
                    tradeListEntry.SetTrade(tradeEntry);
                    // tradeListEntry.onPressed += (trade) => CompleteTrade(trade);
                    
                    bool isEnabled = false;
                    
                    if (_isBuyList)
                    {
                        bool hasProduct = false, hasPayment = false;
                        
                        if (_merchantInventory.TryGetItemEntry(item.Item, out List<ItemEntry> productEntryList))
                        {
                            int total = 0;
                            for (int j = 0; j < productEntryList.Count; ++j)
                            {
                                total += productEntryList[j].Count;
                            }
                        
                            hasProduct = total >= tradeEntry.productAmount;
                        }
                        
                        hasPayment = _playerController.Currency 
                                     >= tradeEntry.price;
                        
                        isEnabled = hasProduct && hasPayment;
                    }
                    
                    else
                    {
                        bool hasProduct = _buyerInventory.Contains(tradeEntry.product);
                        isEnabled = hasProduct;
                    }
                    
                    tradeListEntry.SetEnabled(isEnabled);
                    tradeListEntry.onPressed += EntryPressed;
                }
                else
                {
                    tradeListEntry.SetItem(item);
                    tradeListEntry.SetEnabled(false);
                }
                _buyListEntries.Add(tradeListEntry);
            }
        }
        
        private void EntryPressed(TradeEntry entry, bool shiftClick)
        {
            if (_isBuyList)
            {
                int totalAmount = 1;
                if (shiftClick)
                {
                    totalAmount *= 10;
                    Debug.Log(totalAmount);
                }
                
                if (_playerController.Currency >= entry.price * totalAmount)
                {
                    Debug.Log($"{_playerController.Currency} - {entry.price * totalAmount}");
                    _playerController.RemoveCurrency(entry.price * totalAmount);
                    
                    _ = _merchantInventory.Remove(entry.productId, entry.productAmount * totalAmount);
                    _buyerInventory.Add(entry.productId, entry.productAmount * totalAmount);
                    OnInventriesUpdate?.Invoke();
                    Debug.Log($"Payment processed");
                }
                else
                {
                    Debug.Log("Not enough currency to buy.");
                }
            }
            else
            {
                int totalAmount = 1;
                if (shiftClick)
                {
                    totalAmount *= _merchantInventory.GetItemCount(entry.product);
                    // totalAmount = 10;
                    Debug.Log(totalAmount);
                }
                
                if (_buyerInventory.Contains(entry.product))
                {
                    _playerController.AddCurrency(entry.price * totalAmount);
                    
                    _ = _merchantInventory.Remove(entry.productId, entry.productAmount * totalAmount);
                    _buyerInventory.Add(entry.productId, entry.productAmount * totalAmount);
                    OnInventriesUpdate?.Invoke();
                    Debug.Log($"Item sold for {entry.price} currency");
                }
                else
                {
                    Debug.Log("You don't have the item to sell.");
                }
            }
        }
        
        public void SetBuyMode(bool isBuyList)
        {
            _isBuyList = isBuyList;
        }
        
        private void CompleteTrade(TradeEntry trade)
        {
            //ItemEntry product = _merchantInventory.Find(item => FindProductPredicate(trade, item));
            //ItemEntry payment = _buyerInventory.Find(item => BuyPredicate(trade, item));
        }
        
        // public bool FindProductPredicate(TradeEntry trade, ItemEntry item)
        // {
        //     return trade.productId == item.Item.Id;
        // }
        //
        // public bool HasItemPredicate(ItemEntry entry, ItemBase item)
        // {
        //     return entry.Item == item;
        // }
    }
}