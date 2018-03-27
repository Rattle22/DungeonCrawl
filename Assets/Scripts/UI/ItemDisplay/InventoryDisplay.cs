using RatStudios.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.UI
{
    public class InventoryDisplay : DisplayBase<InventoryDisplay>
    {

        private Dictionary<Slot, ItemDisplay> slotsDisplays = new Dictionary<Slot, ItemDisplay>();

        // Use this for initialization
        void Awake()
        {
            foreach (Slot s in Enum.GetValues(typeof(Slot))) {
                slotsDisplays.Add(s, transform.Find(s.ToString() + "ItemDisplay").GetComponent<ItemDisplay>());
            }

            if (singleton == null)
            {
                singleton = this;
                GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                throw new DuplicateUIException();
            }
        }

        public static void ShowItem(Slot s, Item item) {
            singleton.Displaytem(s, item);
        }

        private void Displaytem(Slot s, Item item) {
            ItemDisplay display;
            if (slotsDisplays.TryGetValue(s, out display))
            {
                display.displayItem(item);
            }
            else {
                throw new MissingUIException();
            }
        }
    }
}