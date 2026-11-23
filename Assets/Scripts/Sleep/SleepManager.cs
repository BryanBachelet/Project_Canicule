using Canicule.Rooms;
using System;
using UnityEngine;

namespace Canicule
{
    public class SleepManager : MonoBehaviour
    {
        public event Action OnSleepStarted;

        [SerializeField] private SleepData m_sleepData;
        [SerializeField] private float m_roomTemperature;
 

        [SerializeField] private SleepingUIPhase m_sleepingUIPhase;

        private RoomManager m_roomManager;
        private AnimationCurve m_sleepChanceOverRoomTemperature;

        private float m_sleepCooldown = 0.0f;
        private float m_sleepChance = 0.0f;
        private float m_sleepChanceModifier = 0.0f;
        private float m_requieredSleepChance = 0.0f;
        private float m_sleepChancePillsModifier = 0.0f;
        private float m_sleepChanceWithoutPillsModifier = 0.0f;
        private float m_sleepChanceEnergyDrinkModifier = 0.0f;
        private float m_sleepTimer = 0.0f;

        private bool m_bCanAttempSleep = true;
        private bool m_bIsSleepTimerRunning = false;
        private bool m_hasTakenPills = false;


        [Header("Debug Feature")]
        [SerializeField] private bool m_alwaysSucceedSleep = false;


        #region Unity Methods
        private void Start()
        {
            m_sleepChanceOverRoomTemperature = m_sleepData.sleepChanceOverRoomTemperature;
            m_sleepCooldown = m_sleepData.sleepCooldown;
            m_sleepTimer = m_sleepCooldown;
            m_sleepingUIPhase.OnEndSleepingPhase.AddListener(EndSleepingPhase); 
        }

        private void Awake()
        {
            m_roomManager = GameManager.GetRoomManager();
        }

        private void Update()
        {
            ComputeSleepTimer();
        }
        #endregion

        // Checkes if the player succeeded their sleep try
        public bool AttemptSleep()
        {
            m_roomTemperature = m_roomManager.GetCurrentRoomPlayer().GetHeatManager().GetDegree();

            if (m_bCanAttempSleep)
            {
                ComputeSleepChance();
                m_requieredSleepChance = 1 - m_sleepChanceOverRoomTemperature.Evaluate(m_roomTemperature);

                if (m_sleepChance >= m_requieredSleepChance || m_alwaysSucceedSleep)
                {
                    Debug.Log("[Sleep Manager] Sleep success!");
                    Debug.Log("[Sleep Manager] Requiered chance was: " + m_requieredSleepChance);
                    OnSleepStarted?.Invoke();
                    GameManager.GetPlayer().GetComponent<Character>().ChangeState(CharacterState.SLEEPING);
                    GameManager.RequestToSwitchGamePhase();
                    m_sleepingUIPhase.LaunchSleepingPhase();

                    // Reset Sleeping Pills Effect
                    m_hasTakenPills = false;
                    m_sleepChancePillsModifier = 0f;

                    // Reset Energy Drink Effect
                    m_sleepChanceEnergyDrinkModifier = 0f;
                    return true;
                }
                else
                {
                    Debug.Log("[Sleep Manager] Fail to sleep");
                    Debug.Log("[Sleep Manager] Requiered chance was: " + m_requieredSleepChance);
                    m_bIsSleepTimerRunning = true;
                    
                }

                m_bCanAttempSleep = false;
            }
            else
            {
                Debug.Log("[Sleep Manager] Attempting sleep unauthorized. Remaining cooldown: " + m_sleepTimer + " seconds");
            }
            return false;

        }

        // Computes the timer cooldown when failed a sleep
        private void ComputeSleepTimer()
        {
            if (!m_bIsSleepTimerRunning) return;

            m_sleepTimer -= Time.deltaTime;

            if (m_sleepTimer <= 0f)
            {
                m_sleepTimer = m_sleepCooldown;
                m_bCanAttempSleep = true;
                m_bIsSleepTimerRunning = false;
            }

        }

        private void ComputeSleepChance()
        {
            if (m_hasTakenPills)
            {
                m_sleepChance = UnityEngine.Random.Range(0f, 1f)
                        + m_sleepChanceModifier
                        + m_sleepChancePillsModifier
                        + m_sleepChanceEnergyDrinkModifier;
            }
            else
            {
                m_sleepChance = UnityEngine.Random.Range(0f, 1f)
                        + m_sleepChanceModifier
                        + m_sleepChanceWithoutPillsModifier
                        + m_sleepChanceEnergyDrinkModifier;
                Debug.Log("[Sleep Chance] Sleep chance was " + m_sleepChance);
                Debug.Log("[Sleep Chance] base modifier: " + m_sleepChanceModifier);
                Debug.Log("[Sleep Chance] without pills modifier: " + m_sleepChanceWithoutPillsModifier);
                Debug.Log("[Sleep Chance] energy drink modifier: " + m_sleepChanceEnergyDrinkModifier);

            }
        }
        
        public void EndSleepingPhase()
        {
            GameManager.GetPlayer().GetComponent<Character>().ChangeState(CharacterState.IDLE);
        }

        public void ModifySleepChanceModifier(float amount)
        {
            m_sleepChanceModifier += amount;
            Debug.Log("[Sleep Manager] sleep chance modifier = " + m_sleepChanceModifier);
        }

        public void ModifySleepChancePillsModifier(float amount)
        {
            m_sleepChancePillsModifier += amount;
            Debug.Log("[Sleep Manager] sleep chance pills modifier = " + m_sleepChancePillsModifier);
        }

        public void ModifySleepChanceWithoutPillsModifier(float amount)
        {
            m_sleepChanceWithoutPillsModifier += amount;
            Debug.Log("[Sleep Manager] sleep chance without pills modifier = " + m_sleepChanceWithoutPillsModifier);
        }

        public void ModifySleepChanceEnergyDrink(float amount)
        {
            m_sleepChanceEnergyDrinkModifier += amount;
            Debug.Log("[Sleep Manager] sleep chance energy drink modifier = " + m_sleepChanceEnergyDrinkModifier);
        }

        public void SetHasTakenPills(bool b)
        {
            m_hasTakenPills = b;
            Debug.Log("[Sleep Manager] Has taken pills? " + m_hasTakenPills);
        }

        public bool CanSleep()
        {
            return m_bCanAttempSleep;
        }

        public float GetRatioCooldownSleepAttemp()
        {
            return m_sleepTimer / m_sleepCooldown;
        }
    }
}
