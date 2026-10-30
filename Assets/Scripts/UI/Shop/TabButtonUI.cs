using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class TabButtonUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_lowerBar;
        [SerializeField] private TMP_Text m_text;
        [SerializeField] private Color m_inactiveColor;
        [SerializeField] private Color m_activeColor;
        [SerializeField] private Button m_button;

        #region Unity Methods
        private void Awake()
        {
            //m_button = GetComponent<Button>();
        }
        #endregion

        #region Tab Button Appearance Managament
        public void ActivateButtonTab()
        {
            // Change the normal color of the button
            ColorBlock cb = m_button.colors;
            cb.normalColor = m_activeColor;
            m_button.colors = cb;

            m_lowerBar.SetActive(true);
            m_text.color = m_activeColor;
        }

        public void DeactivateButtonTab()
        {
            // Change the normal color of the button
            ColorBlock cb = m_button.colors;
            cb.normalColor = m_inactiveColor;
            m_button.colors = cb;

            m_lowerBar.SetActive(false);
            m_text.color = m_inactiveColor;
        }
        #endregion
    }
}
