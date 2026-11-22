using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class OutsideWorkUIPhase : MonoBehaviour
    {
        [SerializeField] private FadeUIComponent m_fadeComponent;
        [SerializeField] private float m_durationBetweeFadeAnimation;
        [SerializeField] private TMP_Text m_outsideWorkText;
        private bool m_outsideWorkAnimationLaunch;
        private float m_betweeFadeTimer = 0.0f;

        [HideInInspector] public UnityEvent OnLaunchOutsidePhase;
        [HideInInspector] public UnityEvent OnEndOutsidePhase;

        #region Unity Functions

        public void Start()
        {
            m_fadeComponent.OnFadeEnd.AddListener(OnFadeEnd);
            m_fadeComponent.HideImages();
        }

        public void Update()
        {
            if (!m_outsideWorkAnimationLaunch) return;

            if (m_betweeFadeTimer < m_durationBetweeFadeAnimation)
            {
                m_betweeFadeTimer += Time.deltaTime;
            }
            else
            {
                m_fadeComponent.LaunchFadeOut();
                m_outsideWorkAnimationLaunch = false;
                m_outsideWorkText.gameObject.SetActive(false);
            }
        }
        #endregion

        public void LaunchOutsidePhase()
        {
            OnLaunchOutsidePhase?.Invoke();
            m_fadeComponent.LaunchFadeIn();
            m_outsideWorkText.gameObject.SetActive(false);
        }

        private void OnFadeEnd(bool isFadeOut)
        {
            if (isFadeOut)
            {
                EndOutsidePhase();
            }
            else
            {
                m_outsideWorkAnimationLaunch = true;
                m_betweeFadeTimer = 0.0f;
                m_outsideWorkText.gameObject.SetActive(true);
            }
        }

        public void EndOutsidePhase()
        {
            OnEndOutsidePhase?.Invoke();
            m_outsideWorkText.gameObject.SetActive(true);

        }
    }
}
