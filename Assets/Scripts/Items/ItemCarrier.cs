using RatStudios.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Items as Objects in the Game World.
/// </summary>
public class ItemCarrier : MonoBehaviour {

    /// <summary>
    /// The item data of this Item GameObject.
    /// </summary>
    private Item item;

    public Item carriedItem {
        get { return item; }
    }

    public void setItem(Item itemToCarry) {
        item = itemToCarry;
        this.GetComponent<SpriteRenderer>().sprite = item.GetSprite();
    }
}
