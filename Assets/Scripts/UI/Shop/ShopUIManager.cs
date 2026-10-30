using UnityEngine;
using System.Collections.Generic;

namespace Canicule
{
    public class ShopUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_itemCard;
        [SerializeField] private List<GameObject> m_tabs;
        [SerializeField] private List<GameObject> m_itemHolders;
        private GameObject m_currentActiveTab;
        private ShopDataManager m_shopManager;

        #region Unity Methods
        private void Awake()
        {
            m_shopManager = FindAnyObjectByType<ShopDataManager>();
        }
        public void Start()
        {
            // Resets the tabs
            CloseAllTabs();
            ActivateTab(m_tabs[0]);

            // Fills the tabs with items
            FillAllTabsWithItemCards();
        }
        #endregion

        #region Tab Management
        public void CloseAllTabs()
        {
            m_currentActiveTab = null;

            foreach(GameObject tab in m_tabs)
            {
                tab.SetActive(false);
            }
        }

        public void ActivateTab(GameObject tab)
        {
            m_currentActiveTab = tab;
            m_currentActiveTab.SetActive(true);
        }

        public void SwitchTab(GameObject tab)
        {
            // Deactivates the previous activate tab & Updates its tab button visual
            m_currentActiveTab.SetActive(false);

            // Stores the new active tab
            m_currentActiveTab = tab;

            // Activates it and updates its tab button visual
            tab.SetActive(true);
        }

        public void FillTabWithItemCards(ExhaustionType type, GameObject itemHolder)
        {
            // Gets the item list of type type
            List<ItemData> items = m_shopManager.GetShopItemsByType(type);
            
            // For each item, instantiates an item card
            foreach(ItemData item in items)
            {
                // Display the item, if it can be bought
                if (item.b_canBeBought)
                {
                    // Should instantiate in the grid layout
                    GameObject newItemCard = GameObject.Instantiate(m_itemCard, itemHolder.transform);
                    
                    // Set the values of newItemCard with item values
                    newItemCard.GetComponent<ItemCardUpdater>().UpdateItemCard(item);
                }
            }

        }

        public void FillAllTabsWithItemCards()
        {
            FillTabWithItemCards(ExhaustionType.Sleep, m_itemHolders[0]);
            FillTabWithItemCards(ExhaustionType.Hunger, m_itemHolders[1]);
            FillTabWithItemCards(ExhaustionType.Thirst, m_itemHolders[2]);
            FillTabWithItemCards(ExhaustionType.BodyTemperature, m_itemHolders[3]);
        }
        #endregion
    }
}
