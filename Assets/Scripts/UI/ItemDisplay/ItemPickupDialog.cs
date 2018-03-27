using RatStudios.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RatStudios.UI
{
    public class ItemPickupDialog : DisplayBase<ItemPickupDialog>
    {
        public delegate void itemChoiceDelegate(Item chosenItem);

        private CanvasGroup parent;
        private ItemDisplay itemDisplay;
        private Button accept;

        public void Start()
        {
            bool noError = true;
            if (singleton == null)
            {
                parent = GetComponent<CanvasGroup>();
                noError = noError && (parent != null);

                itemDisplay = transform.Find("ItemDisplay").GetComponent<ItemDisplay>();
                noError = noError && (itemDisplay != null);

                accept = transform.Find("Take").GetComponent<Button>();
                noError = noError && (accept != null);

                if (noError)
                {
                    singleton = this;
                    setEnabled(false);
                }
                else
                {
                    print(parent);
                    print(itemDisplay);
                    print(accept);
                    throw new WrongUICompositionException();
                }
            }
            else
            {
                throw new DuplicateUIException();
            }

        }

        public static void decide(Item newItem, itemChoiceDelegate callback)
        {
            if (singleton == null)
            {
                throw new MissingUIException();
            }

            singleton.showWith(newItem, callback);
        }

        public static void setPosition(Vector3 pos) {
            singleton.transform.position = pos;
        }

        public static void disable() {
            singleton.setEnabled(false);
        }

        private void showWith(Item newItem, itemChoiceDelegate callback)
        {
            setEnabled(true);

            itemDisplay.displayItem(newItem);


            accept.onClick.AddListener(() => { callback(newItem); setEnabled(false); });
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(accept.gameObject);
        }

        private void setEnabled(bool enabled)
        {
            parent.alpha = enabled ? 1 : 0;
            accept.enabled = enabled;

            if (enabled)
            {
                //addInterrupt(this);
            }
            else
            {
                //removeInterrupt(this);
                accept.onClick.RemoveAllListeners();
            }
            
        }
    }
}