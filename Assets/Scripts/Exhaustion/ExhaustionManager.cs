using UnityEngine;

namespace Canicule
{
    public class ExhaustionManager : MonoBehaviour
    {

        public ExhaustionData exhaustionData;

        [Range(0f, 1f)]
        public float nightRatio = 0f; // To be replaced with the right value in the Night Manager
        [Range(28f, 50f)]
        public float roomTemperature = 28f; // To be replaced with the value in the Room Temp Manager

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

        private SleepManager m_sleepManager;
        private SleepExhaustionManager m_sleepExhaustionManager;
        private HungerExhaustionManager m_hungerExhaustionManager;
        private ThirstExhaustionManager m_thirstExhaustionManager;
        private BodyTemperatureExhaustionManager m_bodyTemperatureExhaustionManager;


        private void Awake()
        {
            m_sleepManager = Object.FindAnyObjectByType<SleepManager>();
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
                exhaustion = ComputeExhaustion();
                exhaustionRatio = ComputeExhaustionRatio(exhaustionRatio, exhaustion, exhaustionData.maxExhaustion);

                // Sleep
                if (m_bCanBeSleepExhausted)
                {
                    sleepExhaustion = m_sleepExhaustionManager.IncreaseExhaustionOverTime(
                        sleepExhaustion, exhaustionData.sleepIncreaseRateOverNightRatio.Evaluate(nightRatio));
                }
                // Hunger
                if (m_bCanBeHungerExhausted)
                {
                    hungerExhaustion = m_hungerExhaustionManager.IncreaseExhaustionOverTime(
                        hungerExhaustion, exhaustionData.hungerIncreaseRate);
                }
                // Thirst
                if (m_bCanBeThirstExhausted)
                {
                    thirstExhaustion = m_thirstExhaustionManager.IncreaseExhaustionOverTime(
                        thirstExhaustion, exhaustionData.thirstIncreaseRateOverTemperature.Evaluate(roomTemperature));
                }
                // Body Temperature
                if (m_bCanBeBodyTemperatureExhausted)
                {
                    bodyTemperatureExhaustion = m_bodyTemperatureExhaustionManager.ModifyExhaustionOverTime(
                        bodyTemperatureExhaustion, 
                        exhaustionData.bodyTemperatureOverTargetTemperature.Evaluate(roomTemperature),
                        exhaustionData.upBodyTemperatureSpeed,
                        exhaustionData.downBodyTemperatureSpeed);
                }

            }
        }



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

        // Modifies exhaustion when sleep event is triggered
        private void HandleSleepStarted()
        {
            sleepExhaustion = m_sleepExhaustionManager.ModifyExhaustionByAmount(
                sleepExhaustion, -1 * exhaustionData.sleepDecreaseAmountOverNightRatio.Evaluate(nightRatio));
        }

        // Modify exhaustion when eating
        public void ModifyHungerExhaustion(float amount)
        {
            hungerExhaustion = m_hungerExhaustionManager.ModifyExhaustionByAmount(
                hungerExhaustion, amount);
        }

        // Modify exhaustion when drinking
        public void ModifyThirstExhaustion(float amount)
        {
            thirstExhaustion = m_thirstExhaustionManager.ModifyExhaustionByAmount(
                thirstExhaustion, amount);
        }

    }
}
