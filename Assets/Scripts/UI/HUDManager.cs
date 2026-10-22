using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Canicule
{
    public class HUDManager : MonoBehaviour
    {

        [SerializeField] private RectTransform m_exhaustionGaugeHolder;
        [SerializeField] private List<RectTransform> m_exhaustionGaugesRect = new List<RectTransform>();

        private List<ExhaustionGauge>  m_exhaustionGauges = new List<ExhaustionGauge>();
        private ExhaustionManager m_exhaustionManager;
        private float m_targetWidth = 0f;
        private float m_exhaustionToWidthConverter;

        private void Awake()
        {
            m_exhaustionManager = FindAnyObjectByType<ExhaustionManager>();

            foreach (RectTransform rectTransform in m_exhaustionGaugesRect)
            {
                m_exhaustionGauges.Add(rectTransform.GetComponent<ExhaustionGauge>());
            }

        }

        private void Start()
        {
            m_exhaustionToWidthConverter = m_exhaustionGaugeHolder.rect.width / m_exhaustionManager.exhaustionData.maxExhaustion;
        }

        void Update()
        {
            SetExhaustionGaugesPositions();
            UpdateExhaustionGaugesValues();

        }

        // Sets the position of the gauges next to each other
        private void SetExhaustionGaugesPositions()
        {
            for (int i = 0; i < m_exhaustionGaugesRect.Count; ++i)
            {
                if (i != 0)
                {
                    for (int k = 0; k < i; ++k)
                    {
                        m_targetWidth += m_exhaustionGaugesRect[k].rect.width;
                    }

                    m_exhaustionGaugesRect[i].anchoredPosition = new Vector2(m_targetWidth, m_exhaustionGaugesRect[i].anchoredPosition.y);
                    m_targetWidth = 0;
                }
            }
        }

        // Updates the exhaustion values of all gauges
        private void UpdateExhaustionGaugesValues()
        {
            // Sleep
            m_exhaustionGauges[0].UpdateExhaustionGaugeValue(m_exhaustionToWidthConverter, m_exhaustionManager.sleepExhaustion);

            // Hunger
            m_exhaustionGauges[1].UpdateExhaustionGaugeValue(m_exhaustionToWidthConverter, m_exhaustionManager.hungerExhaustion);

            // Thirst
            m_exhaustionGauges[2].UpdateExhaustionGaugeValue(m_exhaustionToWidthConverter, m_exhaustionManager.thirstExhaustion);

            // Body Temperature
            m_exhaustionGauges[3].UpdateExhaustionGaugeValue(m_exhaustionToWidthConverter, m_exhaustionManager.bodyTemperatureExhaustion);
        }
    }
}
