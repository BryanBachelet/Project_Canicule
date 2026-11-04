using TMPro;
using UnityEngine;

namespace Canicule
{
    public class BalanceUpdaterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_balance;
        private ShopDataManager m_shopManager;
        private InventoryDataManager m_inventoryManager;

        #region Unity Methods
        private void Awake()
        {
            m_shopManager = FindAnyObjectByType<ShopDataManager>();
            m_inventoryManager = FindAnyObjectByType<InventoryDataManager>();
        }

        private void OnEnable()
        {
            UpdateBalance();
            m_shopManager.OnItemCartAdded += _ => UpdateBalance();
            m_shopManager.OnItemCartRemoved += _ => UpdateBalance();
            m_shopManager.OnItemCartUpdated += (_,_) => UpdateBalance();
        }

        private void OnDisable()
        {
            m_shopManager.OnItemCartAdded -= _ => UpdateBalance();
            m_shopManager.OnItemCartRemoved -= _ => UpdateBalance();
            m_shopManager.OnItemCartUpdated -= (_, _) => UpdateBalance();
        }
        #endregion

        private void UpdateBalance()
        {
            m_balance.text = m_inventoryManager.GetPlayerBalance().ToString("F2");
        }
    }
}
