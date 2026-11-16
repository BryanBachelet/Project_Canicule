using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image m_interactionBarImg;
        [SerializeField] private Image m_interactionBackgroundImg;
        
        #region Unity Functions
        public void Awake()
        {
            DeactivateInteractionBar();
        }
        #endregion

        public void ActiveInteractionBar()
        {
            m_interactionBarImg.enabled = true;
            m_interactionBackgroundImg.enabled = true;
        }

        public void DeactivateInteractionBar()
        {
            m_interactionBarImg.enabled = false;
            m_interactionBackgroundImg.enabled = false;
        }

        public void SetInteractionBar(float ratio)
        {
            m_interactionBarImg.fillAmount = ratio;
        }
    }
}
