using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatStudios.Items
{
    /// <summary>
    /// An item provides methods for retrieving augmentations for fireballs.
    /// </summary>
    public interface Item
    {
        string GetName();
        List<Sprite> GetEffectIcons();
        /// <summary>
        /// Returns the slot this Item belongs to. This alwas returns the same slot, no matter the circumstances.
        /// </summary>
        /// <returns></returns>
        Slot GetSlot();
        /// <summary>
        /// Returns a sprite representing this Item.
        /// </summary>
        /// <returns></returns>
        Sprite GetSprite();

        int GetDamage();
        int GetHealth();
        int GetDefense();

        int GetMovSpeed();
        int GetManaReg();
        int GetCdr();
        void SetEnabled(bool enabled);
    }
}
