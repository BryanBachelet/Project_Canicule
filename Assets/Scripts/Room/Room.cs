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

        [Space]
        [SerializeField] private Vector3[] m_appearencePoints;

        [SerializeField] private HeatManager m_heatManager = new HeatManager();

        private List<RoomConnection> m_roomLink = new List<RoomConnection>();

        #region Unity Functions

        public void Start()
        { 
            m_heatManager.InitHeatManager(m_roomData.m_baseTemperature, m_roomData.roomHeatResistance);
            
            RoomManager roomManager = GameManager.GetRoomManager();
            roomManager.RegisterRoom(this);
            if (gameObject.activeSelf)
            {
                roomManager.SetCurrentRoomPlayer(this);
            }

        }
        public void Update()
        {
            m_heatManager.UpdateHeatManager();
        }
        #endregion

        public void ActivateRoom(Interactor interactor)
        {
            this.gameObject.SetActive(true);
            if (interactor == Interactor.PLAYER)
            {
                GameManager.GetRoomManager().SetCurrentRoomPlayer(this);
            }
            else
            {
                GameManager.GetRoomManager().SetCurrentRoomMonster(this);
            }

        }

        public void DeactivateRoom(Interactor interactor)
        {
            RoomManager roomManager = GameManager.GetRoomManager();
            if( interactor == Interactor.PLAYER && roomManager.GetCurrentRoomMonster() != this ||
                interactor == Interactor.MONSTER && roomManager.GetCurrentRoomPlayer() != this )
            {
                this.gameObject.SetActive(false);
            }
          
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

        public Vector3[] GetAppearencePoint() { return m_appearencePoints; }

        public RoomConnection[] GetRoomsConnection() { return m_roomLink.ToArray(); }
    }


}
