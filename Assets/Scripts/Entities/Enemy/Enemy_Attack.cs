using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the attack value of an enemy.
/// </summary>
public class Enemy_Attack : MonoBehaviour {

    protected int damage = 1;

    public int Damage {
        get { return damage; }
    }

    public void setDamage(int damage) {
        this.damage = damage;
    }
}
