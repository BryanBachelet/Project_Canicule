using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class MoneyButton : MonoBehaviour
    {
        private WorkDataManager m_workDataManager;
        private Button m_button;

        private void Awake()
        {
            m_button = GetComponent<Button>();
            m_workDataManager = GameManager.GetWorkDataManager();
        }

        private void Start()
        {
            m_button.onClick.AddListener(FireGainMoney);
        }

        private void OnDestroy()
        {
            m_button.onClick.RemoveListener(FireGainMoney);
        }

        private void FireGainMoney()
        {
            m_workDataManager.GainMoney();
        }
    }
}
