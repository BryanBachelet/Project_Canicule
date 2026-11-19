using TMPro;
using UnityEngine;

namespace Canicule
{
    public class HourUIInformation : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_hourText;
        private NightDayManager m_nightDayManager;

        private void OnEnable()
        {
            m_nightDayManager = GameManager.GetNightDayManager();
        }

        // Update is called once per frame
        void Update()
        {
            m_hourText.text = m_nightDayManager.GetDispayableHour();
        }
    }
}
