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
        private float m_tryChance = 0.0f;
        private float m_sleepTimer = 0.0f;

        private bool m_bCanAttempSleep = true;
        private bool m_bIsSleepTimerRunning = false;

        #region Unity Methods
        private void Start()
        {
            m_sleepTimer = m_sleepCooldown;
            m_sleepChanceOverRoomTemperature = m_sleepData.sleepChanceOverRoomTemperature;
            m_sleepCooldown = m_sleepData.sleepCooldown;
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
        public void AttemptSleep()
        {
            m_roomTemperature = m_roomManager.GetCurrentRoomPlayer().GetHeatManager().GetDegree();

            if (m_bCanAttempSleep)
            {
                m_tryChance = UnityEngine.Random.Range(0f, 1f);

                if (m_tryChance <= m_sleepChanceOverRoomTemperature.Evaluate(m_roomTemperature))
                {
                    Debug.Log("[Sleep Manager] Sleep success!");
                    OnSleepStarted?.Invoke();
                    GameManager.GetPlayer().GetComponent<Character>().ChangeState(CharacterState.SLEEPING);
                    GameManager.RequestToSwitchGamePhase();
                    m_sleepingUIPhase.LaunchSleepingPhase();
                   
                }
                else
                {
                    Debug.Log("[Sleep Manager] Fail to sleep");
                    m_bIsSleepTimerRunning = true;
                }

                m_bCanAttempSleep = false;
            }
            else
            {
                Debug.Log("[Sleep Manager] Attempting sleep unauthorized");
            }
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
        
        public void EndSleepingPhase()
        {
            GameManager.GetPlayer().GetComponent<Character>().ChangeState(CharacterState.IDLE);
        }
    }
}
