using RatStudios.Projectile;
using System.Collections.Generic;
using sys = System;
using UnityEngine;
using RatStudios.EventSystem;

namespace RatStudios.Items
{
    public class ItemGenerator
    {
        //TODO: Better way to load icons and sprite and everything
        private static readonly GameObject ExplosionAnimation = Resources.Load<GameObject>("Prototypes/FX/AnimatedExplosion");
        private static readonly GameObject BlazingObject = Resources.Load<GameObject>("Prototypes/EnviromentEffects/BlazingTile");
        private static readonly List<Sprite> icons = new List<Sprite>(new Sprite[] {
            Resources.Load<Sprite>("Sprites/Inline Sprites/SnakeIcon"),
            Resources.Load<Sprite>("Sprites/Inline Sprites/ExplosiveIcon"),
            Resources.Load<Sprite>("Sprites/Inline Sprites/BlazingIcon")
        });

        /*************************************************************
         * Every effect should have situations in which it is detrimental,
         * as well as situations in which it is straight up good
        **************************************************************/


        private List<ItemModifier> movementList = new List<ItemModifier>(new ItemModifier[] {
            //Sinusoid Motion
            new ItemModifier(0, delegate(ItemEssence e){
                e.AddFireballMovement(delegate(FireballBase fb){
                    fb.addMovement(delegate(float timeExisting, FireballBase fb2){
                        return new Vector2(0, Mathf.Sin(timeExisting * fb2.Speed / 5) * 0.25f);
                    });
                });
            }, icons[0], "X of the Snake")
            /*//Sinusoid Speed
            new ItemModifier(-2, delegate(ItemEssence e){
                e.AddFireballMovement(delegate(FireballBase fb){
                    fb.addMovement(delegate(float timeExisting, FireballBase fb2){
                        return new Vector2(-Mathf.Abs(Mathf.Sin(timeExisting) * timeExisting), 0);
                    });
                });
            }, "X of Hiccups"),
            new ItemModifier(1, delegate(ItemEssence e){
                e.AddFireballMovement(delegate(FireballBase fb){
                    fb.addMovement(delegate(float timeExisting, FireballBase fb2){
                        return new Vector2(((Mathf.Sin(timeExisting * 5) * 0.2f) - timeExisting), 0);
                    });
                });
            }, "Defender X")
        */});

        private List<ItemModifier> effectList = new List<ItemModifier>(new ItemModifier[] {
            //TODO: Are effects which are sometimes worse than no effect even possible?

            //AoE Effect
            new ItemModifier(1, delegate(ItemEssence e){
                e.AddFireballEffect(delegate(FireballBase fb){
                    fb.addOnDissipate(delegate(FireballBase fb2){
                        Vector2 position = new Vector2(fb2.transform.position.x, fb2.transform.position.y);
                        Collider2D[] hitList = Physics2D.OverlapCircleAll(fb2.transform.position, 1.5f);
                        foreach(Collider2D c in hitList){
                            Entity_Life target = c.GetComponent<Entity_Life>();
                            if (target != null){
                                fb2.damage(target);
                            }
                        }

                        GameObject.Instantiate(ExplosionAnimation, fb2.transform.position, fb2.transform.rotation);
                    });
                });
            }, icons[1], "Explosive X"),
            //The floor is FIRE, FIRE! BURN!
            new ItemModifier(1, delegate(ItemEssence e){
                //Using a movement allows timed spawns of the floor effect.
                e.AddFireballMovement(delegate(FireballBase fb){
                    float lastSpawn = 0;
                    fb.addMovement(delegate(float timeExisting, FireballBase fb2){
                        float delta = timeExisting - lastSpawn;
                        if (delta > 0.05f && timeExisting > 0.1f){
                            GameObject g = GameObject.Instantiate(BlazingObject, fb2.transform.position, fb2.transform.rotation);
                            Ember ember = g.GetComponent<Ember>();
                            ember.SetTrail(fb2);
                            ember.SetStrength(fb2.Power);
                            lastSpawn = timeExisting;
                        }
                        return Vector3.zero;
                    });
                });
                //Ensure that enemies hit also stand on the thing.
                e.AddFireballEffect(delegate(FireballBase fb){
                    fb.addOnDissipate(delegate(FireballBase fb2){
                            GameObject g = GameObject.Instantiate(BlazingObject, fb2.transform.position, fb2.transform.rotation);
                            Ember ember = g.GetComponent<Ember>();
                            ember.SetTrail(fb2);
                            ember.SetStrength(fb2.Power);
                    });
                }
            ); }, icons[2], "Amber X"),


            //Is this ever bad?
            //Knockback Effect TODO: Implement Proplerly
            /*new ItemModifier(1, delegate(ItemEssence e){
                e.addFireballModifier(delegate(FireballBase fb){
                    fb.addOnDamage(delegate(FireballBase fb2, Enemy_Life hit, int damage){
                        hit.transform.position += (hit.transform.position - fb2.transform.position).normalized * (5f * 25f / (25 + damage));
                    });
                });
            }, "Strong"),*/


            //Both of these are boring and, again, never bad as-is.
            //Health Leech
            /*new ItemModifier(1, delegate(ItemEssence e){
                e.AddFireballEffect(delegate(FireballBase fb){
                    fb.addOnDamage(delegate(FireballBase fb2, Enemy_Life hit, int amount){
                        Entity_Life sourceLife = fb2.Source.GetComponent<Entity_Life>();
                        if(sourceLife != null){
                            sourceLife.heal(amount);
                        }
                    });
                });
            }, "Vampiric X"),
            //Mana Leech
            /*new ItemModifier(1, delegate(ItemEssence e){
                e.addFireballModifier(delegate(FireballBase fb){
                    fb.addOnDamage(delegate(FireballBase fb2, Enemy_Life hit, int amount){
                        //TODO: Implement Mana Leech
                    });
                });
            }, "Draining")*/


            //Again, NEVER BAD
            //TODO: Re-add Flippin'
            //"Penetration"
            /*new ItemModifier(1, delegate(ItemEssence e){
                e.AddFireballEffect(delegate(FireballBase fb){
                    fb.addOnHit(delegate(FireballBase fb2, Enemy_Life hit){
                        if(hit.Dead){
                        fb2.overrideOnHitDestruction();
                        }
                    });
                });
            }, "Freudian X"),*/
        });

