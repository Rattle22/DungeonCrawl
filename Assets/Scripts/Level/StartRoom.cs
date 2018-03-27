using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.Rooms
{
    public class StartRoom : Room
    {
        public GameObject door;

        public override Vector3 getExitPosition(RoomEntrancePosition entranceDirection)
        {
            throw new NotImplementedException();
        }

        public override Vector2[] getSpawnPositions()
        {
            throw new NotImplementedException();
        }

        protected override void setDoorsOpened(bool enabled)
        {
            door.GetComponent<Door>().setOpened(true);

        }

        private void Start()
        {
            setDoorsOpened(true);
        }
    }
}