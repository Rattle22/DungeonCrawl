using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.FX
{
    public abstract class FadingParticle : Particle
    {

        protected override void Update() {
            done = fade();
            base.Update();
        }

        protected abstract bool fade();
    }
}