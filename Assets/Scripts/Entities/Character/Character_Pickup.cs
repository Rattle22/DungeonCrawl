using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RatStudios.Items;
using RatStudios.UI;

public class Character_Pickup : MonoBehaviour
{
    private ItemCarrier touchedCarrier;
    private Character_Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Character_Inventory>();
        if (inventory == null) {
            throw new MissingComponentException("Expected a Character_Inventory on " + gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        ItemCarrier itemCarrier = other.GetComponent<ItemCarrier>();
        if (itemCarrier != null)
        {
            touchedCarrier = itemCarrier;
            pickUpItem(itemCarrier.carriedItem);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        ItemCarrier itemCarrier = other.GetComponent<ItemCarrier>();
        if (itemCarrier != null)
        {
            ItemPickupDialog.disable();
            touchedCarrier = null;
        }
    }

    private void pickUpItem(Item foundItem)
    {
        Slot slot = foundItem.GetSlot();
        ItemPickupDialog.setPosition(touchedCarrier.transform.position);
        ItemPickupDialog.decide(foundItem, (Item item) =>
        {
            inventory.addItem(item);
            Destroy(touchedCarrier.gameObject);
        });
    }
}