using System;
using Attributes;
using Data.ScriptableObjects.Inventory;

namespace TradeSystem
{
    [Serializable]
    public class TradeEntry
    {
        [ItemId] public string productId;
        [NonSerialized] public ItemBase product;
        public int productAmount;
        public int price;
    }
}