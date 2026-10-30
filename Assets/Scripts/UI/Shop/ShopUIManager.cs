using UnityEngine;
using System.Collections.Generic;

namespace Canicule
{
    public class ShopUIManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_tabs;
        private GameObject m_currentActiveTab;

        #region Unity Methods
        public void Start()
        {
            // Resets the tabs
            CloseAllTabs();
            ActivateTab(m_tabs[0]);
        }
        #endregion

        #region Tab Management
        public void CloseAllTabs()
        {
            m_currentActiveTab = null;

            foreach(GameObject tab in m_tabs)
            {
                tab.SetActive(false);
            }
        }

        public void ActivateTab(GameObject tab)
        {
            m_currentActiveTab = tab;
            m_currentActiveTab.SetActive(true);
        }

        public void SwitchTab(GameObject tab)
        {
            // Deactivates the previous activate tab & Updates its tab button visual
            m_currentActiveTab.SetActive(false);

            // Stores the new active tab
            m_currentActiveTab = tab;

            // Activates it and updates its tab button visual
            tab.SetActive(true);
        }
        #endregion
    }
}
