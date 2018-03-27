using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.EventSystem {
    public interface ISubscriber<TEventType> where TEventType: Event{

        void OnEventHandler(TEventType e);
    }
}
