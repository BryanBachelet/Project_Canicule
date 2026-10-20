using System;
using UnityEngine;

namespace Canicule
{
    public class SleepManager : MonoBehaviour
    {
        public event Action OnSleepStarted;

        [SerializeField] private SleepData m_sleepData;
        [SerializeField] private float m_roomTemperature; // To be replaced with real value

        private AnimationCurve m_sleepChanceOverRoomTemperature;
        private float m_sleepCooldown;
        private float m_tryChance;
        private float m_sleepTimer = 0f;
        private bool m_bCanAttempSleep = true;
        private bool m_bIsSleepTimerRunning = false;

        private void Start()
        {
            m_sleepTimer = m_sleepCooldown;
            m_sleepChanceOverRoomTemperature = m_sleepData.sleepChanceOverRoomTemperature;
            m_sleepCooldown = m_sleepData.sleepCooldown;
        }

        private void Update()
        {
            ComputeSleepTimer();
        }

        // Checkes if the player succeeded their sleep try
        public void AttemptSleep()
        {
            if (m_bCanAttempSleep)
            {
                m_tryChance = UnityEngine.Random.Range(0f, 1f);

                if (m_tryChance <= m_sleepChanceOverRoomTemperature.Evaluate(m_roomTemperature))
                {
                    Debug.Log("Sleep success!");
                    OnSleepStarted?.Invoke();

                }
                else
                {
                    Debug.Log("Fail to sleep");
                    m_bIsSleepTimerRunning = true;
                }

                m_bCanAttempSleep = false;
            }
            else
            {
                Debug.Log("Attempting sleep unauthorized");
            }
        }

        // Computes the timer cooldown when failed a sleep
        private void ComputeSleepTimer()
        {
            if (m_bIsSleepTimerRunning)
            {
                m_sleepTimer -= Time.deltaTime;

                if(m_sleepTimer <= 0f)
                {
                    m_sleepTimer = m_sleepCooldown;
                    m_bCanAttempSleep = true;
                    m_bIsSleepTimerRunning = false;
                }
            }
        }
    }
}
