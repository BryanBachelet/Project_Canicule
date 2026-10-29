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
        public string itemDescription;
        public int itemPrice;

        [Header("Item Effects")]
        [SerializeReference, SubclassSelector] 
        // SubclassSelector is here to display a dropdown menu in the inspector
        // In this dropdown, you can select the children of ItemEffect
        public List<ItemEffect> itemEffects = new List<ItemEffect>();


        /// <summary>
        /// Apply the effects linked definied in itemEffects
        /// </summary>
        /// <param name="reference"> Usually the exhaustion manager</param>
        public void UseItem(GameObject reference)
        {
            foreach(ItemEffect effect in itemEffects)
            {
                effect?.UseItemEffect(reference);
            }
        }
    }
}
