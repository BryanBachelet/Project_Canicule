using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace Canicule
{
    public class InventoryDataManager : MonoBehaviour
    {
        [SerializeField] private float m_playerBalance;
        private int m_indexSelector = 0;

        private Dictionary<ItemData, int> m_playerItems = new Dictionary<ItemData, int>();
        private List<ItemData> m_displayedInventory = new List<ItemData>();
        private ExhaustionManager m_exhaustionManager;

        public event Action<ItemData, int> OnAddingItem; // Used to update the owned amount in the shop
        public event Action OnDecreasingItemAmount;
        public event Action OnRemovingItem;
        public event Action OnAddingFinished;
        public event Action OnSelectorChanged;

        #region Unity Methods
        private void Awake()
        {
            m_exhaustionManager = FindAnyObjectByType<ExhaustionManager>();
        }
        #endregion

        #region Inventory Data Management Methods
        public void UseItem()
        {
            if(m_indexSelector < m_displayedInventory.Count)
            {
                ItemData item = m_displayedInventory[m_indexSelector];

                Debug.Log("[Inventory Data] Used " + item.itemName);

                if(item.itemType == ItemType.Consumable)
                {
                    RemoveItem(item);
                }
                else if (item.itemType == ItemType.Equippable)
                {
                    Debug.Log("Switch Equip State");
                }
                
                item.UseItem();
            }

        }

        // This method is used to add the cart items from the shopDataManager to the player items
        public void AddNewItems(Dictionary<ItemData, int> itemCart)
        {

            foreach (KeyValuePair<ItemData,int> item in itemCart)
            {
                bool b_hasKey = m_playerItems.ContainsKey(item.Key);

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

            OnAddingFinished?.Invoke();
        }

        public void RemoveItem(ItemData item)
        {
            m_playerItems[item] -= 1;

            if(m_playerItems[item] == 0)
            {
                m_playerItems.Remove(item);
                m_displayedInventory.Remove(item);

                OnAddingItem?.Invoke(item, 0);
                OnRemovingItem?.Invoke();
                Debug.Log("An item has been removed!");
            }
            else
            {
                OnAddingItem?.Invoke(item, m_playerItems[item]);
                OnDecreasingItemAmount?.Invoke();
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

        public Dictionary<ItemData,int> GetPlayerItems()
        {
            return m_playerItems;
        }

        public int GetIndexSelector()
        {
            return m_indexSelector;
        }

        public void IncreaseIndexSelector()
        {
            if(m_indexSelector < m_displayedInventory.Count - 1)
            {
                m_indexSelector++;
                OnSelectorChanged?.Invoke();
            }
        }

        public void DecreaseIndexSelector()
        {
            if(m_indexSelector > 0)
            {
                m_indexSelector--;
                OnSelectorChanged?.Invoke();
            }
        }

        public void SetIndexSelector(int index)
        {
            m_indexSelector = index;
        }

        public List<ItemData> GetDisplayedInventory()
        {
            return m_displayedInventory;
        }

        public void AddItemInDisplayedInventory(ItemData item)
        {
            m_displayedInventory.Add(item);
        }

        public void ClearItemsFromDisplayedInventory()
        {
            m_displayedInventory.Clear();
        }
        #endregion
    }
}
