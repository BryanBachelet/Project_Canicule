using System.Collections.Generic;
using UnityEngine;

namespace Canicule.Temperature
{
    [System.Serializable]
    public class HeatManager
    {

        private float m_currentDegree;

        private float m_currentHeatScore;
        private float m_nextStepHeatScore;
        private float m_prevStepHeatScore;

        private float m_heatScoreResistance;

        private List<HeatSource> m_heatSourcesList = new List<HeatSource>();

        [Header("Debugs parameters")]
        [SerializeField] private bool m_activeTemperaturDebug = false;
        [SerializeField] private bool m_activeRegisterDebug = false;

        public void InitHeatManager(float temperature, float heatResistance)
        {
            m_currentDegree = temperature;
            m_prevStepHeatScore = (- HeatGeneralData.GetValueNextDegreeHeatScore());
            m_nextStepHeatScore = HeatGeneralData.GetValueNextDegreeHeatScore();

            m_heatScoreResistance = heatResistance;
        }

        public void UpdateHeatManager()
        {
            float nextHeatScore = m_currentHeatScore;

            nextHeatScore -= m_heatScoreResistance;

            for (int i = 0; i < m_heatSourcesList.Count; i++)
            {
                float heatScore = m_heatSourcesList[i].ComputeHeatScore(m_currentDegree);
               
                if (m_activeTemperaturDebug)
                    Debug.Log($"[Temperature] Add Heat {heatScore} score from {m_heatSourcesList[i].gameObject}");

                nextHeatScore += heatScore;
            }

            if (nextHeatScore > m_nextStepHeatScore)
            {
                m_currentHeatScore = nextHeatScore - m_nextStepHeatScore;
                IncreaseDegree();
                return;
            }

            if (nextHeatScore < m_prevStepHeatScore)
            {
                m_currentHeatScore = nextHeatScore - m_prevStepHeatScore;
                DecreaseDegree();
                return;
            }

            m_currentHeatScore = nextHeatScore;
            
            if (m_activeTemperaturDebug)
                Debug.Log($"[Temperature] CurrentHeatScore : {m_currentHeatScore}");

        }

        private void IncreaseDegree()
        {
            m_currentDegree++;
            m_nextStepHeatScore = HeatGeneralData.GetValueNextDegreeHeatScore();

            if (m_activeTemperaturDebug)
                Debug.Log($"[Temperature] Increase Degree -> Prev {m_currentDegree - 1} / Current {m_currentDegree}");
        }

        private void DecreaseDegree()
        {
            m_currentDegree--;
            m_prevStepHeatScore = (-HeatGeneralData.GetValueNextDegreeHeatScore());
            
            if (m_activeTemperaturDebug)
                Debug.Log($"[Temperature] Decrease Degree -> Prev {m_currentDegree +1 } / Current {m_currentDegree}");
        }

        public void RegisterHeatSource(HeatSource heatSource)
        {
            if (!heatSource || m_heatSourcesList.Contains(heatSource)) return;

            if (m_activeRegisterDebug)
                Debug.Log($"[Temperature] Register new heat source {heatSource.gameObject.name}");

            m_heatSourcesList.Add(heatSource);
        }

        public void UnregisterHeatSource(HeatSource heatSource)
        {
            if (!heatSource || !m_heatSourcesList.Contains(heatSource)) return;

            if (m_activeRegisterDebug)
                Debug.Log($"[Temperature] Unregister heat source {heatSource.gameObject.name}");

            m_heatSourcesList.Remove(heatSource);
        }

        #region Getter Functions
        public float GetDegree()
        {
            return m_currentDegree;
        }
        #endregion


    }
}
