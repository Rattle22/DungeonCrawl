using RatStudios.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTurret : EnemyBase
{
    [SerializeField]
    private float degreePerSecond;
    [SerializeField]
    private int cd;
    [SerializeField]
    private int delay;
    [SerializeField]
    private GameObject LineRenderPrefab;

    private int actCd = 0;
    private bool isShooting = false;
    private int actDelay = 0;

    private Transform baseTransform;
    private Enemy_Attack att;
    private SpriteRenderer firedSpriteRenderer;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        mov = GetComponent<Enemy_Movement>();
        att = GetComponent<Enemy_Attack>();
        firedSpriteRenderer = transform.Find("Cannon_Fired").GetComponent<SpriteRenderer>();
        baseTransform = transform.Find("CannonBase");
    }

    public override void Update()
    {
        base.Update();

        //Change the overlapping sprite that indicates the cooldown.
        Color color = firedSpriteRenderer.material.color;
        float ratio = ((float)actCd) / cd;

        color.a = ratio;
        firedSpriteRenderer.material.color = color;
    }

    protected override void move()
    {
        if (!isShooting)
        {
            Vector2 turnTo = targetDirection();
            mov.rotate(turnTo, degreePerSecond / 60);
            baseTransform.rotation = Quaternion.AngleAxis(0, Vector3.right);
        }
    }

    protected override void attack()
    {
        if (isShooting)
        {
            if (actDelay <= 0)
            {
                isShooting = false;
                //Hit all things, Fireballs are destroyed
                int layerMask = 1 << 14;
                layerMask = ~layerMask;
                RaycastHit2D[] rayHits = shootAll(layerMask);

                bool hitFireball = true;
                int iterations = 0;
                while (hitFireball && iterations < rayHits.Length)
                {
                    //Assume no fireball has been hit. Only if a fireball has been hit, this is set to true.
                    hitFireball = false;
                    RaycastHit2D currentHit = rayHits[iterations];

                    if (currentHit.collider.gameObject == gameObject) {
                        continue;
                    }

                    Entity_Life life = currentHit.collider.GetComponent<Entity_Life>();
                    FireballBase fireball = currentHit.collider.GetComponent<FireballBase>();
                    if (life != null)
                    {
                        print("Hit damageable thing!");
                        life.dealDamage(att.Damage);
                    }
                    else if (fireball != null)
                    {
                        print("Hit Fireball! Iteration: " + iterations);
                        Destroy(currentHit.collider.gameObject);
                        hitFireball = true;
                    }

                    if (!hitFireball)
                    {
                        GameObject line = Instantiate(LineRenderPrefab);
                        line.GetComponent<LineRenderer>().SetPositions(new Vector3[] { transform.position, currentHit.point });
                    }

                    //To prevent infinite Loops
                    iterations++;
                }
                actCd = cd;
            }
            else
            {
                actDelay--;
            }
        }
        else
        {
            if (actCd <= 0)
            {
                Vector3 direction = mov.transform.TransformVector(Vector2.right);

                //Ignore the projectile Layer
                int layerMask = (1 << 11) | (1 << 14);
                layerMask = ~layerMask;

                RaycastHit2D rayHit = shoot(layerMask);
                if (rayHit.collider != null)
                {
                    print(rayHit.collider.gameObject);
                    Character_Life life = rayHit.collider.GetComponent<Character_Life>();
                    if (life != null)
                    {
                        isShooting = true;
                        actDelay = delay;
                    }
                }
            }
            else
            {
                actCd--;
            }
        }
    }

    private RaycastHit2D shoot(LayerMask mask)
    {
        RaycastHit2D[] hits = shootAll(mask);
        return hits[0];
    }

    private RaycastHit2D[] shootAll(LayerMask mask)
    {
        Vector3 direction = mov.transform.TransformVector(Vector2.right);
        return Physics2D.RaycastAll(transform.position + 0.578125f * direction, direction, Mathf.Infinity, mask);
    }
}