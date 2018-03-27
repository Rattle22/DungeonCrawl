using RatStudios.Items;

namespace RatStudios.EventSystem
{
    public class ItemEquippedEvent : Event
    {
        public readonly Item equippedItem;

        public ItemEquippedEvent(object actInvoker, Item actEquippedItem) : base(actInvoker)
        {
            equippedItem = actEquippedItem;
        }
    }
}