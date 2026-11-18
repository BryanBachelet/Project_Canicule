using Canicule.Rooms;
using UnityEngine;

namespace Canicule
{
    public class ExhaustionManager : MonoBehaviour
    {
        #region Fields
        public ExhaustionData exhaustionData;

        [Range(0f, 1f)]
        [SerializeField] private float m_nightRatio;
        [SerializeField] private float m_roomTemperature;
        private float m_roomTemperatureModifier = 0f;
        private float m_sleepQuality = 1f;

        [Range(0f, 1f)]
        public float exhaustionRatio = 0f;
        public float exhaustion;
        public float sleepExhaustion;
        public float hungerExhaustion;
        public float thirstExhaustion;
        public float bodyTemperatureExhaustion;

        private bool m_bCanBeExhausted = false;
        private bool m_bCanBeSleepExhausted = false;
        private bool m_bCanBeHungerExhausted = false;
        private bool m_bCanBeThirstExhausted = false;
        private bool m_bCanBeBodyTemperatureExhausted = false;

        private RoomManager m_roomManager;
        private SleepManager m_sleepManager;
        private SleepExhaustionManager m_sleepExhaustionManager;
        private HungerExhaustionManager m_hungerExhaustionManager;
        private ThirstExhaustionManager m_thirstExhaustionManager;
        private BodyTemperatureExhaustionManager m_bodyTemperatureExhaustionManager;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            m_roomManager = GameManager.GetRoomManager();
            m_sleepManager = GameManager.GetSleepManager();
            m_sleepExhaustionManager = GetComponent<SleepExhaustionManager>();
            m_hungerExhaustionManager = GetComponent<HungerExhaustionManager>();
            m_thirstExhaustionManager = GetComponent<ThirstExhaustionManager>();
            m_bodyTemperatureExhaustionManager = GetComponent<BodyTemperatureExhaustionManager>();
        }

        private void OnEnable()
        {
            m_sleepManager.OnSleepStarted += HandleSleepStarted;
        }

        private void OnDisable()
        {
            m_sleepManager.OnSleepStarted -= HandleSleepStarted;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SwitchExhaustion(0, true);
        }

        // Update is called once per frame
        void Update()
        {
            if (m_bCanBeExhausted)
            {
                m_roomTemperature = m_roomManager.GetCurrentRoomPlayer().GetHeatManager().GetDegree();
                m_nightRatio = GameManager.GetNightDayManager().GetNightRatio();

                // General
                exhaustion = ComputeExhaustion();
                exhaustionRatio = ComputeExhaustionRatio(exhaustionRatio, exhaustion, exhaustionData.maxExhaustion);

                // Sleep
                if (m_bCanBeSleepExhausted)
                {
                    sleepExhaustion = ComputeSleepExhaustion();
                }
                // Hunger
                if (m_bCanBeHungerExhausted)
                {
                    hungerExhaustion = ComputeHungerExhaustion();
                }
                // Thirst
                if (m_bCanBeThirstExhausted)
                {
                    thirstExhaustion = ComputeThirstExhaustion();
                }
                // Body Temperature
                if (m_bCanBeBodyTemperatureExhausted)
                {
                    bodyTemperatureExhaustion = ComputeBodyTemperatureExhaustion();
                }

            }
        }
        #endregion

        #region General Exhaustion Management
        // Computes the value of exhaustion
        private float ComputeExhaustion()
        {
            return sleepExhaustion + hungerExhaustion + thirstExhaustion + bodyTemperatureExhaustion;
        }

        // Computes the value of exhaustion ratio
        private float ComputeExhaustionRatio(float exhaustionRatio, float exhaustion, float maxExhaustion)
        {
            exhaustionRatio = exhaustion / maxExhaustion;
            return exhaustionRatio;
        }

