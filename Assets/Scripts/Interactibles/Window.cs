using Canicule.Temperature;
using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class Window : InteractibleObject
    {
        public UnityEvent OnWindowOpen;
        public UnityEvent OnWindowClose;

        private bool m_isWindowClose = true;
        private HeatSource m_heatSource;

        [SerializeField] private float m_heatRatioWindowClose = 0.5f;
        [SerializeField] private float m_heatRatioWindowOpen = 1.0f;

        public override void Start()
        {
            base.Start();
            m_heatSource = GetComponent<HeatSource>();
            GameManager.GetEnvirommentHeatSource().OnTemperatureUpdate?.AddListener(OnWindowUpdateTemperature);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.GetEnvirommentHeatSource().OnTemperatureUpdate?.RemoveListener(OnWindowUpdateTemperature);
        }

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

        private void OnWindowUpdateTemperature(float degree)
        {
            m_heatSource.SetTemperature(degree);
        }

        public void OnTriggerEnter(Collider other)
        {
            Monster monsterComp = other.GetComponent<Monster>();
            if (monsterComp && !m_isWindowClose)
            {
                monsterComp.ChangeState(MonsterState.DISAPPEAR);
            }
        }

        public HeatSource GetHeatSource() { return m_heatSource; }

        public bool IsOpen() { return !m_isWindowClose; }
    }
}
