using Canicule.Rooms;
using TMPro;
using UnityEngine;

namespace Canicule
{
    public class TemperatureUIInformation : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_temperatureText;
        [SerializeField] private string m_suffix = "°C";
        private RoomManager m_roomManager;

        private void OnEnable()
        {
            m_roomManager = GameManager.GetRoomManager();
        }

        // Update is called once per frame
        void Update()
        {
            m_temperatureText.text = m_roomManager.GetCurrentRoomPlayer().GetHeatManager().GetDegree().ToString() + m_suffix;
        }
    }
}
