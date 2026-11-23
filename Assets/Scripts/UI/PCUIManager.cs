using Canicule.Temperature;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Canicule
{
    public class PCUIManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject m_PCScreen;
        [SerializeField] private TMP_Text m_timeText;
        [SerializeField] private TMP_Text m_weatherText;
        [SerializeField] private string m_weatherSuffix = "°C";
        [SerializeField] private List<GameObject> m_apps = new List<GameObject>();
        
        private GameObject m_activeApp;
        private NightDayManager m_nightDayManager;
        private EnvirommentHeatSource m_environmentHeatSource;

        public event Action OnPCOpen;
        public event Action OnPCClosed;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            m_nightDayManager = GameManager.GetNightDayManager();
            m_environmentHeatSource = GameManager.GetEnvirommentHeatSource();
        }
        private void Start()
        {
            m_PCScreen.SetActive(false);

            foreach (GameObject app in m_apps)
            {
                app.SetActive(false);
            }
        }

        private void Update()
        {
            m_timeText.text = m_nightDayManager.GetDispayableHour();
            if (m_nightDayManager.GetIsNightActive())
            {
                m_weatherText.text = m_environmentHeatSource.GetEnvironmentTemperature().ToString("F0") + m_weatherSuffix;
            }
            else
            {
                m_weatherText.text = m_environmentHeatSource.GetMorningTemperature().ToString("F0") + m_weatherSuffix;
            }
        }
        #endregion

        #region Display Methods
        public void SwitchPCScreen()
        {
            if (m_PCScreen.activeSelf)
            {
                m_PCScreen.SetActive(false);
                CloseActiveApp();
                OnPCClosed?.Invoke();
            }
            else
            {
                m_PCScreen.SetActive(true);
                OnPCOpen?.Invoke();
            }
        }

        // Opens the active app
        public void OpenActiveApp(GameObject gameObject)
        {
            m_activeApp = gameObject;
            m_activeApp.SetActive(true);
        }

        // Closes the active app
        public void CloseActiveApp()
        {
            if (m_activeApp != null)
            {
                m_activeApp.SetActive(false);
            }
        }
        #endregion
    }
}
