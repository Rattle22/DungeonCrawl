using RatStudios.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ember : MonoBehaviour {

    private static Dictionary<FireballBase, Trail> trailPerFireball = new Dictionary<FireballBase, Trail>();

    private class Trail
    {
        private Dictionary<Entity_Life, int> lastDamageFrame = new Dictionary<Entity_Life, int>();

        private int strength;
        public int Strength {
            get { return strength; }
            set { strength = value; }
        }

        public void TickOn(Entity_Life target, int frame) {
            int lastFrame;
            if (!lastDamageFrame.TryGetValue(target, out lastFrame))
            {
                lastDamageFrame.Add(target, frame - 60);
            }
            if (frame - lastFrame >= 60)
            {
                target.dealDamage(strength);
                lastDamageFrame.Remove(target);
                lastDamageFrame.Add(target, frame);
            }
        }
    }

    private Trail trail;
    private List<Entity_Life> touchers = new List<Entity_Life>();
    private float alive = 0;

	void Update () {
        alive += Time.deltaTime;
        if (alive > 5) {
            Destroy(gameObject);
        }
        foreach (Entity_Life toucher in touchers) {
            if (toucher != null) {
                trail.TickOn(toucher, Time.frameCount);
            }
        }
	}

    public void SetTrail(FireballBase owner) {
        Trail actTrail;
        if (trailPerFireball.TryGetValue(owner, out actTrail))
        {
            trail = actTrail;
        }
        else
        {
            trail = new Trail();
            trailPerFireball.Add(owner, trail);
        }
    }

    public void SetStrength(int amount) {
        trail.Strength = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Entering!");
        Entity_Life life = collision.gameObject.GetComponent<Entity_Life>();
        if (life != null)
        {
            if (!touchers.Contains(life))
            {
                touchers.Add(life);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("Leaving!");
        Entity_Life life = collision.gameObject.GetComponent<Entity_Life>();
        if (life != null)
        {
            if (touchers.Contains(life))
            {
                touchers.Remove(life);
            }
        }

    }
}
