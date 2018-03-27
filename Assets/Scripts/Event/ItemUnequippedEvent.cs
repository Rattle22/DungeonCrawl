using RatStudios.Items;

namespace RatStudios.EventSystem
{
    public class ItemUnequippedEvent : Event
    {
        public readonly Item unequippedItem;

        public ItemUnequippedEvent(object actInvoker, Item actUnequippedItem) : base(actInvoker)
        {
            unequippedItem = actUnequippedItem;
        }
    }
}
