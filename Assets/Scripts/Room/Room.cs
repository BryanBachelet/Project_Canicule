using UnityEngine;
using Canicule.Temperature;
namespace Canicule.Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private RoomData m_roomData;

        [SerializeField] private HeatManager m_heatManager = new HeatManager();

        #region Unity Functions

        public void Start()
        { 
            m_heatManager.InitHeatManager(m_roomData.m_baseTemperature, m_roomData.roomHeatResistance);
        }
        public void Update()
        {
            m_heatManager.UpdateHeatManager();
        }
        #endregion

        public HeatManager GetHeatManager() { return m_heatManager; }

        public float GetRoomDegree() { return m_heatManager.GetDegree(); }
    }


}
