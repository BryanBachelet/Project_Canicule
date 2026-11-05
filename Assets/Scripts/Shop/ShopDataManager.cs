using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Canicule
{
    public class ShopDataManager : MonoBehaviour
    {
        [SerializeField] private ShopContent m_shopContent;
        private InventoryDataManager m_inventoryDataManager;
        private Dictionary<ItemData, int> m_itemCart = new Dictionary<ItemData, int>();
        private float m_totalCartPrice;
        public event Action<ItemData> OnItemCartAdded;
        public event Action<ItemData> OnItemCartRemoved;
        public event Action<ItemData, int> OnItemCartUpdated;
        public event Action OnCartItemsMoved;
        public event Action OnShopOpened;

        #region Unity Methods
        private void Awake()
        {
            m_inventoryDataManager = FindAnyObjectByType<InventoryDataManager>();
        }
        void Start()
        {
            // Reset values
            m_totalCartPrice = 0;
        }
        #endregion

        #region Shop Data Management Methods
        public void AddItemInCart(ItemData item)
        {
            // Update player balance
            m_inventoryDataManager.SetPlayerBalance(m_inventoryDataManager.GetPlayerBalance() - item.itemPrice);

            // Add the item in the cart
            bool b_hasKey = m_itemCart.ContainsKey(item);

            if (b_hasKey)
            {
                // Increase the value of the key item
                m_itemCart[item] += 1;
                OnItemCartUpdated?.Invoke(item, m_itemCart[item]);
            }
            else
            {
                // Create a new key in the dict m_itemCart
                m_itemCart.Add(item, 1);
                OnItemCartAdded?.Invoke(item);
            }

        }

        public void RemoveItemFromCart(ItemData item)
        {
            // Update player balance
            m_inventoryDataManager.SetPlayerBalance(m_inventoryDataManager.GetPlayerBalance() + item.itemPrice);

            // Remove the item from the cart and if there is no more item in the dict, remove the key
            m_itemCart[item] -= 1;
            
            if (m_itemCart[item] == 0)
            {
                m_itemCart.Remove(item);
                OnItemCartRemoved?.Invoke(item);
            }
            else
            {
                OnItemCartUpdated?.Invoke(item, m_itemCart[item]);
            }

        }

        public float ComputeCartTotalPrice()
        {
            // Reset price
            m_totalCartPrice = 0;

            // Runs through the dict and add the price of each item
            foreach (KeyValuePair<ItemData, int> item in m_itemCart)
            {
                m_totalCartPrice += item.Key.itemPrice * item.Value;
            }

            return m_totalCartPrice;
        }

        // Moves the cart items to the inventory
        public void MoveCartItemsToInventory()
        {
            m_inventoryDataManager.AddNewItems(m_itemCart);
            FlagShopItemsAsBought();
            m_itemCart.Clear();
            OnCartItemsMoved?.Invoke();
        }

        // Flag each shop item as "already bought once"
        private void FlagShopItemsAsBought()
        {
            foreach (KeyValuePair<ItemData, int> item in m_itemCart)
            {
                foreach (ItemData shopItem in m_shopContent.shopCategories[(int)item.Key.exhaustionType].items)
                {
                    if (shopItem.b_hasBeenBoughtOnce)
                    {
                        break;
                    }

                    if (shopItem == item.Key)
                    {
                        shopItem.b_hasBeenBoughtOnce = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Event Management
        public void InvokeOnShopOpened()
        {
            OnShopOpened?.Invoke();
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

            foreach(ShopCategory category in m_shopContent.shopCategories)
            {
                if(category.type == type)
                {
                    return category.items;
                }
            }

            return null;
        }
        
        
        public Dictionary<ItemData, int> GetItemCart()
        {
            return m_itemCart;
        }
        #endregion
    }
}
