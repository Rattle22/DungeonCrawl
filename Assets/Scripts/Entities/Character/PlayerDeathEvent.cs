using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.EventSystem
{
    public class PlayerDeathEvent : Event
    {
        public PlayerDeathEvent(object actInvoker) : base(actInvoker)
        {
        }
    }
}