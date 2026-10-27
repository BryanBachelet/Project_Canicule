using UnityEngine;
using System.Collections.Generic;

namespace Canicule
{
    public class PCManager : MonoBehaviour
    {

        [SerializeField] private GameObject m_PCScreen;
        [SerializeField] private List<GameObject> m_apps = new List<GameObject>();
        private InputManager m_inputManager;
        private GameObject m_activeApp;

        private void Awake()
        {
            m_inputManager = FindAnyObjectByType<InputManager>();
        }

        private void Start()
        {
            m_PCScreen.SetActive(false);

            foreach(GameObject app in m_apps)
            {
                app.SetActive(false);
            }
        }

        // Switches the state of the pc screen
        public void SwitchPCScreen()
        {
            if (m_PCScreen.activeSelf)
            {
                m_PCScreen.SetActive(false);
                m_inputManager.EnablePlayerMap();
                m_inputManager.DisableUIMap();
                CloseActiveApp();
                // End interaction please
            }
            else
            {
                m_PCScreen.SetActive(true);
                m_inputManager.DisablePlayerMap();
                m_inputManager.EnableUIMap();
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
            if(m_activeApp != null)
            {
                m_activeApp.SetActive(false);
            }
        }

        
    }
}
