using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

namespace Canicule
{
    public enum InventoryDisplayMode
    {
        FREE = 0,
        SELECTION = 1,
    }

    public class InventoryUIManager : MonoBehaviour
    {
     

        [SerializeField] private GameObject m_inventoryScreen;
        [SerializeField] private GameObject m_inventoryItemCard;
        [SerializeField] private GameObject m_inventoryContainer;
        [SerializeField] private GameObject m_actionKey;
        [SerializeField] private TMP_Text m_itemDescription;
        [SerializeField] private TMP_Text m_itemName;
        [SerializeField] private TMP_Text m_itemAmount;
        [SerializeField] private TMP_Text m_itemType;
        [SerializeField] private TMP_Text m_itemAction;
        [SerializeField] private string m_itemAmountPrefix = "x";
        [SerializeField] private string m_itemDescriptionDefault = "It's empty...";
        [SerializeField] private string m_itemNameDefault = "Inventory";
        [SerializeField] private string m_itemAmountDefault = ".";
        [SerializeField] private string m_itemTypeDefault = "";
        [SerializeField] private string m_itemTypeConsumable = "Consumable";
        [SerializeField] private string m_itemTypeEquipement = "Equipement";
        [SerializeField] private string m_itemActionDefault = "";
        [SerializeField] private string m_itemActionConsumable = "Use";
        [SerializeField] private string m_itemActionEquipmentEquipped = "Unequip";
        [SerializeField] private string m_itemActionEquipmentNotEquipped = "Equip";

        private InventoryDataManager m_inventoryDataManager;

        private float m_itemInitialDistance = 576f;
        private float m_itemWidth;
        private float m_itemSpacing = 140f;

        public event Action<InventoryDisplayMode> OnInventoryOpen;
        public event Action OnInventoryClose;


        private InventoryDisplayMode m_displayMode = InventoryDisplayMode.FREE; 

        #region Unity Methods
        private void Awake()
        {
            m_inventoryDataManager = GameManager.GetInventoryDataManager();
        }

        private void OnEnable()
        {
            m_inventoryDataManager.OnAddingFinished += RecreateInventoryDisplay;
            m_inventoryDataManager.OnSelectorChanged += UpdateItemValues;
            m_inventoryDataManager.OnSelectorChanged += UpdateItemPositions;
            m_inventoryDataManager.OnDecreasingItemAmount += UpdateItemValues;
            m_inventoryDataManager.OnRemovingItem += UpdateDisplayedItemsAfterRemove;
        }

        private void OnDisable()
        {
            m_inventoryDataManager.OnAddingFinished -= RecreateInventoryDisplay;
            m_inventoryDataManager.OnSelectorChanged -= UpdateItemValues;
            m_inventoryDataManager.OnSelectorChanged -= UpdateItemPositions;
            m_inventoryDataManager.OnDecreasingItemAmount -= UpdateItemValues;
            m_inventoryDataManager.OnRemovingItem -= UpdateDisplayedItemsAfterRemove;
        }

        private void Start()
        {
            m_inventoryScreen.SetActive(false);

            // Finding the itemWidth
            RectTransform itemRect = m_inventoryItemCard.GetComponent<RectTransform>();
            m_itemWidth = itemRect.rect.width;

            // If the inventory is empty, set the default inventory
            if(m_inventoryDataManager.GetPlayerItems().Count == 0)
            {
                SetDefaultInventory();
            }
        }
        #endregion
        
        #region Display Methods
        public void SwitchInventoryScreen(InventoryDisplayMode displayMode)
        {
            if (m_inventoryScreen.activeSelf)
            {
                m_inventoryScreen.SetActive(false);
                OnInventoryClose?.Invoke();
            }
            else
            {
                m_inventoryScreen.SetActive(true);
                OnInventoryOpen?.Invoke(displayMode);
                m_displayMode = displayMode;
            }

        }

        private void RecreateInventoryDisplay()
        {
            ClearDisplayedItems();
            DisplayInventoryItems();
            UpdateItemValues();
        }