        // Modifies the booleen values of sleep
        /// <param name="exhaustionIndex">0: Modify all exhaustions 
        /// |1: Modify sleep exhaustion
        /// |2: Modify hunger exhaustion
        /// |3: Modify thirst exhaustion
        /// |4: Modify body temperature exhaustion</param>
        public void SwitchExhaustion(int exhaustionIndex, bool bIsActive)
        {
            switch (exhaustionIndex)
            {
                case 0:
                    m_bCanBeExhausted = bIsActive;
                    m_bCanBeSleepExhausted = bIsActive;
                    m_bCanBeHungerExhausted = bIsActive;
                    m_bCanBeThirstExhausted = bIsActive;
                    m_bCanBeBodyTemperatureExhausted = bIsActive;
                    break;
                case 1:
                    m_bCanBeSleepExhausted = bIsActive;
                    break;
                case 2:
                    m_bCanBeHungerExhausted = bIsActive;
                    break;
                case 3:
                    m_bCanBeThirstExhausted = bIsActive;
                    break;
                case 4:
                    m_bCanBeBodyTemperatureExhausted = bIsActive;
                    break;
                default:
                    Debug.LogError("exhaustion index not found");
                    break;
            }
        }
        #endregion

        #region Sleep
        private float ComputeSleepExhaustion()
        {
            return m_sleepExhaustionManager.IncreaseExhaustionOverTime(
                        sleepExhaustion, exhaustionData.sleepIncreaseRateOverNightRatio.Evaluate(m_nightRatio));
        }

        // Modifies exhaustion when sleep event is triggered
        private void HandleSleepStarted()
        {
            float sleepReduction = -1 * exhaustionData.sleepDecreaseAmountOverNightRatio.Evaluate(m_nightRatio) * m_sleepQuality;
            ModifySleepExhaustionValue(sleepReduction);
        }

        public void ModifySleepQuality(float amount)
        {
            m_sleepQuality += amount;
        }

        public void ModifySleepExhaustionValue(float amount)
        {
            sleepExhaustion = m_sleepExhaustionManager.ModifyExhaustionByAmount(sleepExhaustion, amount);
        }
        #endregion

        #region Hunger
        private float ComputeHungerExhaustion()
        {
            return m_hungerExhaustionManager.IncreaseExhaustionOverTime(
                        hungerExhaustion, exhaustionData.hungerIncreaseRate);
        }

        // Modify exhaustion when eating
        public void ModifyHungerExhaustion(float amount)
        {
            hungerExhaustion = m_hungerExhaustionManager.ModifyExhaustionByAmount(
                hungerExhaustion, amount);
        }

        #endregion

        #region Thirst
        public void ModifyThirstExhaustion(float amount)
        {
            thirstExhaustion = m_thirstExhaustionManager.ModifyExhaustionByAmount(
                thirstExhaustion, amount);
        }

        // Modify exhaustion when drinking
        private float ComputeThirstExhaustion()
        {
            return m_thirstExhaustionManager.IncreaseExhaustionOverTime(
                        thirstExhaustion, exhaustionData.thirstIncreaseRateOverTemperature.Evaluate(m_roomTemperature + m_roomTemperatureModifier));
        }
        #endregion

        #region Body Temperature
        private float ComputeBodyTemperatureExhaustion()
        {
            return m_bodyTemperatureExhaustionManager.ModifyExhaustionOverTime(
                        bodyTemperatureExhaustion,
                        exhaustionData.bodyTemperatureOverTargetTemperature.Evaluate(m_roomTemperature + m_roomTemperatureModifier),
                        exhaustionData.upBodyTemperatureSpeed,
                        exhaustionData.downBodyTemperatureSpeed);
        }

        public void ModifyRoomTemperatureModifier(float amount)
        {
            m_roomTemperatureModifier += amount;
        }

        public void ResetRoomTemperatureModifier()
        {
            m_roomTemperatureModifier = 0;
        }

        public float GetRoomTemperatureModifier()
        {
            return m_roomTemperatureModifier;
        }
        #endregion



    }
}
