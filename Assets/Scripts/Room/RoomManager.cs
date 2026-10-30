using UnityEngine;
using System.Collections.Generic;
using BorsalinoTools;
namespace Canicule.Rooms
{
    public class RoomManager : MonoBehaviour
    {
        private List<Room> m_roomList;
        private Room m_currentRoom;

        [Header("Debug Parameters")]
        [SerializeField] private bool m_activeRoomDebug = false;
        [SerializeField] private bool m_showDebugCurrentRoom = false;


        #region Unity Function
        public void Update()
        {
            if(m_currentRoom && m_showDebugCurrentRoom)
            {
                float degree = m_currentRoom.GetHeatManager().GetDegree();
                ScreenDebuggerTool.AddMessage($"Current Temperature : {degree}");
            }
        }
        #endregion
        public void SetCurrentRoom(Room newRoom)
        {
            m_currentRoom = newRoom;
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


        #region Debug Functions
        #endregion
    }
}
