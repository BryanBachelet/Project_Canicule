using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class ShopUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_itemCard;
        [SerializeField] private GameObject m_cartCard;
        [SerializeField] private List<GameObject> m_tabs;
        [SerializeField] private List<GameObject> m_itemHolders;
        [SerializeField] private GameObject m_cartHolder;
        [SerializeField] private List<ScrollRect> m_shopScrollrect;
        [SerializeField] private ScrollRect m_cartScrollrect;
        private GameObject m_currentActiveTab;
        private ShopDataManager m_shopDataManager;
        private InventoryDataManager m_inventoryManager;

        #region Unity Methods
        private void Awake()
        {
            m_shopDataManager = GameManager.GetShopDataManager();
            m_inventoryManager = FindAnyObjectByType<InventoryDataManager>();
        }

        private void OnEnable()
        {
            m_shopDataManager.OnItemCartAdded += FillCartWithNewCard;
            m_shopDataManager.OnItemCartUpdated += UpdateCartCardAmount;
            m_shopDataManager.OnItemCartRemoved += RemoveCartCard;
            m_shopDataManager.OnCartItemsMoved += ClearAllCartCard;
            m_inventoryManager.OnAddingItem += UpdateCardOwnedAmount;
        }

        private void OnDisable()
        {
            m_shopDataManager.OnItemCartAdded -= FillCartWithNewCard;
            m_shopDataManager.OnItemCartUpdated -= UpdateCartCardAmount;
            m_shopDataManager.OnItemCartRemoved -= RemoveCartCard;
            m_shopDataManager.OnCartItemsMoved -= ClearAllCartCard;
            m_inventoryManager.OnAddingItem -= UpdateCardOwnedAmount;
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
            List<ItemData> items = m_shopDataManager.GetShopItemsByType(type);
            
            // For each item, instantiates an item card
            foreach(ItemData item in items)
            {
                // Should instantiate in the grid layout
                GameObject newItemCard = GameObject.Instantiate(m_itemCard, itemHolder.transform);
                    
                // Set the values of newItemCard with item values
                newItemCard.GetComponent<ItemCardUpdater>().UpdateItemCard(item);
                newItemCard.GetComponent<ItemCardUpdater>().UpdateOwnedAmount(m_inventoryManager.GetItemCount(item));
            }

        }

        public void FillAllTabsWithItemCards()
        {
            FillTabWithItemCards(ExhaustionType.Sleep, m_itemHolders[0]);
            FillTabWithItemCards(ExhaustionType.Hunger, m_itemHolders[1]);
            FillTabWithItemCards(ExhaustionType.Thirst, m_itemHolders[2]);
            FillTabWithItemCards(ExhaustionType.BodyTemperature, m_itemHolders[3]);
        }

        public void UpdateCardOwnedAmount(ItemData item, int amount)
        {
            ItemCardUpdater[] itemCardUpdaters = m_itemHolders[(int)item.exhaustionType].GetComponentsInChildren<ItemCardUpdater>();

            foreach(ItemCardUpdater itemCardUpdater in itemCardUpdaters)
            {
                if(itemCardUpdater.GetStoredItem() == item)
                {
                    itemCardUpdater.UpdateOwnedAmount(amount);
                    return;
                }
            }
        }

        public void ResetTabScrollbars()
        {
            foreach(ScrollRect scrollrect in m_shopScrollrect)
            {
                scrollrect.verticalNormalizedPosition = 1;
            }
        }
        #endregion

        #region Cart Management
        // Creates a new cart card in the cart
        public void FillCartWithNewCard(ItemData item)
        {
            GameObject newCartCard = GameObject.Instantiate(m_cartCard, m_cartHolder.transform);
            newCartCard.GetComponent<CartCardUpdater>().UpdateCartCard(item);
            newCartCard.GetComponent<CartCardUpdater>().UpdateCartAmount(1);
        }

        // Updates the amount of a cart card in the cart
        public void UpdateCartCardAmount(ItemData item, int amount)
        {
            CartCardUpdater[] cartCards = m_cartHolder.GetComponentsInChildren<CartCardUpdater>();

            foreach(CartCardUpdater cartCardUpdater in cartCards)
            {
                if(cartCardUpdater.GetStoredItem() == item)
                {
                    cartCardUpdater.UpdateCartAmount(amount);
                    return;
                }
            }
        }

        // Destroy a cart card in the cart
        public void RemoveCartCard(ItemData item)
        {
            CartCardUpdater[] cartCards = m_cartHolder.GetComponentsInChildren<CartCardUpdater>();

            foreach (CartCardUpdater cartCardUpdater in cartCards)
            {
                if (cartCardUpdater.GetStoredItem() == item)
                {
                    Destroy(cartCardUpdater.gameObject);
                    return;
                }
            }
        }

        // Clear all cart card
        public void ClearAllCartCard()
        {
            CartCardUpdater[] cartCards = m_cartHolder.GetComponentsInChildren<CartCardUpdater>();

            foreach(CartCardUpdater cartCardUpdater in cartCards)
            {
                Destroy(cartCardUpdater.gameObject);
            }
        }

        // Reset cart scrollbar
        public void ResetCartScrollbar()
        {
            m_cartScrollrect.verticalNormalizedPosition = 1;
        }

        #endregion
    }
}
