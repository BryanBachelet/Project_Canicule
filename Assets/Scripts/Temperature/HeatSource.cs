using UnityEngine;

namespace Canicule.Temperature
{
    public class HeatSource : MonoBehaviour
    {
        [SerializeField] private float m_temperature;

        [SerializeField] private bool m_isActive = true;

        private Rooms.Room m_room;

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

            float m_heatScore = m_temperature - refTemperature;
            return m_heatScore;
        }

        public void ActiveHeatSource() { m_isActive = true; }
        public void DeactiveHeatSource() { m_isActive = false; }


    }
}
