using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.FX
{
    [RequireComponent(typeof(LineRenderer))]
    public class FadingLine : FadingParticle
    {
        public float duration;
        public float fadeDelayInPercent;
        private float exists = 0;

        protected override bool fade()
        {
            exists++;

            //Keep it fully visible for the specified duration
            if (exists >= duration * fadeDelayInPercent) {
                Color color = GetComponent<LineRenderer>().material.color;

                //As we want a ratio that goes from 0 to 1, we reduce the ratio by the delay.
                float ratio = exists - (duration * fadeDelayInPercent);
                //Only divide by the remaining duration.
                ratio = ratio / (duration * (1 - fadeDelayInPercent));
                //To smoothly fade from all to nothing, Cosinus (1 at 0, 0 at PI)
                //ratio = Mathf.Cos(ratio * Mathf.PI);

                color.a = Mathf.Cos(ratio);
                GetComponent<LineRenderer>().material.color = color;
            }

            return exists >= duration;
        }
    }
}