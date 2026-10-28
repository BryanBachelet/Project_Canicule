using System;
using System.Collections.Generic;
using UnityEngine;

namespace Canicule
{
    public class PCUIManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject m_PCScreen;
        [SerializeField] private List<GameObject> m_apps = new List<GameObject>();
        
        private GameObject m_activeApp;
        private PCManager m_pcManager;

        public event Action OnPCOpen;
        public event Action OnPCClosed;
        #endregion

        #region Unity Methods
        private void Start()
        {
            m_PCScreen.SetActive(false);

            foreach (GameObject app in m_apps)
            {
                app.SetActive(false);
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
