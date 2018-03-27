using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.EventSystem
{
    /// <summary>
    /// Base class for Event types.
    /// </summary>
    public abstract class Event
    {
        /// <summary>
        /// Whoever <i>fired</i> the event. This is not equivalent to who is the subject of the event!
        /// </summary>
        public readonly object invoker;

        public Event(object actInvoker)
        {
            invoker = actInvoker;
        }
    }
}
