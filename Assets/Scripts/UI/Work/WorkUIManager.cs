using TMPro;
using UnityEngine;

namespace Canicule
{
    public class WorkUIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_balanceText;
        [SerializeField] private string m_currency;
        private InventoryDataManager m_inventoryDataManager;

        private void Awake()
        {
            m_inventoryDataManager = GameManager.GetInventoryDataManager();
        }

        private void OnEnable()
        {
            UpdateBalance();
        }

        public void UpdateBalance()
        {
            m_balanceText.text = m_inventoryDataManager.GetPlayerBalance().ToString("F2") + m_currency;
        }

        
    }
}
