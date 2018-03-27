using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.EventSystem
{
    public class EnemyDiedEvent : Event
    {
        public readonly Entity_Life dead;

        public EnemyDiedEvent(object actInvoker, Entity_Life ded) : base(actInvoker)
        {
            dead = ded;
        }
    }
}