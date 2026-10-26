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

        public override void LaunchInteraction(Transform actor)
        {
            base.LaunchInteraction(actor);
            TrySleep();
        }

        private void TrySleep()
        {
            m_sleepManager.AttemptSleep();
        }
    }
}
