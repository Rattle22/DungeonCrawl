using RatStudios.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.EventSystem
{
    public class FireballCreatedEvent : Event
    {
        public readonly FireballBase createdFireball;

        public FireballCreatedEvent(object actInvoker, GameObject createdFireball) : base(actInvoker){
            FireballBase script = createdFireball.GetComponent<FireballBase>();
            if (script == null) {
                throw new MissingComponentException();
            }
            this.createdFireball = script;
        }
    }
}