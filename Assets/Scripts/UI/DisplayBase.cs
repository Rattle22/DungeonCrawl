using UnityEngine;

namespace RatStudios.UI
{
    public abstract class DisplayBase<T> : MonoBehaviour
    where T : DisplayBase<T>, new()
    {
        protected static T singleton;
        public static T Singleton
        {
            get
            {
                return singleton;
            }
        }
    }
}