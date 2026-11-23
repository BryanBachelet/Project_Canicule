using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class NightDayManager : MonoBehaviour
    {
        [SerializeField] private bool m_isNightActive = false;

        [SerializeField] private float m_nightTotalDuration = 600f; // in seconds
        private float m_nightRatio = 0.0f;
        private float m_nightTotalTimer = 0.0f;
        private int m_dayCount = 0;

        [HideInInspector] public UnityAction OnStartNight;
        [HideInInspector] public UnityAction OnEndNight;

        [Header("Hours Parameters")]
        [SerializeField] private int m_nightStartHour = 18;
        [SerializeField] private int m_nightEndHour = 8;
        [SerializeField] private string m_letterHour = "h";
        private const int m_maxHour = 24;
        private int m_currentHour = 0;
        private float m_hourDuration = 0;
        private float m_hourTimer = 0.0f;
        private const int m_minMinutes = 0;
        private const int m_maxMinutes = 59;

        [HideInInspector] public UnityAction<int> OnHourChange;

        #region Unity Methods
        private void Start()
        {
            InitComponent();
            StartNight();
        }

        private void Update()
        {
            Debug.Log(m_isNightActive);

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
            m_hourDuration = m_nightTotalDuration / (m_nightEndHour + (m_maxHour - m_nightStartHour));
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

        public string GetDispayableHour()
        {
            // Hours

            int hours = GetHour();
            string hoursText = hours.ToString();
            if (hours >= 0f && hours < 10f)
            {
                hoursText = "0" + hoursText;
            }
            else if (hours == 24)
            {
                hoursText = "00";
            }

            // Minutes

            float m_hourRatio = GetHourRatio() - GetHour();
            float minutes = Mathf.Round(Mathf.Lerp(m_minMinutes, m_maxMinutes, m_hourRatio) / 10f) * 10f;

            if (minutes > 50f)
            {
                minutes = 50f;
            }

            string minutesText = minutes.ToString();

            if (minutes < 10f)
            {
                minutesText = "0" + minutesText;
            }

            return hoursText + m_letterHour + minutesText;
        }

        public int GetDay()
        {
            return m_dayCount;
        }

        public bool GetIsNightActive()
        {
            return m_isNightActive;
        }
        #endregion
    }
}
