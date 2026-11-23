using UnityEngine;

namespace Canicule
{
    public class StandingFan : InteractibleObject
    {
        private ItemEffectManager m_itemEffectManager;
        [SerializeField] private float m_sleepChanceIncreaseAmount = 0f;
        [SerializeField] private float m_sleepQualityDecreaseAmount = 0f;
        [SerializeField] private bool b_isOn = false;
        [SerializeField] private bool b_isInstalled = false;
        [SerializeField] private GameObject m_lightOn;
        [SerializeField] private GameObject m_lightOff;

        #region Unity Methods
        public override void Start()
        {
            base.Start();
            m_itemEffectManager = GameManager.GetItemEffectManager();

            if (!b_isInstalled)
            {
                gameObject.SetActive(false);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        #endregion

        public override void LaunchInteraction(Transform actor, Interactor interactActorType)
        {
            base.LaunchInteraction(actor, interactActorType);
            StandingFanInteraction();
        }

        public void InitStandingFan(float sleepChance, float sleepQuality)
        {
            b_isInstalled = true;
            m_sleepChanceIncreaseAmount = sleepChance;
            m_sleepQualityDecreaseAmount = sleepQuality;
            StandingFanInteraction();
        }
        private void StandingFanInteraction()
        {
            if (!b_isOn)
            {
                b_isOn = true;
                m_itemEffectManager.ModifySleepChance(m_sleepChanceIncreaseAmount);
                m_itemEffectManager.ModifySleepQuality(m_sleepQualityDecreaseAmount);
                m_lightOn.SetActive(true);
                m_lightOff.SetActive(false);
                Debug.Log("[Standing Fan] Switched on!");
            }
            else
            {
                b_isOn = false;
                m_itemEffectManager.ModifySleepChance(-1 * m_sleepChanceIncreaseAmount);
                m_itemEffectManager.ModifySleepQuality(-1 * m_sleepQualityDecreaseAmount);
                m_lightOn.SetActive(false);
                m_lightOff.SetActive(true);
                Debug.Log("[Standing Fan] Switched off!");
            }
        }
    }
}
