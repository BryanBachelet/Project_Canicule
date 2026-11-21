using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Canicule
{
    public class SleepingUIPhase : MonoBehaviour
    {
        [SerializeField] private FadeUIComponent m_fadeComponent;
        [SerializeField] private float m_durationBetweeFadeAnimation;
        [SerializeField] private TMP_Text m_sleepingText;
        private bool m_sleepingAnimationLaunch;
        private float m_betweeFadeTimer = 0.0f;

        [HideInInspector] public UnityEvent OnLaunchSleepingPhase;
        [HideInInspector] public UnityEvent OnEndSleepingPhase;

        #region Unity Functions

        public void Start()
        {
            m_fadeComponent.OnFadeEnd.AddListener(OnFadeEnd);
            m_fadeComponent.HideImages();
        }

        public void Update()
        {
            if (!m_sleepingAnimationLaunch) return;

            if (m_betweeFadeTimer < m_durationBetweeFadeAnimation)
            {
                m_betweeFadeTimer += Time.deltaTime;
            }
            else
            {
                m_fadeComponent.LaunchFadeOut();
                m_sleepingAnimationLaunch = false;
                m_sleepingText.gameObject.SetActive(false);
            }
        }
        #endregion

        public void LaunchSleepingPhase()
        {
            OnLaunchSleepingPhase?.Invoke();
            m_fadeComponent.LaunchFadeIn();
            m_sleepingText.gameObject.SetActive(false);
        }

        private void OnFadeEnd(bool isFadeOut)
        {
            if (isFadeOut)
            {
                EndSleepingPhase();
            }
            else
            {
                m_sleepingAnimationLaunch = true;
                m_betweeFadeTimer = 0.0f;
                m_sleepingText.gameObject.SetActive(true);
            }
        }

        public void EndSleepingPhase()
        {
            OnEndSleepingPhase?.Invoke();
           
          
        }
    }
}