        private void DisplayInventoryItems()
        {
            int rank = 0;

            foreach(KeyValuePair<ItemData, int> item in m_inventoryDataManager.GetPlayerItems())
            {
                if(item.Value > 0 && item.Key.itemType != ItemType.Upgrade)
                {
                    GameObject newItemCard = GameObject.Instantiate(m_inventoryItemCard, m_inventoryContainer.transform);

                    InventoryItemCardUpdater itemCardUpdater = newItemCard.GetComponent<InventoryItemCardUpdater>();

                    itemCardUpdater.Initializer(newItemCard.GetComponent<RectTransform>(), gameObject.GetComponent<InventoryUIManager>(), m_inventoryDataManager);
                    itemCardUpdater.SetRank(rank);
                    itemCardUpdater.SetSprite(item.Key.itemIcon);
                    itemCardUpdater.UpdatePosition();
                    itemCardUpdater.UpdateStatus();

                    rank++;

                    m_inventoryDataManager.AddItemInDisplayedInventory(item.Key);
                }
            }
        }

        private void ClearDisplayedItems()
        {
            InventoryItemCardUpdater[] itemCards = m_inventoryContainer.GetComponentsInChildren<InventoryItemCardUpdater>();

            foreach(InventoryItemCardUpdater itemCard in itemCards)
            {
                Destroy(itemCard.gameObject);
            }

            m_inventoryDataManager.ClearItemsFromDisplayedInventory();
        }

        private void UpdateItemValues()
        {
            ItemData item = m_inventoryDataManager.GetDisplayedInventory()[m_inventoryDataManager.GetIndexSelector()];

            m_itemDescription.text = item.itemDescription.ToString();
            m_itemName.text = item.itemName.ToString();
            m_itemAmount.text = m_itemAmountPrefix + m_inventoryDataManager.GetPlayerItems()[item].ToString();

            switch (item.itemType)
            {
                case ItemType.Consumable:
                    m_itemType.text = m_itemTypeConsumable;
                    m_itemAction.text = m_itemActionConsumable;
                    break;
                case ItemType.Equippable:
                    m_itemType.text = m_itemTypeEquipement;
                    if (item.b_isEquipped)
                    {
                        m_itemAction.text = m_itemActionEquipmentEquipped;
                    }
                    else
                    {
                        m_itemAction.text = m_itemActionEquipmentNotEquipped;
                    }
                    break;
            }

            m_actionKey.SetActive(true);
        }

        private void UpdateItemPositions()
        {
            InventoryItemCardUpdater[] itemCards = m_inventoryContainer.GetComponentsInChildren<InventoryItemCardUpdater>();

            foreach (InventoryItemCardUpdater itemCard in itemCards)
            {
                itemCard.UpdatePosition();
                itemCard.UpdateStatus();
            }
        }

        private void SetDefaultInventory()
        {
            m_itemDescription.text = m_itemDescriptionDefault;
            m_itemName.text = m_itemNameDefault;
            m_itemAmount.text = m_itemAmountDefault;
            m_itemType.text = m_itemTypeDefault;
            m_itemAction.text = m_itemActionDefault;
            m_actionKey.SetActive(false);
        }

        private void UpdateDisplayedItemsAfterRemove()
        {
            if(m_inventoryDataManager.GetDisplayedInventory().Count == 0)
            {
                // Resets the inventory if there is no items in it
                ClearDisplayedItems();
                SetDefaultInventory();
            }
            else
            {
                InventoryItemCardUpdater[] itemCards = m_inventoryContainer.GetComponentsInChildren<InventoryItemCardUpdater>();
                
                Destroy(itemCards[m_inventoryDataManager.GetIndexSelector()].gameObject);

                if(m_inventoryDataManager.GetIndexSelector() == itemCards.Length - 1)
                {
                    // Moves the selector to ensure items are centered
                    m_inventoryDataManager.DecreaseIndexSelector();
                }
                else
                {
                    // Moves the items on the left to center them back
                    for (int i = m_inventoryDataManager.GetIndexSelector() + 1; i < itemCards.Length; i++)
                    {
                        itemCards[i].SetRank(itemCards[i].GetRank() - 1);
                        itemCards[i].UpdatePosition();
                        itemCards[i].UpdateStatus();
                    }

                    UpdateItemValues();
                }
            }
        }
        #endregion

        #region Getter
        public float GetItemWidth()
        {
            return m_itemWidth;
        }

        public float GetInitialItemDistance()
        {
            return m_itemInitialDistance;
        }

        public float GetItemSpacing()
        {
            return m_itemSpacing;
        }
        #endregion
    }
}
