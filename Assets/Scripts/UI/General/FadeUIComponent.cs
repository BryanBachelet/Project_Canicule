using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Canicule
{
    public class FadeUIComponent : MonoBehaviour
    {
        [System.Serializable]
        public struct FadeData
        {
            public bool isFadeOut;
            [SerializeField] public float m_fadeInDuration;
            [SerializeField] public AnimationCurve m_fadeInCurve;
            [HideInInspector] public float m_fadeTimer;
        }

        [SerializeField] private bool m_includeChildren = false;

        [SerializeField] private FadeData m_fadeInData;
        [SerializeField] private FadeData m_fadeOutData;

        private Image m_imageMainComponent = null;
        private Image[] m_imagesChildComponents = new Image[0];

        private bool m_fadeOccuring = false;
        private FadeData m_currentFadaData;

        //bool - is if the fade in or out
        [HideInInspector] public UnityEvent<bool> OnFadeLaunch;
        [HideInInspector] public UnityEvent<bool> OnFadeEnd;

        void Awake()
        {
            m_imageMainComponent = GetComponent<Image>();
            if (m_includeChildren)
            {
                m_imagesChildComponents = GetComponentsInChildren<Image>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_fadeOccuring) return;

            m_currentFadaData.m_fadeTimer += Time.deltaTime;
            float currentRatio = m_currentFadaData.m_fadeTimer / m_currentFadaData.m_fadeInDuration;

            currentRatio = m_currentFadaData.m_fadeInCurve.Evaluate(currentRatio);

            if (m_currentFadaData.isFadeOut)
            {
                currentRatio = 1.0f - currentRatio;
            }

            SetImageAlphaValue(currentRatio);


            if (currentRatio >= 1.0f)
            {
                m_fadeOccuring = false;
                OnFadeEnd.Invoke(m_currentFadaData.isFadeOut);
            }
        }

        public void HideImages()
        {
            SetImageAlphaValue(0.0f);
        }

        public void ShowImage()
        {
            SetImageAlphaValue(1.0f);
        }
 
        private void SetImageAlphaValue(float ratio)
        {
            m_imageMainComponent.color = new Color(m_imageMainComponent.color.r, m_imageMainComponent.color.g, m_imageMainComponent.color.b, ratio);
            for (int i = 0; i < m_imagesChildComponents.Length; i++)
            {
                m_imagesChildComponents[i].color = new Color(m_imageMainComponent.color.r, m_imageMainComponent.color.g, m_imageMainComponent.color.b, ratio);
            }
        }

        public void LaunchFadeIn()
        {
            m_currentFadaData = m_fadeInData;
            _LaunchFade();
        }

        public void LaunchFadeOut()
        {
            m_currentFadaData = m_fadeOutData;
            _LaunchFade();
        }

        private void _LaunchFade()
        {
            OnFadeLaunch?.Invoke(m_currentFadaData.isFadeOut);
            m_fadeOccuring = true;
        }
    }
}
