using RatStudios.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RatStudios.Rooms
{
    public class MonsterRoom : Room, ISubscriber<EnemyGroupDiedEvent>
    {
        #region Attributes
        EnemyGroup master;
        EnemyGroup slave;

        //TODO: This seems both unsafe and too specific for *all* Monster rooms. Probably change?
        public List<Vector2> spawnPositions;

        //TODO: Streamline use of Prefab vs Prototype
        private GameObject doorPrefab;
        private Dictionary<RoomEntrancePosition, Door> doors = new Dictionary<RoomEntrancePosition, Door>();
        #endregion

        #region Init and Deinit
        void Awake()
        {
            doorPrefab = Resources.Load<GameObject>("Prototypes/Room Interiors Prefabs/Door");
            EventAggregator.singleton.subscribe(this);
        }

        //TODO: Change to also take other attributes as arguments
        public void initialize(Vector3 worldSpacePosition, int id, GameObject target, float difficulty, GameObject mType, int mSize, GameObject sType, int sSize)
        {
            transform.position = worldSpacePosition;
            this.id = id;

            master = new EnemyGroup(target, getSpawnPositions(), difficulty, mType, mSize);
            slave = new EnemyGroup(target, getSpawnPositions(), difficulty, sType, sSize);

            master.spawn(transform.position);
            slave.spawn(transform.position);

            initDoors();
            setDoorsOpened(false);
        }

        private void initDoors()
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject newDoor = Instantiate(doorPrefab);
                newDoor.transform.parent = this.transform;
                Door doorScript = newDoor.GetComponent<Door>();
                doorScript.setPlacement((RoomEntrancePosition)i);
                Vector3 pos = transform.position;
                switch (i)
                {
                    case 0:
                        pos += new Vector3(0, 9.5f, 0);
                        break;
                    case 1:
                        pos += new Vector3(0, -9.5f, 0);
                        break;
                    case 2:
                        pos += new Vector3(9.5f, 0, 0);
                        break;
                    case 3:
                        pos += new Vector3(-9.3f, 0, 0);
                        break;
                    default: break;
                }
                newDoor.transform.position = pos;
                doors.Add((RoomEntrancePosition) i, doorScript);
            }
        }

        private void OnDestroy()
        {
        }
        #endregion

        #region Update Loop
        public override void Update()
        {
            //TODO: As enemy groups no longer update their members, this does nothing. But maybe it should?
        }
        #endregion Update Loop

        protected override void setDoorsOpened(bool enabled)
        {
            foreach (Door d in doors.Values)
            {
                d.setOpened(enabled);
            }
        }

        public override Vector3 getExitPosition(RoomEntrancePosition entranceDirection)
        {
            Door door;
            doors.TryGetValue(entranceDirection, out door);

            return transform.position + ((door.transform.position - transform.position) * 0.8f);
        }

        public void OnEventHandler(EnemyGroupDiedEvent e)
        {
            if (!master.Alive && !slave.Alive)
            {
                itemSpawn = e.deathPosition;
                EventAggregator.singleton.removeSubscription(this);
                EventAggregator.singleton.fire(new RoomClearedEvent(this, this));
                setDoorsOpened(true);
            }
        }

        public override Vector2[] getSpawnPositions()
        {
            Vector2[] positions = new Vector2[8];

            for (int i = 0; i < 8; i++)
            {
                positions[i] = spawnPositions[i];
            }

            return positions;
        }
    }
}
