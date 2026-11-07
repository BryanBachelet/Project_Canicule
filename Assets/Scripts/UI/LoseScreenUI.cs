using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Canicule
{
    public class LoseScreenUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_loseText;
        [SerializeField] private Button m_replayButton;
        [SerializeField] private int m_sceneReplayIndex = 0;

        public void CreateUI()
        {
            GameManager.SubscribeToOnLoseEvent(ActiveLoseScreen);
        }

        public void OnDestroy()
        {
            GameManager.UnsubscribeToOnLoseEvent(ActiveLoseScreen);
        }

        public void ActiveLoseScreen()
        {
            gameObject.SetActive(true);
        }

        public void HideLoseScreen()
        {
            gameObject.SetActive(false);
        }

        public void Replay()
        {
            SceneManager.LoadScene(m_sceneReplayIndex);
        }
    }
}
