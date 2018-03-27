using RatStudios.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RatStudios.Items;
using RatStudios.UI;

namespace RatStudios.Rooms
{
    public class DungeonManager : MonoBehaviour, ISubscriber<DoorEnteredEvent>, ISubscriber<RoomClearedEvent>, ISubscriber<PlayerDeathEvent>
    {
        #region Attribute Class Declarations
        private abstract class RoomAttribute
        {
            protected int delta = 0;

            public void increaseDelta()
            {
                delta++;
            }

            public abstract void advance();
        }

        private class RoomAttribute<T> : RoomAttribute
        {
            private Queue<T> queue = new Queue<T>();

            /// <summary>
            /// Initializes this attribute with the given set. The order of the attribute elements depend on the order the enumerable returns them in.
            /// </summary>
            /// <param name="initialSet"></param>
            public RoomAttribute(IEnumerable<T> initialSet)
            {
                foreach (T t in initialSet)
                {
                    queue.Enqueue(t);
                }
            }

            public T Current
            {
                get
                {
                    T attribute = queue.Peek();
                    return attribute;
                }
            }

            public override void advance()
            {
                for (; delta > 0; delta--)
                {
                    queue.Enqueue(queue.Dequeue());
                }
            }

            /// <summary>
            /// Returns the value that will come in jumps steps.
            /// 
            /// Primarily to be used for the group Modifier.
            /// </summary>
            /// <param name="jumps">How many steps to skip</param>
            /// <returns>The value that will come in jumps steps</returns>
            public T getValueIn(int jumps)
            {
                T output = queue.Peek();

                while (jumps > queue.Count)
                {
                    jumps -= queue.Count;
                }

                for (int i = 0; i < queue.Count; i++)
                {
                    if (i == jumps)
                    {
                        output = queue.Peek();
                    }
                    queue.Enqueue(queue.Dequeue());
                }

                return output;
            }
        }
        #endregion

        #region Attribute Handling
        private void initilaizeAttributes()
        {
            attributeList = new List<RoomAttribute> { sizeModAttr, enemyType, groupModAttr, passiveAttr };
            changed = new List<RoomAttribute> { sizeModAttr, enemyType };
        }

        /// <summary>
        /// The set with which the roomTypeAtttribute is initialized.
        /// </summary>
        public List<GameObject> initialRoomSet;

        //This is initialized in Start to use the initialRoomSet
        [SerializeField]
        private RoomAttribute<GameObject> roomTypeAttr;

        //TODO: Add last attribute

        private RoomAttribute<int> sizeModAttr = new RoomAttribute<int>(new int[] { 8, 4, 2 });

        public List<GameObject> initialEnemyTypeSet;
        private RoomAttribute<GameObject> enemyType;

        private RoomAttribute<int> groupModAttr = new RoomAttribute<int>(new int[] { 0, 1, 2, 3 });

        //TODO: Make this actually change the type, it gives the enemies some kind of passive effect like regeneration, or absorbing damage to return it later
        private RoomAttribute<int> passiveAttr = new RoomAttribute<int>(new int[] { 4, 2, 1 });

        //Initialized in Start()
        private List<RoomAttribute> attributeList;
        private List<RoomAttribute> changed;
        private System.Random rng = new System.Random();
        
        private RoomAttribute updateAttributes()
        {
            List<RoomAttribute> changeableAttributes = new List<RoomAttribute>(attributeList);
            changeableAttributes.RemoveAll((RoomAttribute r) => changed.Contains(r));

            //TODO: Make RNG deterministic
            int random = rng.Next(0, changeableAttributes.Count - 1);
            RoomAttribute unchanged = changeableAttributes[random];
            changeableAttributes.RemoveAt(random);

            foreach (RoomAttribute r in changeableAttributes)
            {
                r.advance();
            }

            changed = changeableAttributes;

            foreach (RoomAttribute r in attributeList)
            {
                r.increaseDelta();
            }

            return unchanged;
        }
        #endregion

