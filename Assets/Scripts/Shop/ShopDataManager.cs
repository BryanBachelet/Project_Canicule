using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Canicule
{
    public class ShopDataManager : MonoBehaviour
    {
        [SerializeField] private ShopContent m_shopContent;
        private List<ShopCategory> m_shopCategories;
        private InventoryDataManager m_inventoryDataManager;
        private Dictionary<ItemData, int> m_itemCart;
        private float m_totalCartPrice;

        #region Unity Methods
        private void Awake()
        {
            m_inventoryDataManager = FindAnyObjectByType<InventoryDataManager>();
        }
        void Start()
        {
            // Reset values
            m_totalCartPrice = 0;

            // This will erase shop updates! Change this!
            m_shopCategories = m_shopContent.m_shopCategories;
        }
        #endregion

        #region Shop Data Management Methods
        public void AddItemInCart(ItemData item)
        {
            bool b_hasKey = m_itemCart.ContainsKey(item);

            if (b_hasKey)
            {
                // Increase the value of the key item
                m_itemCart[item] += 1;
            }
            else
            {
                // Create a new key in the dict m_itemCart
                m_itemCart.Add(item, 1);
            }

            // Update player balance
            m_inventoryDataManager.SetPlayerBalance(m_inventoryDataManager.GetPlayerBalance() - item.itemPrice);
        }

        public void RemoveItemFromCart(ItemData item)
        {
            m_itemCart[item] -= 1;
            
            // Remove the item from the cart dict if there is no more item in it
            if (m_itemCart[item] == 0)
            {
                m_itemCart.Remove(item);
            }

            // Update player balance
            m_inventoryDataManager.SetPlayerBalance(m_inventoryDataManager.GetPlayerBalance() + item.itemPrice);
        }

        public float ComputeCartTotalPrice(Dictionary<ItemData, int> itemCart)
        {
            // Runs through the dict and add the price of each item
            foreach (KeyValuePair<ItemData, int> item in m_itemCart)
            {
                m_totalCartPrice += item.Key.itemPrice * item.Value;
            }

            return m_totalCartPrice;
        }

        // This method is used when buying an upgrade
        // An upgrade can't be purchased twice
        public void RemoveItemFromShop(ItemData item)
        {
            // Find the item in the shop categories
            foreach(ShopCategory shopCategory in m_shopCategories)
            {
                // Check if the type matches
                if(shopCategory.type == item.exhaustionType)
                {
                    foreach(ItemData itemData in shopCategory.items)
                    {
                        // Check if it's the same item
                        if(itemData == item)
                        {
                            itemData.b_canBeBought = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region Getter
        public float GetTotalCartPrice()
        {
            return m_totalCartPrice;
        }

        /// <summary>
        /// Returns the list of items associated with the exhaustion type
        /// </summary>
        /// <param name="type">Type of the list of items</param>
        /// <returns></returns>
        public List<ItemData> GetShopItemsByType(ExhaustionType type)
        {
            List<ItemData> items = new List<ItemData>();

            foreach(ShopCategory category in m_shopCategories)
            {
                if(category.type == type)
                {
                    return category.items;
                }
            }

            return null;
        }
        #endregion
    }
}
