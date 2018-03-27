using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTouchAttack : Enemy_Attack {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null)
        {
            Entity_Life life = collision.gameObject.GetComponent<Character_Life>();
            if (life != null)
            {
                life.dealDamage(damage);
            }
        }
    }
}
