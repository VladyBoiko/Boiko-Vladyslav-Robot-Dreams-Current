using System;
using UnityEngine;

namespace Data.ScriptableObjects.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Data/Items/Item", order = 0)]
    public class Item : ItemBase
    {
        public override Type ItemType { get; } = typeof(Item);
    }
}