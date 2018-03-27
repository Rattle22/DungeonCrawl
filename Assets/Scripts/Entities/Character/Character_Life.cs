using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RatStudios.EventSystem;
using RatStudios.UI;

public class Character_Life : Entity_Life, ISubscriber<ItemEquippedEvent>, ISubscriber<ItemUnequippedEvent>
{
    private bool rcDmg = false;
    [SerializeField]
    private float invulnerabilityCd = 0;
    private bool died = false;

    public bool recentlyDamaged
    {
        get
        {
            return rcDmg;
        }
    }

    protected override void onDeath()
    {
        if (!died)
        {
            died = true;
            EventAggregator.singleton.fire(new PlayerDeathEvent(this));
        }
    }

    // Use this for initialization
    void Start()
    {
        setLife(10);
        EventAggregator.singleton.subscribe<ItemEquippedEvent>(this);
        EventAggregator.singleton.subscribe<ItemUnequippedEvent>(this);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        HealthBar.showHealth(maxLife, life);

        if (invulnerabilityCd > 0) {
            invulnerabilityCd -= Time.deltaTime;
        } else if (rcDmg) {
            rcDmg = false;
        }
    }

    public override int dealDamage(int damage)
    {
        if (!recentlyDamaged) {
            int actDamage = base.dealDamage(damage);

            if (actDamage > 0) {
                rcDmg = true;
                invulnerabilityCd = 1;
                return actDamage;
            }
        }
        return 0;
    }

    public void OnEventHandler(ItemEquippedEvent e)
    {
        maxLife += e.equippedItem.GetHealth();
        heal(e.equippedItem.GetHealth());
        defense += e.equippedItem.GetDefense();
    }

    public void OnEventHandler(ItemUnequippedEvent e)
    { 
        maxLife -= e.unequippedItem.GetHealth();
        //Heal negative to prevent double punishment for healing debuffs
        heal(-e.unequippedItem.GetHealth());
        defense -= e.unequippedItem.GetDefense();
    }
}
