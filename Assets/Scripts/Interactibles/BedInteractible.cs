using UnityEngine;

namespace Canicule
{
    public class BedInteractible : InteractibleObject
    {
        private SleepManager m_sleepManager;

        private void Awake()
        {
            m_sleepManager = GameManager.GetSleepManager();
        }

        public override void LaunchInteraction(Transform actor,Interactor interactActorType)
        {
            base.LaunchInteraction(actor, interactActorType);
            TrySleep();
        }

        private void TrySleep()
        {
            m_sleepManager.AttemptSleep();
        }
    }
}
