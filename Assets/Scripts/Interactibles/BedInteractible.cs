using UnityEngine;

namespace Canicule
{
    public class BedInteractible : InteractibleObject
    {
        private SleepManager m_sleepManager;

        private void Awake()
        {
            m_sleepManager = Object.FindAnyObjectByType<SleepManager>();
        }

        public override void Interact()
        {
            TrySleep();
        }

        private void TrySleep()
        {
            m_sleepManager.AttemptSleep();
        }
    }
}
