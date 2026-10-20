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

        public float exhaustion;
        //[HideInInspector]
        public float sleepExhaustion;
        //[HideInInspector]
        public float hungerExhaustion;
        //[HideInInspector]
        public float thirstExhaustion;
        //[HideInInspector]
        public float bodyTemperatureExhaustion;

        private bool m_bCanBeExhausted = false;
        private bool m_bCanBeSleepExhausted = false;
        private bool m_bCanBeHungerExhausted = false;
        private bool m_bCanBeThirstExhausted = false;
        private bool m_bCanBeBodyTemperatureExhausted = false;

        private SleepExhaustionManager m_sleepExhaustionManager;
        private SleepManager m_sleepManager;


        private void Awake()
        {
            m_sleepManager = Object.FindAnyObjectByType<SleepManager>();
            m_sleepExhaustionManager = GetComponent<SleepExhaustionManager>();
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

                if (m_bCanBeSleepExhausted)
                {
                    m_sleepExhaustionManager.ComputeSleepExhaustion();
                }
            }
        }



        // Computes the value of exhaustion
        private float ComputeExhaustion()
        {
            return sleepExhaustion + hungerExhaustion + thirstExhaustion + bodyTemperatureExhaustion;
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

        // Modifies exhaustion when sleeping starts
        private void HandleSleepStarted()
        {
            m_sleepExhaustionManager.DecreaseExhaustionWhenSleeping();
        }

    }
}
