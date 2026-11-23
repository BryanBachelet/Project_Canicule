using TMPro;
using UnityEngine;

namespace Canicule
{
    public class WorkUIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_balanceText;
        [SerializeField] private string m_currency;
        [SerializeField] private GameObject m_moneyAppOnlineState;
        [SerializeField] private GameObject m_moneyAppOfflineState;

        private WorkDataManager m_workDataManager;
        private NightDayManager m_nightDayManager;

        #region Unity Methods
        private void Awake()
        {
            m_workDataManager = GameManager.GetWorkDataManager();
            m_nightDayManager = GameManager.GetNightDayManager();
        }

        private void OnEnable()
        {
            UpdateBalance();

            if (m_nightDayManager.GetIsNightActive())
            {
                MakeAppOnline();
            }
            else
            {
                MakeAppOffline();
            }
        }
        #endregion

        #region UI Management
        public void UpdateBalance()
        {
            m_balanceText.text = m_workDataManager.GetPlayerBalance().ToString("F2") + m_currency;
        }

        private void MakeAppOnline()
        {
            m_moneyAppOnlineState.SetActive(true);
            m_moneyAppOfflineState.SetActive(false);
        }

        private void MakeAppOffline()
        {
            m_moneyAppOnlineState.SetActive(false);
            m_moneyAppOfflineState.SetActive(true);
        }
        #endregion
    }
}
