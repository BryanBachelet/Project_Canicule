using UnityEngine;
using System.Collections.Generic;

namespace Canicule
{
    public class PCManager : MonoBehaviour
    {
        private InputManager m_inputManager;
        private PCUIManager m_pcUIManager;

        #region Unity Methods
        private void Awake()
        {
            m_inputManager = FindAnyObjectByType<InputManager>();
            m_pcUIManager = GetComponent<PCUIManager>();
        }

        private void OnEnable()
        {
            m_pcUIManager.OnPCOpen += SwitchToUIMap;
            m_pcUIManager.OnPCClosed += SwitchToPlayerMap;
        }

        private void OnDisable()
        {
            m_pcUIManager.OnPCOpen -= SwitchToUIMap;
            m_pcUIManager.OnPCClosed -= SwitchToPlayerMap;
        }
        #endregion

        #region Input Methods
        private void SwitchToUIMap()
        {
            m_inputManager.DisablePlayerMap();
            m_inputManager.EnableUIMap();
        }

        private void SwitchToPlayerMap()
        {
            m_inputManager.EnablePlayerMap();
            m_inputManager.DisableUIMap();
        }
        #endregion

    }
}
