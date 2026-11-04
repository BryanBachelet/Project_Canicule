using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace Canicule
{
    public class InventoryDataManager : MonoBehaviour
    {
        [SerializeField] private float m_playerBalance;
        private Dictionary<ItemData, int> m_playerItems = new Dictionary<ItemData, int>();
        private ExhaustionManager m_exhaustionManager;
        private ShopDataManager m_shopManager;
        public event Action<ItemData, int> OnAddingItem;

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

                // Add item in inventory if it's not an upgrade
                if (b_hasKey)
                {
                    m_playerItems[item.Key] += item.Value;
                    OnAddingItem?.Invoke(item.Key, m_playerItems[item.Key]);
                }
                else
                {
                    m_playerItems.Add(item.Key, item.Value);
                    OnAddingItem?.Invoke(item.Key, 1);
                }
            }
        }

        public void RemoveItem(ItemData item)
        {
            m_playerItems[item] -= 1;

            if(m_playerItems[item] == 0)
            {
                m_playerItems.Remove(item);
                OnAddingItem?.Invoke(item, 0);
            }
            else
            {
                OnAddingItem?.Invoke(item, m_playerItems[item]);
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

        public int GetItemCount(ItemData item)
        {
            if (m_playerItems.ContainsKey(item))
            {
                return m_playerItems[item];
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}
