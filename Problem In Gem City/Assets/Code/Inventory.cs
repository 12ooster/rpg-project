using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

namespace AssemblyCSharp
{
    [Serializable]
    public class Inventory
    {
        public Dictionary<int,InventoryItem> Items;

        public Inventory()
        {
            Items = new Dictionary<int,InventoryItem>();
        }

        public Inventory(Dictionary<int,InventoryItem> items)
        {
            Items = items;
        }

        public void AddItem(InventoryItem item)
        {
            Debug.Log("Item added, with inventory:" + item.ID);

            //Check for matching IDs and then increase item count rather then adding duplicates.
            if (Items.ContainsKey(item.ID))
            {
                Items[item.ID].Count++;
            }
            else
            {
                //If no matching item found, add the item
                Items.Add(item.ID, item);
            }
           
        }

        public void RemoveItem(InventoryItem item)
        {
            //If there are multiple of this item in the inventory, remove 1
            if (Items[item.ID].Count > 1)
            {
                Items[item.ID].Count--;
            }
            else
            {
                Items.Remove(item.ID);
            }
        }
    }
}

