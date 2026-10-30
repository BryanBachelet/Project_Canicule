using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class ItemCardUpdater : MonoBehaviour
    {
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
        [SerializeField] private string m_currency;
        private ItemData storedItem;

        public void UpdateItemCard(ItemData item)
        {
            storedItem = item;

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

        }

        public void SwapButtons()
        {
            m_buyButton.SetActive(!m_buyButton.activeSelf);
            m_tooExpensiveButton.SetActive(!m_tooExpensiveButton.activeSelf);
        }
    }
}
