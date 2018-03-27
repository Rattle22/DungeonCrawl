using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child_Life : Enemy_Life {

    private Enemy_Life actualLife;

    private void Start()
    {
        actualLife = transform.parent.GetComponent<Enemy_Life>();
    }

    public override bool Dead
    {
        get
        {
            return actualLife.Dead;
        }
    }

    public override void setDefense(int defense)
    {
        actualLife.setDefense(defense);
    }

    public override void setLife(int life)
    {
        actualLife.setLife(life);
    }

    public override int dealDamage(int damage)
    {
        return actualLife.dealDamage(damage);
    }

    public override void heal(int amount)
    {
        actualLife.heal(amount);
    }

    public override void Update()
    {
        //ActualLife already updates itself
        return;
    }
}
