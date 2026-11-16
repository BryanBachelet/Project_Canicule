using TMPro;
using UnityEngine;

namespace Canicule
{
    public class WorkUIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_balanceText;
        [SerializeField] private string m_currency;
        private WorkDataManager m_workDataManager;

        private void Awake()
        {
            m_workDataManager = GameManager.GetWorkDataManager();
            
        }

        private void OnEnable()
        {
            UpdateBalance();
        }

        public void UpdateBalance()
        {
            m_balanceText.text = m_workDataManager.GetPlayerBalance().ToString("F2") + m_currency;
        }

        
    }
}
