using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Basics")]
        public Sprite itemIcon;
        public string itemName;
        public ItemType itemType;
        public ExhaustionType exhaustionType;
        public string itemDescription;
        public int itemPrice;
        public bool b_canBeBought = true; // Upgrades can be bought only once

        [Header("Item Effects")]
        [SerializeReference, SubclassSelector]
        // SubclassSelector (comes from the SerializeReferenceExtensions package=
        // It is here to display a dropdown menu in the inspector
        // In this dropdown, you can select the children of ItemEffect
        public List<ItemEffect> itemEffects = new List<ItemEffect>();


        /// <summary>
        /// Apply the effects linked defined in itemEffects
        /// </summary>
        /// <param name="reference"> The exhaustion manager</param>
        public void UseItem(GameObject reference)
        {
            foreach(ItemEffect effect in itemEffects)
            {
                effect?.UseItemEffect(reference);
            }
        }
    }
}
