using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Canicule
{
    public class WinScreenUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_loseText;
        [SerializeField] private Button m_replayButton;
        [SerializeField] private int m_sceneReplayIndex = 0;

        public void CreateUI()
        {
            GameManager.SubscribeToOnWinEvent(ActiveWinScreen);
        }

        public void OnDestroy()
        {
            GameManager.SubscribeToOnWinEvent(ActiveWinScreen);
        }

        public void ActiveWinScreen()
        {
            gameObject.SetActive(true);
        }

        public void HideWinScreen()
        {
            gameObject.SetActive(false);
        }

        public void Replay()
        {
            SceneManager.LoadScene(m_sceneReplayIndex);
        }
    }
}
