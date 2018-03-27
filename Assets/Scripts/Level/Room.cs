using RatStudios.EventSystem;
using RatStudios.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.Rooms
{
    public abstract class Room : MonoBehaviour
    {
        protected int id;
        public int ID
        {
            get { return id; }
        }

        private GameObject itemCarrier;
        protected Vector2 itemSpawn = Vector2.zero;

        private void Start()
        {
            itemCarrier = Resources.Load<GameObject>("Prototypes/Entity Objects/ItemCarrier");
        }
        
        protected abstract void setDoorsOpened(bool enabled);
        public abstract Vector3 getExitPosition(RoomEntrancePosition entranceDirection);

        /// <summary>
        /// Returns a list of positions within the room bounds at which enemies may spawn.
        /// 
        /// TODO: Make sure (somehow) that there are always enough spawn positions. How? no idea
        /// </summary>
        /// <returns></returns>
        public abstract Vector2[] getSpawnPositions();

        #region Update Loop
        public virtual void Update() { }
        #endregion

        public virtual void spawnItem(Item itemToSpawn) {
            GameObject newItemCarrier = Instantiate(itemCarrier);
            ItemCarrier carrierScript = newItemCarrier.GetComponent<ItemCarrier>();
            carrierScript.setItem(itemToSpawn);

            newItemCarrier.transform.position = itemSpawn;
        }
    }
}