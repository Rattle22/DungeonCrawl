using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RatStudios.EventSystem;

public class Enemy_Life : Entity_Life {

    protected override void onDeath()
    {
        //TODO: implement simple death animation for skulls
        EventAggregator.singleton.fire(new EnemyDiedEvent(this, this));
        Destroy(gameObject);
    }

    public override void Update()
    {
        base.Update();
    }
}