        public static Item generateGenericItem(Slot s, int power)
        {
            ItemEssence item = new ItemEssence();
            item.SetSlot(s);
            item.IncreasePrimaryStat(power);

            return item.createItem();
        }

        public Item generateItem(int itemLevel)
        {
            ItemEssence essence = new ItemEssence();

            //TODO: Pseudo Randomness

            int slot = (Mathf.RoundToInt((Random.value * sys.Enum.GetNames(typeof(Slot)).Length) + 0.5f)) - 1;
            essence.SetSlot((Slot)slot);

            int effectPower = (int)((Random.value * 0.3f + 0.1f) * itemLevel);
            ItemModifier effect = chooseEffectFrom(effectList, effectPower, 0.3f);
            if (effect != null)
            {
                itemLevel -= effect.price;
                //TODO: Should this not rather be in the definition of effects?
                essence.ApplyItemModifier(effect);
            }

            int movementPower = Mathf.RoundToInt((Random.value * 0.2f) * itemLevel);
            ItemModifier movement = chooseEffectFrom(movementList, movementPower, 0.3f);
            if (movement != null)
            {
                itemLevel -= movement.price;
                essence.ApplyItemModifier(movement);
            }

            /*int specialPower = Mathf.RoundToInt((Random.value * 0.3f + 0.2f) * itemLevel);
            int primarySpecialPower = Mathf.RoundToInt((Random.value * 5));
            int secondarySpecialPower = Mathf.RoundToInt((Random.value * 5));
            if (specialPower >= (Mathf.Pow(primarySpecialPower, 2)))
            {
                specialPower -= (int)(Mathf.Pow(primarySpecialPower, 2));
                if (specialPower >= (Mathf.Pow(primarySpecialPower, 2)))
                {
                    specialPower -= (int)(Mathf.Pow(primarySpecialPower, 2));
                }
            }
            itemLevel += specialPower;
            essence.IncreasePrimarySpecial(primarySpecialPower);
            essence.IncreaseSecondarySpecial(secondarySpecialPower);*/

            int primaryStat = Mathf.RoundToInt((Random.value * 0.3f + 0.3f) * itemLevel) + 1;
            int secondaryStat = Mathf.RoundToInt((Random.value * 0.3f + 0.3f) * itemLevel);

            essence.IncreasePrimaryStat(primaryStat);
            essence.IncreaseSecondaryStat(secondaryStat);
            return essence.createItem();
        }

        private ItemModifier chooseEffectFrom(List<ItemModifier> list, int effectPower, float failChance)
        {
            ItemModifier choice = null;

            if (effectPower == 0)
            {
                return choice;
            }

            if (Random.value > failChance)
            {
                int choiceNumber = Mathf.RoundToInt(Random.value * (list.Count - 1));
                ItemModifier potChoice = list[choiceNumber];
                if (potChoice.price > effectPower)
                {
                    potChoice = chooseEffectFrom(list, effectPower / 2, failChance);
                }
                else
                {
                    choice = potChoice;
                }
            }

            return choice;
        }
    }
}