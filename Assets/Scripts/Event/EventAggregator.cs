using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.EventSystem
{
    //public delegate void Action<T>(T payload) where T: IEvent;

    /// <summary>
    /// Responsible for Event Subscriptions and Notifications.
    /// </summary>
    public class EventAggregator
    {
        private class EventDictionary
        {
            private static class container<T> where T : Event
            {
                public static List<ISubscriber<T>> list = new List<ISubscriber<T>>();
                public static List<ISubscriber<T>> toRemove = new List<ISubscriber<T>>();
            }

            public void Add<T>(ISubscriber<T> add) where T : Event
            {
                container<T>.list.Add(add);
            }

            public void Remove<T>(ISubscriber<T> remove) where T : Event
            {
                container<T>.toRemove.Add(remove);
            }

            public IEnumerator<ISubscriber<T>> GetEnumerator<T>() where T : Event
            {
                container<T>.list.RemoveAll(x => container<T>.toRemove.Contains(x));
                container<T>.toRemove.RemoveAll(x => true);
                return container<T>.list.GetEnumerator();
            }
        }

        public static readonly EventAggregator singleton = new EventAggregator();

        private EventDictionary subscriberList = new EventDictionary();

        private EventAggregator() { }

        /// <summary>
        /// Adds the given ISubscriber as a subscriber for the given IEvent Type.
        /// Each subscriber should make sure to unsubscribe before the end of its lifetime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscriber"></param>
        public void subscribe<T>(ISubscriber<T> subscriber) where T : Event
        {
            subscriberList.Add(subscriber);
        }

        public static void Subscribe<T>(ISubscriber<T> subscriber) where T : Event
        {
            singleton.subscribe(subscriber);
        }

        /// <summary>
        /// Fires the given event, notifying all subscribers of it.
        /// </summary>
        /// <typeparam name="T">The IEvent Type the Event belongs to.</typeparam>
        /// <param name="eventObject"></param>
        public void fire<T>(T eventObject) where T : Event
        {
            IEnumerator<ISubscriber<T>> e = subscriberList.GetEnumerator<T>();
            while (e.MoveNext())
            {
                if (e.Current != null)
                {
                    e.Current.OnEventHandler(eventObject);
                }
                else
                {
                    removeSubscription(e.Current);
                }
            }
        }

        public static void Fire<T>(T eventObject) where T : Event
        {
            singleton.fire(eventObject);
        }

        /// <summary>
        /// Removes the given ISubscriber as a subscriber of the given IEvent Type.
        /// This should be called at least once before the end of a given ISubscribers lifetime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscriber"></param>
        public void removeSubscription<T>(ISubscriber<T> subscriber) where T : Event
        {
            subscriberList.Remove(subscriber);
        }

        public static void RemoveSubscription<T>(ISubscriber<T> subscriber) where T : Event
        {
            singleton.removeSubscription(subscriber);
        }
    }
}