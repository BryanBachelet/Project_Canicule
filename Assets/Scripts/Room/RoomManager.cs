using UnityEngine;
using System.Collections.Generic;
using BorsalinoTools;
using Canicule.Temperature;

namespace Canicule.Rooms
{
    public class RoomManager : MonoBehaviour
    {
        struct RoomNavData
        {
            public Room room;
            public float q;
            public float h;
            public float f;
            public RoomNavData(Room room)
            {
                q = 0;
                h = 0;
                f = 0;
                this.room = room;
            }
        }

        private List<Room> m_roomList = new List<Room>();
        private Room m_currentRoomPlayer;
        private Room m_currentRoomMonster;

        [SerializeField] private EnvirommentHeatSource m_envirommentHeatSource;

        [Header("Debug Parameters")]
        [SerializeField] private bool m_activeRoomDebug = false;
        [SerializeField] private bool m_showDebugCurrentRoom = false;


        #region Unity Function
        public void Update()
        {
            if(m_currentRoomPlayer && m_showDebugCurrentRoom)
            {
                float degree = m_currentRoomPlayer.GetHeatManager().GetDegree();
                ScreenDebuggerTool.AddMessage($"Current Temperature : {degree}");
            }
        }
        #endregion
        public void SetCurrentRoomPlayer(Room newRoom)
        {
            m_currentRoomPlayer = newRoom;
        }

        public void SetCurrentRoomMonster(Room newRoom)
        {
            m_currentRoomMonster = newRoom;
        }
        
        public void RegisterRoom(Room roomToRegister)
        {
            if (m_roomList.Contains(roomToRegister))
                return;

            m_roomList.Add(roomToRegister);

            if (m_activeRoomDebug)
            {
                Debug.Log($"[Room] Add room {roomToRegister.gameObject.name}");
            }
        }

        public void UnregisterRoom(Room roomToRemove)
        {
            if (!m_roomList.Contains(roomToRemove))
                return;

            m_roomList.Remove(roomToRemove);

            if (m_activeRoomDebug)
            {
                Debug.Log($"[Room] Remove room {roomToRemove.gameObject.name}");
            }
        }

        public Room[] GetRoomByTemperature(float temperature)
        {
            Room[] m_roomArray = m_roomList.ToArray();
            List<Room> m_roomValid = new List<Room>();

            for (int i = 0; i < m_roomArray.Length; i++)
            {
                float degree = m_roomArray[i].GetHeatManager().GetDegree();
                if (degree > temperature)
                {
                    m_roomValid.Add(m_roomArray[i]);
                }
            }

            return m_roomValid.ToArray();
        }
        
        public Room GetCurrentRoomPlayer() { return m_currentRoomPlayer; }
        public Room GetCurrentRoomMonster() { return m_currentRoomMonster; }

        #region Navigation Functions 
        
        public List<RoomConnection> ComputeRoomNavigation(Room dest, Room start)
        {
            if(dest == null || start ==null)
            {
                throw new System.NullReferenceException("[Navigation] One of the room is null");
            }

            if(dest == start)
            {
                Debug.LogWarning("[Navigation] Destination and Start are the same room");
                return null;
            }

            List<RoomConnection> roomsPath = new List<RoomConnection>();

            Stack<RoomNavData> openRoom =new Stack<RoomNavData>();

            RoomNavData startRoomNavData = new RoomNavData(start);

            openRoom.Push(startRoomNavData);
            while(openRoom.Count != 0)
            {
                RoomNavData currentRoom = openRoom.Pop(); 
                RoomConnection[] currentRoomsConnection = currentRoom.room.GetRoomsConnection();

                float lowerF = float.MaxValue;
                int indexNextDoor = -1;
                RoomNavData nextRoomNavData = new RoomNavData();

                for (int i = 0; i < currentRoomsConnection.Length; i++)
                {
                    if (currentRoomsConnection[i].roomLink == dest)
                    {
                        roomsPath.Add(currentRoomsConnection[i]);
                        return roomsPath;
                    }

                    float q = currentRoom.q + Vector2.Distance(currentRoom.room.transform.position, currentRoomsConnection[i].roomLink.transform.position);
                    float h = Vector2.Distance(currentRoom.room.transform.position, dest.transform.position);

                    float f = q + h;

                    if(!roomsPath.Contains(currentRoomsConnection[i]) && f < lowerF )
                    {
                        nextRoomNavData = new RoomNavData(currentRoomsConnection[i].roomLink);
                        nextRoomNavData.f = f;
                        nextRoomNavData.q = q;
                        nextRoomNavData.h = h;

                        lowerF = f;
                        indexNextDoor = i;
                    }

                }

                roomsPath.Add(currentRoomsConnection[indexNextDoor]);
                openRoom.Push(nextRoomNavData);
            }

            return roomsPath;
        }

        #endregion

        #region Debug Functions
        #endregion
    }
}
