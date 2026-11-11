using UnityEngine;

namespace Canicule.Temperature
{
    public class HeatSource : MonoBehaviour
    {
        [SerializeField] private float m_temperature;

        [SerializeField] private bool m_isActive = true;

        [SerializeField] private float baseHeatScoreRatio = 1.0f;

        private Rooms.Room m_room;

#if UNITY_EDITOR || UNITY_DEVELOPMENT
        private float m_lastHeatScore = 0.0f;
#endif

        #region Unity Function
        public void Start()
        {
            m_room = GetComponentInParent<Rooms.Room>();

            if (!m_room) return;

            HeatManager heatManager = m_room.GetHeatManager();
            heatManager.RegisterHeatSource(this);
        }
        #endregion

        public float ComputeHeatScore(float refTemperature)
        {
            if (!m_isActive) return 0;

            float sign = Mathf.Sign((m_temperature - refTemperature));
            float baseHeatScore = HeatGeneralData.GetPositifHeatScoreGain();
            if (sign < 0)
            {
                baseHeatScore = HeatGeneralData.GetNegatifHeatScoreGain();
            }

            float m_heatScore = baseHeatScore + ((m_temperature - refTemperature) * HeatGeneralData.GetRatioPerDegreeGap());
#if UNITY_EDITOR || UNITY_DEVELOPMENT
            m_lastHeatScore = m_heatScore * baseHeatScoreRatio;
#endif
            return m_heatScore * baseHeatScoreRatio;
        }

        public void ActiveHeatSource() { m_isActive = true; }

        public void DeactiveHeatSource() { m_isActive = false; }

        public float GetTemperature() { return m_temperature; }

        public void SetTemperature(float degree)
        {
            m_temperature = degree;
        }

        public void SetTemperatureScoreRatio(float scoreRatio)
        {
            baseHeatScoreRatio = Mathf.Clamp(scoreRatio, 0.001f, float.MaxValue);
        }

#region Debug Functions

#if UNITY_EDITOR || UNITY_DEVELOPMENT

        public void OnDrawGizmosSelected()
        {
            BorsalinoTools.ScreenDebuggerTool.AddMessage($"[HeatSource] Current temperutre {m_temperature}");
            BorsalinoTools.ScreenDebuggerTool.AddMessage($"[HeatSource] Current gain degree score point {m_lastHeatScore}");
        }
#endif

#endregion

    }

}
