using UnityEngine;

namespace Canicule
{
    public class UIManager : MonoBehaviour
    {
        public LoseScreenUI m_loseScreenUI;

        public void Start()
        {
            m_loseScreenUI.CreateUI();
        }
    }
}
