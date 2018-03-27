using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace RatStudios.Rooms
{
    [Serializable]
    internal class MissingRoomException : Exception
    {
        Vector2 missingRoomPosition;

        public MissingRoomException(Vector2 missingRoomPosition) : this("", missingRoomPosition)
        {
        }

        public MissingRoomException(string message, Vector2 missingRoomPosition) : base(message)
        {
            this.missingRoomPosition = missingRoomPosition;
        }

        public MissingRoomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingRoomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}