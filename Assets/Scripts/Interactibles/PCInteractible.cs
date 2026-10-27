using UnityEngine;

namespace Canicule
{
    public class PCInteractible : InteractibleObject
    {
        private PCManager m_pcManager;

        private void Awake()
        {
            m_pcManager = FindAnyObjectByType<PCManager>();
        }

        protected override void Interact()
        {
            base.Interact();

            m_pcManager.SwitchPCScreen();
        }
    }
}
