using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class BedInteractible : InteractibleObject
    {
        private SleepManager m_sleepManager;

        [Header("Components")]
        [SerializeField] private Image m_interactionBarImg;
        [SerializeField] private Image m_interactionBackgroundImg;
        private bool m_isCooldownActive = false;

        private void Awake()
        {
            m_sleepManager = GameManager.GetSleepManager();

            GameManager.SubcribeToOnGameEventChange(OnGamePhaseChange);
        }

        public override void OnDestroy()
        {
            GameManager.UnsubcribeToOnGameEventChange(OnGamePhaseChange);
        }

        public override void Update()
        {
            base.Update();
            if(!m_sleepManager.CanSleep())
            {
                UpdateUICooldown(m_sleepManager.GetRatioCooldownSleepAttemp());
                m_isCooldownActive = true;
            }
            else if(m_isCooldownActive)
            {
                DeactiveUI();
            }

        }

        private void OnGamePhaseChange(GamePhase phase)
        {
            bool bIsNight = phase == GamePhase.NIGHT;
            SetAvailability(bIsNight);
        }

        public override void LaunchInteraction(Transform actor,Interactor interactActorType)
        {
            if (!m_sleepManager.CanSleep() && m_isCooldownActive) return;
            
            base.LaunchInteraction(actor, interactActorType);
        }

        protected override void Interact()
        {
            TrySleep();
            base.Interact();
        }

        private void TrySleep()
        {
            if (!m_sleepManager.AttemptSleep())
            {
                ActiveUI();
            }
        }

        private void ActiveUI()
        {
            m_interactionBackgroundImg.gameObject.SetActive(true);
            m_interactionBarImg.gameObject.SetActive(true);
           
        }

        private void DeactiveUI()
        {
            m_interactionBackgroundImg.gameObject.SetActive(false);
            m_interactionBarImg.gameObject.SetActive(false);
            m_isCooldownActive = false;
        }

        private void UpdateUICooldown(float ratio)
        {
            m_interactionBarImg.fillAmount = ratio;

        }


    }
}
