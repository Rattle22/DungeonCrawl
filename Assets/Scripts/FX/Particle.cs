using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.FX
{
    public class Particle : MonoBehaviour
    {

        public bool done = false;

        protected virtual void Update()
        {
            if (done)
            {
                Destroy(gameObject);
            }
        }
    }
}