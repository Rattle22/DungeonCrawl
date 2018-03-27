using RatStudios.Entities.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : EnemyBase
{
    protected override void attack()
    {
        //TODO: This is currently done by Enemy_Attack
    }

    protected override void move()
    {
        mov.move(targetDirection());
    }
}
