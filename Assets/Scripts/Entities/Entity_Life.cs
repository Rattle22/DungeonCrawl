using RatStudios.EventSystem;
using RatStudios.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity_Life : MonoBehaviour
{
    protected int maxLife = 10;
    [SerializeField]
    protected int life = 10;
    [SerializeField]
    protected bool damageAble = true;
    protected int defense = 0;
    protected bool isDying = false;
    public virtual bool Dead {
        get { return isDying || life <= 0; }
    }

    public virtual void setLife(int life)
    {
        this.life = life;
    }

    public virtual void setDefense(int defense)
    {
        this.defense = defense;
    }

    /// <summary>
    /// Attempts to deal the given amount of damage to the Entity.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>The amount of damage actually done.</returns>
    public virtual int dealDamage(int damage)
    {
        damage -= defense;

        if (damage <= 0) {
            damage = 1;
        }

        if (!damageAble) {
            damage = 0;
        }

        life -= damage;

        if (life <= 0)
        {
            isDying = true;
        }

        DamageTakenEvent e = new DamageTakenEvent(this, this, damage);
        EventAggregator.singleton.fire<DamageTakenEvent>(e);
        //TODO: Should damage display call be in the dealDamage function of entities?
        FloatingDamageDisplay.ShowDamage(-damage, gameObject, 0.25f);
        return damage;
    }

    public virtual void heal(int amount)
    {
        life += amount;
        life = life > maxLife ? maxLife : life;
    }

    public virtual void Update()
    {
        if (Dead)
        {
            onDeath();
        }
    }

    protected abstract void onDeath();
}