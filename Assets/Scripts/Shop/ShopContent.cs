using System.Collections.Generic;
using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "ShopContent", menuName = "Scriptable Objects/ShopContent")]
    public class ShopContent : ScriptableObject
    {
        public List<ShopCategory> shopCategories;
        
        public ShopContent Clone()
        {
            ShopContent instance = ScriptableObject.CreateInstance<ShopContent>();

            instance.name = this.name + "(clone)";

            instance.shopCategories = new List<ShopCategory>();

            // Constructs categories for the instance
            for(int i = 0; i < shopCategories.Count; i++)
            {
                instance.shopCategories.Add(new ShopCategory());
                instance.shopCategories[i].type = shopCategories[i].type;
                instance.shopCategories[i].items = new List<ItemData>();

                // Add cloned items in instance lists
                foreach(ItemData item in shopCategories[i].items)
                {
                    instance.shopCategories[i].items.Add(item.Clone());
                }
            }

            return instance;
        }
    }
}
