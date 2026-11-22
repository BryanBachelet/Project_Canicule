using UnityEngine;

namespace Canicule
{
    public class UIManager : MonoBehaviour
    {
        public LoseScreenUI m_loseScreenUI;
        public WinScreenUI m_winScreenUI;

        public void Start()
        {
            m_winScreenUI.CreateUI();
            m_loseScreenUI.CreateUI();
        }
    }
}
