using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class ExhaustionGauge : MonoBehaviour
    {

        [SerializeField] private GameObject m_exhaustionIcon;
        private RectTransform m_rectTransform;
        private float allowedWidth = 40f;

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }


        void Update()
        {
            ManageExhaustionIcon();
        }

        // Sets the width of the gauge depending on the exhaustion value
        public void UpdateExhaustionGaugeValue(float converter, float exhaustion)
        {
            m_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                converter * exhaustion);
        }

        // Activates or deactivates icons depending on the gauge width (avoid icons overlapping)
        private void ManageExhaustionIcon()
        {
            if(m_rectTransform.rect.width < allowedWidth)
            {
                m_exhaustionIcon.SetActive(false);
            }
            else
            {
                m_exhaustionIcon.SetActive(true);
            }
        }

    }
}
