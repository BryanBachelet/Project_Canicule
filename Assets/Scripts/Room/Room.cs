using UnityEngine;
using Canicule.Temperature;
using System.Collections.Generic;
namespace Canicule.Rooms
{
    [System.Serializable]
    public struct RoomConnection
    {
        public Room roomLink;
        public Teleporter teleporter;

        public RoomConnection(Room room, Teleporter teleporter)
        {
            this.roomLink = room;
            this.teleporter = teleporter;
        }
    }


    public class Room : MonoBehaviour
    {
        [SerializeField] private RoomData m_roomData;

        [SerializeField] private HeatManager m_heatManager = new HeatManager();

        private List<RoomConnection> m_roomLink;

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

        public void ActivateRoom()
        {
            this.gameObject.SetActive(true);
        }

        public void DeactivateRoom()
        {
            this.gameObject.SetActive(false);
        }

        public void LinkRoomConnection(RoomConnection roomLink)
        {
            if (m_roomLink.Contains(roomLink)) return;

            m_roomLink.Add(roomLink);
        }

        public void UnlinkRoomConnection(RoomConnection roomLink)
        {
            if (!m_roomLink.Contains(roomLink)) return;

            m_roomLink.Remove(roomLink);
        }

        public HeatManager GetHeatManager() { return m_heatManager; }

        public float GetRoomDegree() { return m_heatManager.GetDegree(); }
    }


}
