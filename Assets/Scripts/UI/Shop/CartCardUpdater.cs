using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class CartCardUpdater : MonoBehaviour
    {
        [SerializeField] private Image m_icon;
        [SerializeField] private TMP_Text m_name;
        [SerializeField] private TMP_Text m_amount;
        [SerializeField] private TMP_Text m_price;
        [SerializeField] private string m_currency;
        [SerializeField] private string m_amountPrefix;
        private ItemData m_storedItem;

        public void UpdateCartCard(ItemData item)
        {
            m_storedItem = item;
            m_icon.sprite = item.itemIcon;
            m_name.text = item.itemName;
            m_price.text = item.itemPrice + m_currency;
        }

        public void UpdateCartAmount(int amount)
        {
            m_amount.text = m_amountPrefix + amount.ToString();
        }

        #region Getter
        public ItemData GetStoredItem()
        {
            return m_storedItem;
        }
        #endregion
    }
}
