using Canicule.Temperature;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Canicule
{
    public class Window : InteractibleObject
    {
        public event Action OnWindowOpen;
        public event Action OnWindowClose;

        private bool m_isWindowClose = true;
        private HeatSource m_heatSource;
        private NightDayManager m_nightDayManager;

        [SerializeField] private float m_heatRatioWindowClose = 0.5f;
        [SerializeField] private float m_heatRatioWindowOpen = 1.0f;

        [SerializeField] private SpriteRenderer m_windowRenderer;
        [SerializeField] private Sprite m_windowOpen;
        [SerializeField] private Sprite m_windowClose;
        [SerializeField] private Sprite m_windowOpenSurvivalBlanket;
        [SerializeField] private Sprite m_windowCloseSurvivalBlanket;

        private bool b_areSurvivalBlanketsInstalled = false;
        private float m_degreeReductionFromSurvivalBlankets = 0f;

        #region Unity Methods
        public override void Start()
        {
            base.Start();
            m_heatSource = GetComponent<HeatSource>();
            m_nightDayManager = GameManager.GetNightDayManager();
            GameManager.GetEnvirommentHeatSource()?.OnTemperatureUpdate?.AddListener(OnWindowUpdateTemperature);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.GetEnvirommentHeatSource()?.OnTemperatureUpdate.RemoveListener(OnWindowUpdateTemperature);
        }

        private void OnEnable()
        {
            OnWindowOpen += ChangeWindowSpriteToOpen;
            OnWindowClose += ChangeWindowSpriteToClose;
        }

        private void OnDisable()
        {
            OnWindowOpen -= ChangeWindowSpriteToOpen;
            OnWindowClose -= ChangeWindowSpriteToClose;
        }

        public void OnTriggerEnter(Collider other)
        {
            Monster monsterComp = other.GetComponent<Monster>();
            if (monsterComp && !m_isWindowClose)
            {
                monsterComp.ChangeState(MonsterState.DISAPPEAR);
            }
        }
        #endregion

        #region Interaction Logic
        public override void LaunchInteraction(Transform actor, Interactor interactActorType)
        {
            base.LaunchInteraction(actor,interactActorType);
            
            m_isWindowClose = !m_isWindowClose; 
            WindowInteraction();
        }

        private void WindowInteraction()
        {
            if(m_isWindowClose)
            {
                OnWindowClose?.Invoke();
                m_heatSource.SetTemperatureScoreRatio(m_heatRatioWindowClose);
            }
            else
            {
                OnWindowOpen?.Invoke();
                m_heatSource.SetTemperatureScoreRatio(m_heatRatioWindowOpen);
            }
        }
        #endregion

        #region Visual Logic
        public void ChangeWindowSpriteToOpen()
        {
            if (!b_areSurvivalBlanketsInstalled)
            {
                m_windowRenderer.sprite = m_windowOpen;
            }
            else
            {
                m_windowRenderer.sprite = m_windowOpenSurvivalBlanket;
            }
        }
        
        public void ChangeWindowSpriteToClose()
        {
            if (!b_areSurvivalBlanketsInstalled)
            {
                m_windowRenderer.sprite = m_windowClose;
            }
            else
            {
                m_windowRenderer.sprite = m_windowCloseSurvivalBlanket;
            }
        }

        public void InstallSurvivalBlanket(float degreeReduction)
        {
            b_areSurvivalBlanketsInstalled = true;
            m_degreeReductionFromSurvivalBlankets = degreeReduction;

            if (m_isWindowClose)
            {
                ChangeWindowSpriteToClose();
            }
            else
            {
                ChangeWindowSpriteToOpen();
            }
        }
        #endregion

        private void OnWindowUpdateTemperature(float degree)
        {
            float effectiveDegree = degree;

            if (!m_nightDayManager.GetIsNightActive() && b_areSurvivalBlanketsInstalled)
            {
                effectiveDegree = degree - m_degreeReductionFromSurvivalBlankets;
            }

            m_heatSource.SetTemperature(effectiveDegree);
        }

        public HeatSource GetHeatSource() { return m_heatSource; }

        public bool IsOpen() { return !m_isWindowClose; }
    }
}
