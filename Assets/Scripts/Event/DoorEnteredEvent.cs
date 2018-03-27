using RatStudios.EventSystem;

namespace RatStudios.Rooms
{
    public class DoorEnteredEvent : Event
    {
        public readonly RoomEntrancePosition exitPosition;
        public readonly Door doorEntered;

        public DoorEnteredEvent(object actInvoker, RoomEntrancePosition direction) : base(actInvoker){
            exitPosition = direction;
        }
    }
}