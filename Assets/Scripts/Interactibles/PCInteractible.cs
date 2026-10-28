using UnityEngine;

namespace Canicule
{
    public class PCInteractible : InteractibleObject
    {
        private PCUIManager m_pcUIManager;

        private void Awake()
        {
            m_pcUIManager = FindAnyObjectByType<PCUIManager>();
        }

        protected override void Interact()
        {
            base.Interact();

            m_pcUIManager.SwitchPCScreen();
        }
    }
}
