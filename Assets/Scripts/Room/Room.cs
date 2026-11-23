using UnityEngine;
using Canicule.Temperature;
using System.Collections.Generic;
namespace Canicule.Rooms
{
    [System.Serializable]
    public class RoomConnection
    {
        public Room roomLink;
        public Teleporter teleporter;

        public RoomConnection(Room room, Teleporter teleporter)
        {
            this.roomLink = room;
            this.teleporter = teleporter;
        }

        public bool IsDoorOpen()
        {
            return teleporter.GetDoorGeneral().IsDoorOpen();
        }
    }


    public class Room : MonoBehaviour
    {
        [SerializeField] private RoomData m_roomData;
        [SerializeField] private bool m_isStartingRoom = false; 

        [Space]
        [SerializeField] private Vector3[] m_appearencePoints;

        [SerializeField] private HeatManager m_heatManager = new HeatManager();

        private List<RoomConnection> m_roomConnectionArray = new List<RoomConnection>();

        private Window[] m_windowArray;

        private EdgeCollider2D m_edgeCollider;

        #region Unity Functions

        public void Start()
        {
            m_heatManager.InitHeatManager(m_roomData.m_baseTemperature, m_roomData.roomHeatResistance);

            RoomManager roomManager = GameManager.GetRoomManager();
            roomManager.RegisterRoom(this);
            if (gameObject.activeSelf)
            {
                roomManager.SetCurrentRoomPlayer(this);
                roomManager.OnPlayerChangeRoom.AddListener(OnPlayerChangedRoom);
            }

            InitializeWindows();
            InitializeEdgeCollider();
        }
        public void Update()
        {
            if (GameManager.GetCurrentGamePhase() == GamePhase.NIGHT)
            {
                m_heatManager.UpdateHeatManager();

               
            }

        }
        #endregion

        #region Inits Functions

        public void InitializeWindows()
        {
            m_windowArray = GetComponentsInChildren<Window>();
            for (int i = 0; i < m_windowArray.Length; i++)
            {
                m_windowArray[i].OnWindowOpen += OnDoorWindowOpened;
            }
        }

        public void InitializeEdgeCollider()
        {
            m_edgeCollider = GetComponentInChildren<EdgeCollider2D>();
            if (!m_edgeCollider)
            {
                Debug.LogWarning("[Collider] Need to add an edgeCollider");
            }
            else
            {
                if (!m_isStartingRoom)
                    m_edgeCollider.enabled = false;
            }
        }

        #endregion

        public void OnDoorWindowOpened()
        {
            if (IsADoorOpen() && IsAWindowOpen())
            {
                GameManager.GetDraftManager().TryCreateDraft(this);
            }
        }

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
            if (interactor == Interactor.PLAYER && roomManager.GetCurrentRoomMonster() != this ||
                interactor == Interactor.MONSTER && roomManager.GetCurrentRoomPlayer() != this)
            {
                this.gameObject.SetActive(true);
            }

        }

        public void PlayerLeaveRoom()
        {
            if (!m_edgeCollider) return;

            m_edgeCollider.enabled = false;
        }

        public void PlayerEnterRoom()
        {
            if (!m_edgeCollider) return;

            m_edgeCollider.enabled = true;
        }

        public void LinkRoomConnection(RoomConnection roomLink)
        {
            if (m_roomConnectionArray.Contains(roomLink)) return;

            m_roomConnectionArray.Add(roomLink);
            roomLink.teleporter.GetDoorGeneral().OnDoorOpened.AddListener(OnDoorWindowOpened);
        }

        public void UnlinkRoomConnection(RoomConnection roomLink)
        {
            if (!m_roomConnectionArray.Contains(roomLink)) return;

            m_roomConnectionArray.Remove(roomLink);
            roomLink.teleporter.GetDoorGeneral().OnDoorOpened.RemoveListener(OnDoorWindowOpened);
        }

        public HeatManager GetHeatManager() { return m_heatManager; }

        public float GetRoomDegree() { return m_heatManager.GetDegree(); }

        public Vector3[] GetAppearencePoint() { return m_appearencePoints; }

        public RoomConnection[] GetRoomsConnection() { return m_roomConnectionArray.ToArray(); }

        public bool IsAWindowOpen()
        {
            for (int i = 0; i < m_windowArray.Length; i++)
            {
                if (m_windowArray[i].IsOpen())
                    return true;
            }

            return false;
        }
  
        public Window GetOpenWindow()
        {
            for (int i = 0; i < m_windowArray.Length; i++)
            {
                if (m_windowArray[i].IsOpen())
                    return m_windowArray[i];
            }

            return null;
        }

        public bool IsADoorOpen(Room excludeRoom = null)
        {
            for (int i = 0; i < m_roomConnectionArray.Count; i++)
            {
                if (excludeRoom != m_roomConnectionArray[i].roomLink && m_roomConnectionArray[i].IsDoorOpen())
                {
                    return true;
                }
            }
            return false;
        }

        public RoomConnection GetMatchingRoomConnection(Room roomLink)
        {
            for (int i = 0; i < m_roomConnectionArray.Count; i++)
            {
                if (m_roomConnectionArray[i].roomLink == roomLink)
                {
                    return m_roomConnectionArray[i];
                }
            }

            return null;
        }


        public bool HasDraft()
        {
            for (int i = 0; i < m_roomConnectionArray.Count; i++)
            {
                Teleporter teleporterCmpt = m_roomConnectionArray[i].teleporter;
                if (teleporterCmpt.GetDoorGeneral().HasDraft())
                {
                    return true;
                }
            }

            return false;
        }

        private void OnGamePhaseChanged(GamePhase phase)
        {
            if (phase == GamePhase.MORNING)
            {
                m_heatManager.SetHeat(m_windowArray[0].GetHeatSource().GetTemperature());
            }
        }

        private void OnPlayerChangedRoom(Room room)
        {
            if (room == this)
            {
                PlayerEnterRoom();
            }
            else
            {
                PlayerLeaveRoom();
            }
        }
    }

}
