using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.EventSystem
{
    public class DamageTakenEvent : Event
    {
        //TODO: Allow subscriber to modify amount?
        public readonly int amount;
        public readonly Entity_Life taker;

        public DamageTakenEvent(object invoker, Entity_Life damageTaker, int damageTaken) : base(invoker)
        {
            amount = damageTaken;
            taker = damageTaker;
        }
    }
}