using UnityEngine;

namespace Canicule
{
    public class NightDayManager : MonoBehaviour
    {
        [SerializeField] private float m_nightDuration = 600f; // in seconds
        private float m_nightRatio;
        private float m_timer;

        #region Unity Methods
        private void Start()
        {
            InitNight();
        }

        private void Update()
        {
            ComputeNightRatio();
        }
        #endregion

        #region Night Management
        private void InitNight()
        {
            m_timer = 0;
        }

        private void ComputeNightRatio()
        {
            m_timer += Time.deltaTime;
            m_nightRatio = m_timer / m_nightDuration;
        }
        #endregion

        #region Getter
        public float GetNightRatio()
        {
            return m_nightRatio;
        }
        #endregion
    }
}
