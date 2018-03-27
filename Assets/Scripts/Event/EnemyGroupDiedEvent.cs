using RatStudios.Rooms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.EventSystem
{
    public class EnemyGroupDiedEvent : Event
    {
        public readonly EnemyGroup deadGroup;
        public readonly Vector2 deathPosition;

        public EnemyGroupDiedEvent(object actInvoker, EnemyGroup dying, Vector2 deathPosition) : base(actInvoker)
        {
            deadGroup = dying;
            this.deathPosition = deathPosition;
        }
    }
}