        private void Start()
        {
            roomTypeAttr = new RoomAttribute<GameObject>(initialRoomSet);
            enemyType = new RoomAttribute<GameObject>(initialEnemyTypeSet);

            attributeList = new List<RoomAttribute>(new RoomAttribute[] {sizeModAttr, enemyType, groupModAttr, passiveAttr, roomTypeAttr });
            changed = new List<RoomAttribute>(new RoomAttribute[] {sizeModAttr, enemyType });
            itemGen = new ItemGenerator();

            //TODO: Better starting area.
            EventAggregator.singleton.subscribe<DoorEnteredEvent>(this);
            EventAggregator.singleton.subscribe<RoomClearedEvent>(this);
            EventAggregator.singleton.subscribe<PlayerDeathEvent>(this);
            currentRoom = startRoom;
            currentRoomScript = currentRoom.GetComponent<StartRoom>();
        }

        public void OnDestroy()
        {

            EventAggregator.singleton.removeSubscription<DoorEnteredEvent>(this);
            EventAggregator.singleton.removeSubscription<RoomClearedEvent>(this);
            EventAggregator.singleton.removeSubscription<PlayerDeathEvent>(this);
        }

        #region Room Spawning

        public GameObject startRoom;
        public GameObject playerObject;
        public float defaultRoomWidth;
        public float defaultRoomHeight;
        public float countdownFactor;

        private GameObject currentRoom;
        private Room currentRoomScript;
        private Vector2 currentCoordinates;
        private int lowestY = 0;

        private int clearedRooms = 0;
        private ItemGenerator itemGen;

        private Dictionary<Vector2, Room> map = new Dictionary<Vector2, Room>();
        
        public void OnEventHandler(DoorEnteredEvent e)
        {
            Room room;
            RoomEntrancePosition direction = e.exitPosition;
            Vector2 coordinates = calculateNewRoomPos(direction);

            if (!map.TryGetValue(coordinates, out room)) {
                room = initRoom(coordinates);
                map.Add(coordinates, room);
            }
            transitionTo(coordinates, direction);
        }

        /// <summary>
        /// Calculates the map coordinates a room one step in the given direction should have, based on the current room.
        /// </summary>
        /// <param name="exitDirection">On which side the old room was left</param>
        /// <returns>At which map coordinates on the map the new room would be.</returns>
        private Vector2 calculateNewRoomPos(RoomEntrancePosition exitDirection)
        {

            int x = 0;
            int y = 0;

            switch (exitDirection)
            {
                case RoomEntrancePosition.North:
                    y += 1;
                    break;
                case RoomEntrancePosition.South:
                    y -= 1;
                    break;
                case RoomEntrancePosition.East:
                    x += 1;
                    break;
                case RoomEntrancePosition.West:
                    x -= 1;
                    break;
                default: break;
            }
            Vector2 coordinates = currentCoordinates + (new Vector2(x, y));
            return coordinates;
        }

        /// <summary>
        /// Takes necessary steps to move play into the room at the given map coordinates.
        /// </summary>
        /// <param name="mapCoordinates">At which map coordinates the room to transition to is.</param>
        /// <param name="direction">In which direction the old room was left.</param>
        private void transitionTo(Vector2 mapCoordinates, RoomEntrancePosition direction)
        {
            RoomEntrancePosition entranceDirection = RoomEntrancePosition.North;
            Room existingRoom;
            if (!map.TryGetValue(mapCoordinates, out existingRoom)) {
                throw new MissingRoomException(mapCoordinates);
            }

            switch (direction)
            {
                case RoomEntrancePosition.North:
                    entranceDirection = RoomEntrancePosition.South;
                    break;
                case RoomEntrancePosition.South:
                    entranceDirection = RoomEntrancePosition.North;
                    break;
                case RoomEntrancePosition.East:
                    entranceDirection = RoomEntrancePosition.West;
                    break;
                case RoomEntrancePosition.West:
                    entranceDirection = RoomEntrancePosition.East;
                    break;
                default: break;
            }

            placePlayer(existingRoom, entranceDirection);

            startAnimation(existingRoom.transform.position);

            currentRoom = existingRoom.gameObject;
            currentRoomScript = existingRoom;
            currentCoordinates = mapCoordinates;

            LevelDisplay.setDifficulty(-(int)currentCoordinates.y);
            LevelDisplay.setRoomNumber(currentRoomScript.ID);
        }

