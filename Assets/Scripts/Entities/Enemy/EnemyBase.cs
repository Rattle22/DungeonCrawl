using RatStudios.Entities.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemy {

    protected GameObject target;
    protected Enemy_Movement mov;

    protected abstract void attack();
    protected abstract void move();
    
    public void setTarget(GameObject target)
    {
        this.target = target;
    }

    public virtual void Start() {
        mov = GetComponent<Enemy_Movement>();
    }

    public virtual void Update()
    {
        move();
        attack();
    }
    
    protected Vector2 targetDirection()
    {
        return target.transform.position - transform.position;
    }

    public void setStrength(float strength)
    {
        throw new System.NotImplementedException();
    }

    public void setSizeod(int size)
    {
        throw new System.NotImplementedException();
    }

    public void setNavMesh(NavigationMesh mesh)
    {
        throw new System.NotImplementedException();
    }
}
