using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class NightDayManager : MonoBehaviour
    {
        private bool m_isNightActive = false;

        [SerializeField] private float m_nightTotalDuration = 600f; // in seconds
        private float m_nightRatio = 0.0f;
        private float m_nightTotalTimer = 0.0f;
        private int m_dayCount = 0;

        [HideInInspector] public UnityAction OnStartNight;
        [HideInInspector] public UnityAction OnEndNight;

        [Header("Hours Parameters")]
        [SerializeField] private int m_nightStartHour = 18;
        [SerializeField] private int m_nightEndHour = 8;
        private const int m_maxHour = 24;
        private int m_currentHour = 0;
        private float m_hourDuration = 0;
        private float m_hourTimer = 0.0f;

        [HideInInspector] public UnityAction<int> OnHourChange;

        #region Unity Methods
        private void Start()
        {
            InitComponent();
            StartNight();
        }

        private void Update()
        {
            if (!m_isNightActive)
            {
                return;
            }

            ComputeNightRatio();
            UpdateHour();
        }
        #endregion

        #region Night Management
        private void InitComponent()
        {
            m_hourDuration = m_nightEndHour + (m_maxHour - m_nightStartHour) / m_nightTotalDuration;
        }

        private void ComputeNightRatio()
        {
            m_nightTotalTimer += Time.deltaTime;
            m_nightRatio = m_nightTotalTimer / m_nightTotalDuration;
        }
        private void UpdateHour()
        {
            m_hourTimer += Time.deltaTime;
            if (m_hourTimer > m_hourDuration)
            {
                m_hourTimer = 0;
                AddHour();
            }
        }

        private void AddHour()
        {
            m_currentHour++;
            if (m_currentHour > m_maxHour)
            {
                m_currentHour = m_maxHour - m_currentHour;
            }

            OnHourChange?.Invoke(m_currentHour);

            if (m_currentHour == m_nightEndHour)
            {
                FinishNight();
            }
        }

        public void SetHour(int hour)
        {
            if (hour >= m_maxHour)
            {
                m_currentHour = 0;
                return;
            }
            if (hour > m_nightEndHour && hour < m_nightStartHour)
            {
                m_currentHour = m_nightStartHour;
            }

            m_currentHour = hour;

        }

        public void StartNight()
        {
            m_nightTotalTimer = 0.0f;
            m_currentHour = m_nightStartHour;
            m_isNightActive = true;
            OnStartNight?.Invoke();
            m_dayCount++;
        }

        private void FinishNight()
        {
            m_isNightActive = false;
            OnEndNight?.Invoke();
        }
        #endregion

        #region Getter Functions
        public float GetNightRatio()
        {
            return m_nightRatio;
        }

        public float GetHourRatio()
        {
            return m_currentHour + (m_hourTimer / m_hourDuration);
        }

        public int GetHour()
        {
            return m_currentHour;
        }

        public int GetDay()
        {
            return m_dayCount;
        }
        #endregion
    }
}
