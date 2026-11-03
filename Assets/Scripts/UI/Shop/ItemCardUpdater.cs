using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Canicule
{
    public class ItemCardUpdater : MonoBehaviour
    {
        #region Fields for values management
        [SerializeField] private ColorsData m_colors;
        [SerializeField] private Image m_icon;
        [SerializeField] private TMP_Text m_name;
        [SerializeField] private TMP_Text m_description;
        [SerializeField] private TMP_Text m_itemPrice;
        [SerializeField] private TMP_Text m_hungerHealValue;
        [SerializeField] private TMP_Text m_thirstHealValue;
        [SerializeField] private TMP_Text m_ownedAmount;
        [SerializeField] private GameObject m_hungerField;
        [SerializeField] private GameObject m_thirstField;
        [SerializeField] private GameObject m_buyButton;
        [SerializeField] private GameObject m_tooExpensiveButton;
        [SerializeField] private GameObject m_consumableField;
        [SerializeField] private GameObject m_upgradeField;
        [SerializeField] private string m_currency;
        private ItemData m_storedItem;
        #endregion

        #region Fields for button management

        private ShopDataManager m_shopManager;
        private InventoryDataManager m_inventoryDataManager;

        #endregion

        #region Unity Methods
        private void Awake()
        {
            m_shopManager = FindAnyObjectByType<ShopDataManager>();
            m_inventoryDataManager = FindAnyObjectByType<InventoryDataManager>();
        }

        private void OnEnable()
        {
            m_shopManager.OnItemCartAdded += _ => VerifyPlayerBalance();
            m_shopManager.OnItemCartRemoved += _ => VerifyPlayerBalance();
            m_shopManager.OnItemCartUpdated += (_, _) => VerifyPlayerBalance();
            //m_inventoryDataManager.OnAddingItem += UpdateOwnedAmount;
        }

        public void OnDisable()
        {
            m_shopManager.OnItemCartAdded -= _ => VerifyPlayerBalance();
            m_shopManager.OnItemCartRemoved -= _ => VerifyPlayerBalance();
            m_shopManager.OnItemCartUpdated -= (_, _) => VerifyPlayerBalance();
            //m_inventoryDataManager.OnAddingItem -= UpdateOwnedAmount;
        }

        private void Start()
        {
            VerifyPlayerBalance();
        }
        #endregion

        #region Values Management
        public void UpdateItemCard(ItemData item)
        {
            m_storedItem = item;

            m_icon.sprite = item.itemIcon;
            m_name.text = item.itemName;
            m_description.text = item.itemDescription;
            m_itemPrice.text = item.itemPrice.ToString() + " " + m_currency;

            // Change buy button color
            Image buttonImage = m_buyButton.GetComponent<Image>();
            switch (item.exhaustionType)
            {
                case ExhaustionType.Sleep:
                    buttonImage.color = m_colors.sleepColor;
                    break;
                case ExhaustionType.Hunger:
                    buttonImage.color = m_colors.hungerColor;
                    break;
                case ExhaustionType.Thirst:
                    buttonImage.color = m_colors.thirstColor;
                    break;
                case ExhaustionType.BodyTemperature:
                    buttonImage.color = m_colors.bodyTemperatureColor;
                    break;
            }

            // Hunger & Thirst
            m_hungerField.SetActive(false);
            m_thirstField.SetActive(false);

            // Activate the hunger and thirst fields if there are hunger or thirst effects in the item
            foreach(ItemEffect effect in item.itemEffects)
            {
                if(effect is ModifyHunger)
                {
                    m_hungerField.SetActive(true);

                    ModifyHunger modifyHunger = (ModifyHunger)effect;
                    m_hungerHealValue.text = modifyHunger.GetHungerModifierAmount().ToString();
                }

                if(effect is ModifyThirst)
                {
                    m_thirstField.SetActive(true);

                    ModifyThirst modifyThirst = (ModifyThirst)effect;
                    m_thirstHealValue.text = modifyThirst.GetThirstModifierAmount().ToString();
                }
            }

            // Activate the consumable or upgrade field
            if(item.itemType == ItemType.Upgrade)
            {
                m_upgradeField.SetActive(true);
                m_consumableField.SetActive(false);
            }
            else
            {
                m_upgradeField.SetActive(false);
                m_consumableField.SetActive(true);
            }

        }
        public void UpdateOwnedAmount(int ownedAmount)
        {
            m_ownedAmount.text = ownedAmount.ToString();
        }
        #endregion

        #region Button Management
        public void VerifyPlayerBalance()
        {
            if(m_storedItem.itemPrice > m_inventoryDataManager.GetPlayerBalance())
            {
                DeactivateAddButton();
            }
            else
            {
                ActivateAddButton();
            }
        }

        public void ActivateAddButton()
        {
            m_buyButton.SetActive(true);
            m_tooExpensiveButton.SetActive(false);
        }

        public void DeactivateAddButton()
        {
            m_buyButton.SetActive(false);
            m_tooExpensiveButton.SetActive(true);
        }
        #endregion

        #region Getter
        public ItemData GetStoredItem()
        {
            return m_storedItem;
        }
        #endregion

    }
}
