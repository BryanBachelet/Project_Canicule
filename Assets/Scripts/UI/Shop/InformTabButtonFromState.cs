using UnityEngine;

namespace Canicule
{
    public class InformTabButtonFromState : MonoBehaviour
    {
        [SerializeField] private TabButtonUI m_tabButtonUI;

        private void OnEnable()
        {
            m_tabButtonUI.ActivateButtonTab();
        }

        private void OnDisable()
        {
            m_tabButtonUI.DeactivateButtonTab();
        }
    }
}