        /// <summary>
        /// Initializes a new Room based on the given map coordinates.
        /// </summary>
        /// <param name="newRoomCoordinates">At which map coordinates the new Room should be created.</param>
        /// <returns>A new Room at the given coordinates.</returns>
        private Room initRoom(Vector2 newRoomCoordinates)
        {
            if (newRoomCoordinates.y < lowestY) {
                lowestY = Mathf.RoundToInt(newRoomCoordinates.y);
                Countdown.setRunning(true);
                Countdown.addTime((30 * (countdownFactor + 1))/(-lowestY + countdownFactor));
            }

            cleanRoom();
            Vector3 worldSpacePosition = calculateNewRoomWorldSpacePos(newRoomCoordinates);
            MonsterRoom room = createRoom();
            room.name = newRoomCoordinates + "created by DungeonManager";

            RoomAttribute staticAttr = updateAttributes();

            int groupMod = groupModAttr.Current;
            room.initialize(worldSpacePosition, clearedRooms + 1, playerObject, -currentCoordinates.y, enemyType.Current, sizeModAttr.Current, enemyType.getValueIn(groupMod), sizeModAttr.getValueIn(groupMod));

            return room;
            //currentPosition is changed in calculatenNewRoomPos()
        }

        /// <summary>
        /// Calculates the point in world space at which a room at the given map coordinates should be.
        /// </summary>
        /// <param name="newRoomPos"></param>
        /// <returns></returns>
        private Vector3 calculateNewRoomWorldSpacePos(Vector2 newRoomPos)
        {
            return new Vector3(newRoomPos.x * (defaultRoomWidth + 1) /*Buffer to prevent player from slightly touching the wrong doors*/, newRoomPos.y * (defaultRoomHeight + 1));
        }

        /// <summary>
        /// Removes all temporary objects from the old Room.
        /// TODO: Right now, does not respond to new temporary creations
        /// </summary>
        private void cleanRoom()
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("fireball"))
            {
                Destroy(g);
            }
        }

        /// <summary>
        /// Creates a new Room and returns its Script.
        /// </summary>
        /// <returns>The Script of the newly instantiated Room.</returns>
        private MonsterRoom createRoom()
        {
            GameObject newRoom = Instantiate(roomTypeAttr.Current);
            MonsterRoom newRoomScript = newRoom.GetComponent<MonsterRoom>();

            return newRoomScript;
        }

        /// <summary>
        /// Places the player in the new Room, based on the given Entrance Position.
        /// </summary>
        /// <param name="room">Which room should be entered</param>
        /// <param name="entrancePlacement">The position from which the room was entered.</param>
        private void placePlayer(Room room, RoomEntrancePosition entrancePlacement) {
            //TODO: Improve Placement of player
            Vector3 playerPos = room.getExitPosition(entrancePlacement);
            playerObject.transform.position = playerPos;
        }

        /// <summary>
        /// Handles the animation of a room transition. Currently not implemented.
        /// </summary>
        /// <param name="destination"></param>
        private void startAnimation(Vector3 destination)
        {
            //TODO: Properly implement startAnimation()
            destination.z = -10;
            Camera.main.transform.position = destination;
        }

        public void OnEventHandler(RoomClearedEvent e)
        {
            if (currentRoom.name.Equals(e.clearedRoom.name)) {
                clearedRooms++;
                currentRoomScript.spawnItem(itemGen.generateItem(clearedRooms));
            }
        }

        public void OnEventHandler(PlayerDeathEvent e)
        {
            Scoreboard.addEntry(DateTime.Now, Mathf.RoundToInt(clearedRooms * (-currentCoordinates.y)));
            Scoreboard.show();
        }

        #endregion
    }
}