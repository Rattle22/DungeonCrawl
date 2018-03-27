using RatStudios.EventSystem;
using RatStudios.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.Items {
    public class ItemBase : Item, ISubscriber<FireballCreatedEvent>
    {
        /* TODO:
         * Items have Slots
         * Items modify damage and fly pattern of fireballs
         * Items grant movement abilities
         * Name is based on Attributes
         */

        private Slot slot;
        private bool enabled;
        private string name;

        #region Scaling values
        private int damage;
        private int health;
        private int defense;
        #endregion

        #region Flat values
        private int movSpdPercent;
        private int manaReg;
        private int cdr;
        #endregion

        private List<Sprite> effectIcons;

        private List<FireballModifierDelegate> effects;
        private List<FireballModifierDelegate> movements;

        public void SetEnabled(bool nowEnabled)
        {
            enabled = nowEnabled;
            if (enabled)
            {
                EventAggregator.Subscribe(this);
            }
            else {
                EventAggregator.RemoveSubscription(this);
            }
        }

        public string GetName()
        {
            return name;
        }

        public List<Sprite> GetEffectIcons()
        {
            return effectIcons;
        }

        public Slot GetSlot()
        {
            return slot;
        }

        public int GetDamage()
        {
            return damage;
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetDefense()
        {
            return defense;
        }

        public int GetMovSpeed()
        {
            return movSpdPercent;
        }

        public int GetManaReg()
        {
            return manaReg;
        }

        public int GetCdr()
        {
            return cdr;
        }

        public void OnEventHandler(FireballCreatedEvent e)
        {
            e.createdFireball.Power += damage;
            foreach (FireballModifierDelegate fm in effects)
            {
                fm(e.createdFireball);
            }
            foreach (FireballModifierDelegate fm in movements)
            {
                fm(e.createdFireball);
            }
        }

        public Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/" + slot.ToString());
        }

        public ItemBase(string name, Slot slot, List<FireballModifierDelegate> fireballEffectList, List<FireballModifierDelegate> fireballMovementList, List<Sprite> effectIcons, int hp = 0, int def = 0, int dmg = 0, int mov = 0, int mReg = 0, int cdr = 0)
        {
            this.name = name;
            this.slot = slot;
            effects = fireballEffectList;
            movements = fireballMovementList;
            this.effectIcons = effectIcons;

            damage = dmg;
            health = hp;
            defense = def;

            movSpdPercent = mov;
            manaReg = mReg;
            this.cdr = cdr;

            enabled = false;
        }

        public override string ToString()
        {
            string slotString = name + "\n";
            string additionalInfo = "";

            Action addLinebreak = () =>
            {
                if (!additionalInfo.Equals(""))
                {
                    additionalInfo += "\n";
                }
            };

            if (damage > 0)
            {
                addLinebreak();
                additionalInfo += "Damage: " + damage;
            }

            if (health > 0)
            {
                addLinebreak();
                additionalInfo += "Health: " + health;
            }

            if (defense > 0)
            {
                addLinebreak();
                additionalInfo += "Defense: " + defense;
            }

            if (movSpdPercent > 0)
            {
                addLinebreak();
                additionalInfo += "Movement Speed: " + movSpdPercent + "%";
            }

            if (manaReg > 0)
            {
                addLinebreak();
                additionalInfo += "Mana Regeneration: " + manaReg;
            }

            if (cdr > 0)
            {
                addLinebreak();
                additionalInfo += "Cooldown Reduction: " + cdr + "%";
            }

            if (effects.Count > 0) {
                addLinebreak();
                additionalInfo += "Modifies Fireball Effects!";
            }

            if (movements.Count > 0)
            {
                addLinebreak();
                additionalInfo += "Modifies Fireball Movement!";
            }

            return slotString + additionalInfo;
        }
    }
}