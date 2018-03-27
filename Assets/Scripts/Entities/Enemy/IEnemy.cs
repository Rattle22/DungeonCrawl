using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.Entities.Enemies
{
    public interface IEnemy
    {

        /// <summary>
        /// Updates this enemy. This includes adjusting its movement, launching an attack and similar.
        /// This is called automatically on MonoBehaviours.
        /// </summary>
        void Update();

        /// <summary>
        /// Sets which GameObject this Entity attempts to target.
        /// </summary>
        /// <param name="target">The gameObject to Target.</param>
        void setTarget(GameObject target);

        /// <summary>
        /// Sets the strength of this enemy, determining the abilities and the numerical power of it.
        /// </summary>
        /// <param name="strength">The strength. Entity is guaranteed to have meaningful stats for all values, with higher values representing more power.</param>
        void setStrength(float strength);

        /// <summary>
        /// Sets the size modifier of this entity. This influences mostly health and attack power, but can also have other numerical effects.
        /// </summary>
        /// <param name="size">The size as a positive integer between 1 and 12.</param>
        void setSizeod(int size);

        /// <summary>
        /// Sets the NavigationMesh for this entity to use.
        /// </summary>
        /// <param name="mesh"></param>
        void setNavMesh(NavigationMesh mesh);
    }
}