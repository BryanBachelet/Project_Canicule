using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;

namespace Canicule
{
    public class HUDManager : MonoBehaviour
    {

        [SerializeField] private RectTransform m_exhaustionGaugeHolder;
        [SerializeField] private List<RectTransform> m_exhaustionGaugesRect = new List<RectTransform>();
        [SerializeField] private Animator m_infobarAC;
        [SerializeField] private CanvasGroup m_exhaustionGaugesCanvaGroup;
        [SerializeField] private float m_exhaustionGaugesTemporaryDuration = 4f;
        [SerializeField] private float m_exhaustionGaugesLerpDuration = 1f;

        private List<ExhaustionGauge>  m_exhaustionGauges = new List<ExhaustionGauge>();
        private ExhaustionManager m_exhaustionManager;
        private NightDayManager m_nightDayManager;
        private float m_targetWidth = 0f;
        private float m_exhaustionToWidthConverter;
        private float m_temporaryTimer = 0f;
        private float m_temporaryLerpTimer = 0f;
        private bool b_isTemporaryTimerRunning = false;
        private bool b_isTemporaryGaugesDisplayed = false;

        #region Unity Methods
        private void Awake()
        {
            m_exhaustionManager = GameManager.GetExhaustionManager();
            m_nightDayManager = GameManager.GetNightDayManager();

            foreach (RectTransform rectTransform in m_exhaustionGaugesRect)
            {
                m_exhaustionGauges.Add(rectTransform.GetComponent<ExhaustionGauge>());
            }

        }

        private void OnEnable()
        {
            m_nightDayManager.OnStartNight += UpdateExhaustionGaugesVisibility;
            m_nightDayManager.OnEndNight += UpdateExhaustionGaugesVisibility;
            m_exhaustionManager.OnModifyExhaustion += ManageTemporaryVisibilityExhaustionGauges;
        }

        private void OnDisable()
        {
            m_nightDayManager.OnStartNight -= UpdateExhaustionGaugesVisibility;
            m_nightDayManager.OnEndNight -= UpdateExhaustionGaugesVisibility;
            m_exhaustionManager.OnModifyExhaustion -= ManageTemporaryVisibilityExhaustionGauges;
        }

        private void Start()
        {
            m_exhaustionToWidthConverter = m_exhaustionGaugeHolder.rect.width / m_exhaustionManager.exhaustionData.maxExhaustion;
        }

        void Update()
        {
            SetExhaustionGaugesPositions();
            UpdateExhaustionGaugesValues();

            if (b_isTemporaryTimerRunning)
            {
                UpdateTemporaryTimer();
            }

            TemporarilyDisplayingGauges();
        }
        #endregion

        #region Exhaustion Gauges
        // Update the visibility of the exhaustion gauges
        private void UpdateExhaustionGaugesVisibility()
        {
            // Reactivate the animator for the night phase
            if (m_nightDayManager.GetIsNightActive())
            {
                m_infobarAC.enabled = true;
            }

            m_infobarAC.SetBool("isNightActive", m_nightDayManager.GetIsNightActive());
        }

        // Manage temporary visibility of the exhaustion gauges
        private void ManageTemporaryVisibilityExhaustionGauges()
        {
            m_temporaryTimer = m_exhaustionGaugesTemporaryDuration;

            if (!b_isTemporaryTimerRunning)
            {
                m_temporaryLerpTimer = 0;
            }

            b_isTemporaryTimerRunning = true;
        }

        private void UpdateTemporaryTimer()
        {
            m_temporaryTimer -= Time.deltaTime;

            if(m_temporaryTimer < 0)
            {
                m_temporaryLerpTimer = 0;
                b_isTemporaryTimerRunning = false;
            }
        }

        /*
        private void ManageTemporaryVisibility()
        {
            if (!b_isTemporaryGaugesDisplayed)
            {
                b_isTemporaryGaugesDisplayed = true;
                m_temporaryLerpTimer = 0;
            }
        }
        */

        private void TemporarilyDisplayingGauges()
        {
            if (!m_infobarAC.isActiveAndEnabled)
            {
                m_temporaryLerpTimer += Time.deltaTime;
                float timerRatio = m_temporaryLerpTimer / m_exhaustionGaugesLerpDuration;

                if (b_isTemporaryTimerRunning)
                {
                    m_exhaustionGaugesCanvaGroup.alpha = Mathf.Lerp(0, 1, timerRatio);
                }
                else
                {
                    m_exhaustionGaugesCanvaGroup.alpha = Mathf.Lerp(1, 0, timerRatio);
                }
            }
        }

        // Sets the position of the exhaustion gauges next to each other
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
        #endregion

    }
}
