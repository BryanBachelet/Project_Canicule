using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace Canicule
{
    public class InventoryDataManager : MonoBehaviour
    {
        [SerializeField] private float m_playerBalance;
        private Dictionary<ItemData, int> m_playerItems;
        private ExhaustionManager m_exhaustionManager;
        private ShopDataManager m_shopManager;

        #region Unity Methods
        private void Awake()
        {
            m_exhaustionManager = FindAnyObjectByType<ExhaustionManager>();
            m_shopManager = FindAnyObjectByType<ShopDataManager>();
        }
        #endregion

        #region Inventory Data Management Methods
        public void UseItem(ItemData item)
        {
            item.UseItem(m_exhaustionManager.gameObject);

            RemoveItem(item);
        }

        // This method is used to add the cart items from the shopDataManager to the player items
        public void AddNewItems(Dictionary<ItemData, int> itemCart)
        {
            foreach (KeyValuePair<ItemData,int> item in itemCart)
            {
                bool b_hasKey = m_playerItems.ContainsKey(item.Key);

                
                if (item.Key.itemType != ItemType.Upgrade)
                {
                    // Add item in inventory if it's not an upgrade
                    if (b_hasKey)
                    {
                        m_playerItems[item.Key] += item.Value;
                    }
                    else
                    {
                        m_playerItems.Add(item.Key, item.Value);
                    }
                }
                else
                {
                    // If it's an upgrade, ask the Shop Manager to remove the item from the shop
                    m_shopManager.RemoveItemFromShop(item.Key);
                }

            }
        }

        public void RemoveItem(ItemData item)
        {
            m_playerItems[item] -= 1;

            if(m_playerItems[item] == 0)
            {
                m_playerItems.Remove(item);
            }
        }
        #endregion

        #region Getter & Setter
        public float GetPlayerBalance()
        {
            return m_playerBalance;
        }

        public void SetPlayerBalance(float newBalance)
        {
            m_playerBalance = newBalance;
        }
        #endregion
    }
}
