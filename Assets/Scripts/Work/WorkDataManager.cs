using System;
using UnityEngine;

namespace Canicule
{
    public class WorkDataManager : MonoBehaviour
    {
        [SerializeField] private WorkData m_WorkData;
        private InventoryDataManager m_inventoryDataManager;

        private void Awake()
        {
            m_inventoryDataManager = FindAnyObjectByType<InventoryDataManager>();
        }

        public void GainMoney()
        {
            float randomGain = UnityEngine.Random.Range(m_WorkData.moneyGainFromClick.x, m_WorkData.moneyGainFromClick.y);
            float roundedRandomGain = Mathf.Round(randomGain * 100f) / 100f;

            float newbalance = m_inventoryDataManager.GetPlayerBalance() + roundedRandomGain;

            m_inventoryDataManager.SetPlayerBalance(newbalance);
        }
    }
}
