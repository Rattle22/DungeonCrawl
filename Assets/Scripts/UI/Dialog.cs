using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.UI
{
    public class Dialog<T> : DisplayBase<T> where T :  Dialog<T>, new()
    {

        protected static List<object> interrupts = new List<object>();

        public Dialog()
        {
        }

        protected static void addInterrupt(object invoker)
        {
            interrupts.Add(invoker);
            if (interrupts.Count != 0)
            {
                Time.timeScale = 0;
            }
        }

        protected static void removeInterrupt(object invoker)
        {
            interrupts.Remove(invoker);
            if (interrupts.Count == 0)
            {
                Time.timeScale = 1;
            }
        }
    }
}