using System;
using System.Collections.Generic;
using RatStudios.Projectile;
using UnityEngine;

namespace RatStudios.Items
{
    public delegate void itemModifier(ItemEssence stats);
    public class ItemModifier
    {
        public readonly int price;
        public readonly itemModifier modify;
        public readonly string nameModifier;
        public readonly Sprite Icon;

        public ItemModifier(int price, itemModifier modifier, Sprite icon, string nameModifier)
        {
            this.price = price;
            modify = modifier;
            Icon = icon;
            this.nameModifier = nameModifier;
        }
    }
    public class ItemEssence
    {
        private Slot slot;
        private string baseName;
        private string nameModifier = "X";

        private int primaryStat = 0;
        private int secondaryStat = 0;

        private int primarySpecial = 0;
        private int secondarySpecial = 0;

        private List<FireballModifierDelegate> fireballEffectList = new List<FireballModifierDelegate>();
        private List<FireballModifierDelegate> fireballMovementList = new List<FireballModifierDelegate>();
        private List<Sprite> effectIcons = new List<Sprite>();
        public void SetSlot(Slot s)
        {
            slot = s;
            baseName = s.ToString();
        }

        public void IncreasePrimaryStat(int increase)
        {
            primaryStat += increase;
        }

        public void IncreaseSecondaryStat(int increase)
        {
            secondaryStat += increase;
        }

        public void IncreasePrimarySpecial(int increase)
        {
            primarySpecial += increase;
        }

        public void IncreaseSecondarySpecial(int increase)
        {
            secondarySpecial += increase;
        }

        public void AddFireballEffect(FireballModifierDelegate fm)
        {
            fireballEffectList.Add(fm);
        }

        public void AddFireballMovement(FireballModifierDelegate fm)
        {
            fireballMovementList.Add(fm);
        }

        public void ApplyItemModifier(ItemModifier modifier)
        {
            modifier.modify(this);
            effectIcons.Add(modifier.Icon);
            nameModifier = ApplyName(nameModifier, modifier.nameModifier);
        }

        /// <summary>
        /// Applies the given Item name modifier to the current name.
        /// <br>
        /// The modifier is to be encoded in the form "modifier X modifier", where X will be replaced with the current name of the Item.
        /// </summary>
        /// <param name="base"></param>
        public static string ApplyName(string @base, string extra)
        {
            string[] parts = extra.Split(new Char[] { 'X' }, StringSplitOptions.None);
            if (parts.Length != 2)
            {
                throw new Exception("Unsupported Item Name Modifier format: " + @base);
            }

            @base = parts[0] + @base + parts[1];
            return @base;
        }

        public Item createItem()
        {
            ItemBase item;
            switch (slot)
            {
                case Slot.Boots:
                    item = new ItemBase(ApplyName(baseName, nameModifier), slot, fireballEffectList, fireballMovementList, effectIcons, def: primaryStat, dmg: secondaryStat, mov: primarySpecial, cdr: secondarySpecial);
                    break;
                case Slot.Staff:
                    item = new ItemBase(ApplyName(baseName, nameModifier), slot, fireballEffectList, fireballMovementList, effectIcons, dmg: primaryStat, hp: secondaryStat, cdr: primarySpecial, mReg: secondarySpecial);
                    break;
                default:
                case Slot.Cowl:
                    item = new ItemBase(ApplyName(baseName, nameModifier), slot, fireballEffectList, fireballMovementList, effectIcons, hp: primaryStat, def: secondaryStat, mReg: primarySpecial, mov: secondarySpecial);
                    break;
            }
            return item;
        }

        public void setName(String newName)
        {
            baseName = newName;
        }
    }
}