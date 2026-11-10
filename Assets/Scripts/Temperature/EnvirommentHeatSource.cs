using UnityEngine;
using UnityEngine.Events;

namespace Canicule.Temperature
{
    [System.Serializable]
    public struct EnvirommentDayTemperature
    {
        public AnimationCurve m_temperaturePerHour; 
    }

    public class EnvirommentHeatSource : MonoBehaviour
    {
        private NightDayManager m_nightDayManager = null;

        [SerializeField] private EnvirommentDayTemperature[] m_dayTemperature;

        [HideInInspector] public UnityEvent<float> OnTemperatureUpdate;

        #region Unity Functions
        private void Start()
        {
            m_nightDayManager = GameManager.GetNightDayManager();
        }

        public void Update()
        {
            UpdateHeatSource();
        }
        #endregion

        public void UpdateHeatSource()
        {
            int day = m_nightDayManager.GetDay();
            float hourRatio = m_nightDayManager.GetHourRatio();
            float degree = m_dayTemperature[day].m_temperaturePerHour.Evaluate(hourRatio);
            OnTemperatureUpdate?.Invoke(degree);
        }

    }
}
