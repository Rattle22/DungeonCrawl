using RatStudios.EventSystem;
using RatStudios.Items;
using RatStudios.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Character_Inventory : MonoBehaviour
{

    private Dictionary<Slot, Item> inventory = new Dictionary<Slot, Item>();

    private void Start()
    {
        foreach (Slot s in Enum.GetValues(typeof(Slot))) {
            addItem(ItemGenerator.generateGenericItem(s, 1));
        }
    }

    private void OnDestroy()
    {
        foreach (Item i in inventory.Values) {
            i.SetEnabled(false);
        }
    }

    public Item getItem(Slot slot) {
        Item outItem;
        inventory.TryGetValue(slot, out outItem);
        return outItem;
    }

    public void addItem(Item itemToAdd)
    {

        Slot slot = itemToAdd.GetSlot();
        Item oldItem;

        if (inventory.TryGetValue(slot, out oldItem))
        {
            if (!(oldItem.Equals(itemToAdd)))
            {
                oldItem.SetEnabled(false);
                inventory.Remove(slot);
                EventAggregator.singleton.fire(new ItemUnequippedEvent(this, oldItem));
            }
            else
            {
                return;
            }
        }
        
        inventory.Add(slot, itemToAdd);
        InventoryDisplay.ShowItem(slot, itemToAdd);
        itemToAdd.SetEnabled(true);
        EventAggregator.singleton.fire(new ItemEquippedEvent(this, itemToAdd));
    }
}

