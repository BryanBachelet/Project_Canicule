using Canicule.Temperature;
using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class SunCycle : MonoBehaviour
    {
        [System.Serializable]
        private struct DayTemperatureVariation
        {
            public AnimationCurve m_animation;
        }

        [Header("Night Parameters")]
        [SerializeField] private float m_hourDuration = 0;
        private float m_currentNightTimer = 0;

        [SerializeField] private int startNightHour = 20;
        [SerializeField] private int endNightHour = 10;
        private const int m_dayHourLimit = 24;
        private int m_currentHour = 0;

        private int m_currentDay;
        [HideInInspector] public UnityAction<int> OnEndDay;

        [Header("Day Temperature")]
        [SerializeField]
        private DayTemperatureVariation[] m_dayTemperatureArray;
        private HeatSource m_heatSource;

        #region Unity Functions
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_heatSource = GetComponent<HeatSource>();
        }

        private void Update()
        {
            UpdateNight();
        }
        #endregion

        private void UpdateTemperature()
        {
            int index = Mathf.Max(m_currentDay, m_dayTemperatureArray.Length-1);
            float currentDegree = m_dayTemperatureArray[index].m_animation.Evaluate(m_currentHour);
            m_heatSource.SetTemperature(currentDegree);
        }
        private void UpdateNight()
        {
            m_currentNightTimer += Time.deltaTime;
            if(m_currentNightTimer > m_hourDuration)
            {
                m_currentNightTimer = 0.0f;
                AddHour();
            }
        }

        #region Hours Functions
        private void SetHour(int hour)
        {
            m_currentHour = hour;
        }

        private void AddHour()
        {
            m_currentHour++;
            if(m_currentHour >m_dayHourLimit)
            {
                m_currentHour = 0;
            }
        }

        public int GetHour() { return m_currentHour; }

        public void EndDay()
        {
            ChangeDay();
            OnEndDay?.Invoke(m_currentDay);
        }

        private void ChangeDay()
        {
            m_currentDay++;
        }

        #endregion






    }
}
