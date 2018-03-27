using System;
using System.Collections.Generic;
using UnityEngine;
using RatStudios.EventSystem;
using RatStudios.Entities.Enemies;

namespace RatStudios.Rooms
{
    public class EnemyGroup : ISubscriber<EnemyDiedEvent>
    {
        private GameObject enemyProto;

        private float strength;
        private int size;
        private Vector2[] spawns;

        private GameObject target;

        public GameObject Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                foreach (Entities.Enemies.IEnemy e in enemyList) {
                    e.setTarget(value);
                }
            }
        }

        private List<IEnemy> enemyList = new List<IEnemy>();

        public bool Alive
        {
            get
            {
                return enemyList.Count != 0;
            }
        }

        public EnemyGroup(GameObject target, Vector2[] actSpawns, float actStrength, GameObject enemyType, int actSize)
        {
            Target = target;

            strength = actStrength;
            enemyProto = enemyType;
            size = actSize;
            spawns = actSpawns;

            EventAggregator.singleton.subscribe(this);
        }

        //TODO: Add constraints to spawning
        public void spawn(Vector3 center)
        {
            //Using Gompertz-function for movement speed.
            //Base Speed
            float min = 1.5f;
            //Maximum value of function
            double a = 2.5f;
            //x delta
            double b = 1;
            //Growth
            double c = 0.05;
            float compertz = (float)(a * Math.Exp(-b * Math.Exp(-c * strength))) + min;

            for (int i = 0; i < (16 / size); i++)
            {
                var newEnemy = GameObject.Instantiate(enemyProto);
                var pos = spawns[i];
                newEnemy.transform.position = ((Vector3) pos) + center;

                try
                {
                    //TODO: This is bs, change this
                    newEnemy.GetComponent<Enemy_Movement>().setSpeed(compertz);
                } catch (Exception e) {
                }
                newEnemy.GetComponent<Enemy_Life>().setLife(10 + (size/2));
                newEnemy.GetComponent<Enemy_Life>().setDefense((int)strength);
                //TODO: The damage scales too fast early. This is unfun, as early mistakes are punished hard if no defense items are found.
                newEnemy.GetComponent<Enemy_Attack>().setDamage((int) (strength));
                newEnemy.GetComponent<IEnemy>().setTarget(target);
                enemyList.Add(newEnemy.GetComponent<IEnemy>());
            }
        }

        //TODO: Maybe move enemy update to enemy group again?

        public void OnEventHandler(EnemyDiedEvent e)
        {
            Entity_Life damageTaker = e.dead;
            if (damageTaker is Enemy_Life)
            {
                Enemy_Life enemy = (Enemy_Life)damageTaker;
                if (enemyList.Contains(enemy.GetComponent<IEnemy>()))
                {
                    enemyList.Remove(enemy.GetComponent<IEnemy>());
                    if (enemyList.Count == 0)
                    {
                        EventAggregator.singleton.removeSubscription<EnemyDiedEvent>(this);
                        EventAggregator.singleton.fire(new EnemyGroupDiedEvent(this, this, enemy.transform.position));
                    }
                }
            }
        }
    }
}