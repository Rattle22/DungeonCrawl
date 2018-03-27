using RatStudios.EventSystem;

namespace RatStudios.Rooms
{
    public class RoomClearedEvent : Event
    {

        public readonly Room clearedRoom;

        public RoomClearedEvent(object actInvoker, Room actRoom) : base(actInvoker)
        {
            clearedRoom = actRoom;
        }
    }
}