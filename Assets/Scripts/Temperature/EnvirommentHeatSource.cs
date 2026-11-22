using UnityEngine;
using UnityEngine.Events;

namespace Canicule.Temperature
{
    [System.Serializable]
    public struct EnvirommentDayTemperature
    {
        public AnimationCurve m_temperaturePerHour;
        public float m_morningTemperature;
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
            GameManager.SubcribeToOnGameEventChange(OnGameStateChange);
        }

        public void Update()
        {
            if (GameManager.GetCurrentGamePhase() == GamePhase.NIGHT)
            {
                UpdateHeatSource();
            }
        }
        #endregion

        public void UpdateHeatSource()
        {
            int day = m_nightDayManager.GetDay();
            float hourRatio = m_nightDayManager.GetHourRatio();
            float degree = m_dayTemperature[day].m_temperaturePerHour.Evaluate(hourRatio);
            OnTemperatureUpdate?.Invoke(degree);
        }

        private void OnGameStateChange(GamePhase phase)
        {
            if(phase == GamePhase.MORNING)
            {
                int day = m_nightDayManager.GetDay();
                OnTemperatureUpdate?.Invoke(m_dayTemperature[day].m_morningTemperature);
            }
           
        }

    }
}
