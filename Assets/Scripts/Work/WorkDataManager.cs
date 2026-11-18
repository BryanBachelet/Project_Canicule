using System;
using UnityEngine;

namespace Canicule
{
    public class WorkDataManager : MonoBehaviour
    {
        [SerializeField] private WorkData m_WorkData;
        private InventoryDataManager m_inventoryDataManager;
        private float m_gainModifier = 0f;

        private void Awake()
        {
            m_inventoryDataManager = GameManager.GetInventoryDataManager();
        }

        public void GainMoney()
        {
            float randomGain = UnityEngine.Random.Range(m_WorkData.moneyGain.x, 
                m_WorkData.moneyGain.y + m_gainModifier);
            float roundedRandomGain = Mathf.Round(randomGain * 100f) / 100f;

            float newbalance = m_inventoryDataManager.GetPlayerBalance() + roundedRandomGain;

            m_inventoryDataManager.SetPlayerBalance(newbalance);

            Debug.Log("gain modifier : " + m_gainModifier);
            Debug.Log("rounded random gain : " + roundedRandomGain);
        }
        public void ModifyGainModifier(float amount)
        {
            m_gainModifier += amount;
        }

        public float GetPlayerBalance()
        {
            return m_inventoryDataManager.GetPlayerBalance();
        }

    }
}
