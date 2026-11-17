using UnityEngine;

namespace Canicule
{
    public class BedInteractible : InteractibleObject
    {
        private SleepManager m_sleepManager;

        private void Awake()
        {
            m_sleepManager = GameManager.GetSleepManager();

            GameManager.SubcribeToOnGameEventChange(OnGamePhaseChange);
        }

        public override void OnDestroy()
        {
            GameManager.UnsubcribeToOnGameEventChange(OnGamePhaseChange);
        }

        private void OnGamePhaseChange(GamePhase phase)
        {
            bool bIsNight = phase == GamePhase.NIGHT;
            SetAvailability(bIsNight);
        }

        public override void LaunchInteraction(Transform actor,Interactor interactActorType)
        {
            base.LaunchInteraction(actor, interactActorType);
        }

        protected override void Interact()
        {
            TrySleep();
            base.Interact();
        }

        private void TrySleep()
        {
            m_sleepManager.AttemptSleep();
        }
    }
}
