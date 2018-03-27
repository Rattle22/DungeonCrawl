using RatStudios.EventSystem;
using RatStudios.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Fire : MonoBehaviour{

    public GameObject fireball;
    public int fireballCd;
    public int fireballLockout;

    private int actFireballCd = 0;
    private int actFireballLockout = 0;
    private int timeSinceLastShot = 0;

    private void Update()
    {
        if (actFireballCd > 0) {
            actFireballCd--;
        } else if (actFireballLockout > 0) {
            actFireballLockout--;
        }
        timeSinceLastShot++;
    }

    public void fire() {
        if (actFireballCd <= 0) {
            //The combination of lockout and cd smooths out the dps, as missing the exact frame that the cd comes back up simply reduces the next cd.
            actFireballCd = fireballCd + actFireballLockout;
            actFireballLockout = fireballLockout;
            timeSinceLastShot = 0;

            var newFireball = GameObject.Instantiate(fireball);
            newFireball.transform.position = transform.position;
            newFireball.transform.rotation = transform.rotation;
            newFireball.GetComponent<FireballBase>().Source = gameObject;

            EventAggregator.Fire(new FireballCreatedEvent(this, newFireball));
        }
    }
}
