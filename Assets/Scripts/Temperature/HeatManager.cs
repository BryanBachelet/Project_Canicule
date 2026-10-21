using System.Collections.Generic;
using UnityEngine;

namespace Canicule.Temperature
{
    public class HeatManager
    {
        private float m_degree;

        private float m_currentHeatScore;
        private float m_nextStepHeatScore;
        private float m_prevStepHeatScore;

        private float m_heatScoreResistance;

        private List<HeatSource> m_heatSourcesList = new List<HeatSource>();


        public void InitHeatManager(float temperature, float heatResistance)
        {
            m_degree = temperature;
            m_prevStepHeatScore = -1 * (m_degree - 1);
            m_nextStepHeatScore = m_degree + 1;

            m_heatScoreResistance = heatResistance;
        }

        public void UpdateHeatManager()
        {
            float nextHeatScore = m_currentHeatScore;

            nextHeatScore -= m_heatScoreResistance;

            for (int i = 0; i < m_heatSourcesList.Count; i++)
            {
                nextHeatScore += m_heatSourcesList[i].ComputeHeatScore(m_degree);
            }

            if(nextHeatScore > m_nextStepHeatScore)
            {
                m_currentHeatScore = nextHeatScore - m_nextStepHeatScore;
                IncreaseDegree();
                return;
            }

            if(nextHeatScore < m_prevStepHeatScore)
            {
                m_currentHeatScore = nextHeatScore - m_prevStepHeatScore;
                DecreaseDegree();
                return;
            }

            m_currentHeatScore = nextHeatScore;

        }

        private void IncreaseDegree()
        {
            m_degree++;
            m_nextStepHeatScore = m_degree+1;
        }

        private void DecreaseDegree()
        {
            m_degree--;
            m_prevStepHeatScore = -1* (m_degree-1);
        }

        public void RegisterHeatSource(HeatSource heatSource)
        {
            if (!heatSource || m_heatSourcesList.Contains(heatSource)) return;

            m_heatSourcesList.Add(heatSource);
        }

        public void UnregisterHeatSource(HeatSource heatSource)
        {
            if (!heatSource || !m_heatSourcesList.Contains(heatSource)) return;

            m_heatSourcesList.Add(heatSource);
        }

        #region Getter Functions
        public float GetDegree()
        {
            return m_degree;
        }
        #endregion


    }
}
