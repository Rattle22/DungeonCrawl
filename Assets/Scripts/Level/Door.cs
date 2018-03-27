using UnityEngine;
using RatStudios.EventSystem;

namespace RatStudios.Rooms
{
    public class Door : MonoBehaviour
    {
        public Sprite spriteOpen;
        public Sprite spriteClosed;

        //TODO: Phase out RoomEntrancePosition for less restrictive System
        private RoomEntrancePosition placement = RoomEntrancePosition.South;
        private bool open = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (open)
            {
                if (collision.gameObject.GetComponent<Character_Movement>() != null)
                {
                    EventAggregator.singleton.fire(new DoorEnteredEvent(this, placement));
                }
            }
        }

        public void setOpened(bool nowOpen) {
            open = nowOpen;
            GetComponent<SpriteRenderer>().sprite = nowOpen ? spriteOpen : spriteClosed;
        }

        public void setPlacement(RoomEntrancePosition pos) {
            placement = pos;
        }
    }
}