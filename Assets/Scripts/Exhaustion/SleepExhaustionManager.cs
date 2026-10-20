using UnityEngine;

namespace Canicule
{
    public class SleepExhaustionManager : MonoBehaviour
    {
        private ExhaustionManager m_exhaustionManager;

        private void Awake()
        {
            m_exhaustionManager = GetComponent<ExhaustionManager>();
        }

        // Computes sleep exhaustion based on night ratio
        public void ComputeSleepExhaustion()
        {
            m_exhaustionManager.sleepExhaustion 
                += m_exhaustionManager.exhaustionData.sleepIncreaseRateOverNightRatio.Evaluate(m_exhaustionManager.nightRatio)
                * Time.deltaTime;
        }

        // Decreases sleep exhaustion based on night ratio
        public void DecreaseExhaustionWhenSleeping()
        {
            m_exhaustionManager.sleepExhaustion 
                -= m_exhaustionManager.exhaustionData.sleepDecreaseAmountOverNightRatio.Evaluate(m_exhaustionManager.nightRatio);
            
            if(m_exhaustionManager.sleepExhaustion < 0)
            {
                m_exhaustionManager.sleepExhaustion = 0;
            }

            Debug.Log("Sleep exhaustion decreased by " + 
                m_exhaustionManager.exhaustionData.sleepDecreaseAmountOverNightRatio.Evaluate(m_exhaustionManager.nightRatio));
        }

    }
}
