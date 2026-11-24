using Canicule;
using Canicule.Rooms;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

namespace Canicule
{

    public class DraftData
    {
        public Room m_roomStarter;
        public Room m_roonEnder;
        public RoomConnection[] m_roomConnection;
    }

    public class DraftManager : MonoBehaviour
    {
    

        private List<DraftData> m_draftData = new List<DraftData>();


        public void TryCreateDraft(Room starterRoom)
        {
            List<RoomConnection> roomConnectionList = new List<RoomConnection>();
            List<Room> excludeRoom = new List<Room>();
            Room finalRoom = _FindRoomWithAirFlow(starterRoom,starterRoom.GetRoomsConnection(), ref roomConnectionList,ref excludeRoom);

            if (!finalRoom) return;

            CreateDraft(starterRoom, finalRoom, roomConnectionList.ToArray());
        }


        private void CreateDraft(Room startRoom, Room endRoom, RoomConnection[] roomConnectionArray)
        {
            DraftData draftData = new DraftData();
            draftData.m_roomStarter = startRoom;
            draftData.m_roonEnder = endRoom;
            draftData.m_roomConnection = roomConnectionArray;

            m_draftData.Add(draftData);

            startRoom.SetRoomVentilate();
            endRoom.SetRoomVentilate();

            foreach (RoomConnection item in draftData.m_roomConnection)
            {
                item.teleporter.GetDoorGeneral().m_doorCmpt.CreateDraft(startRoom, endRoom);
            }
        }

        private Room _FindRoomWithAirFlow(Room starterRoom, RoomConnection[] roomConnections, ref List<RoomConnection> connectionList, ref List<Room> excludeRoom)
        {

            for (int i = 0; i < roomConnections.Length; i++)
            {
                if (!roomConnections[i].IsDoorOpen() || excludeRoom.Contains(roomConnections[i].roomLink)) continue;

                Room roomLink = roomConnections[i].roomLink;
                if (roomLink.IsAWindowOpen())   
                {
                    connectionList.Add(roomConnections[i]);
                    RoomConnection otherRoomConnection = roomConnections[i].roomLink.GetMatchingRoomConnection(roomConnections[i].teleporter.GetOwnerRoom());
                    connectionList.Add(otherRoomConnection);
                    return roomLink;
                }

                if (roomLink.IsADoorOpen(starterRoom))
                {
                    excludeRoom.Add(starterRoom);
                    Room roomFound = _FindRoomWithAirFlow(roomLink,roomLink.GetRoomsConnection(), ref connectionList, ref excludeRoom);
                    if (roomFound)
                    {
                        connectionList.Add(roomConnections[i]);
                        RoomConnection otherRoomConnection = roomConnections[i].roomLink.GetMatchingRoomConnection(roomConnections[i].teleporter.GetOwnerRoom());
                        connectionList.Add(otherRoomConnection);
                        return roomFound;
                    }
                }
            }

            return null;
        }

        public void OnDoorClosed(DoorGeneral doorGeneral)
        {
            for (int i = 0; i < m_draftData.Count ; i++)
            {
                DraftData data = m_draftData[i];
                for (int j = 0; j < data.m_roomConnection.Length; j++)
                {
                    if(doorGeneral == data.m_roomConnection[i].teleporter.GetDoorGeneral())
                    {
                        DeleteDraft(data);
                        break;
                    }
                }
            }
        }

        public void OnWindowClosed(Room roomTarget)
        {
            for (int i = 0; i < m_draftData.Count; i++)
            {
                DraftData data = m_draftData[i];
                for (int j = 0; j < data.m_roomConnection.Length; j++)
                {
                    if (roomTarget == data.m_roomStarter || data.m_roomStarter == roomTarget)
                    {
                        DeleteDraft(data);
                        break;
                    }
                }
            }
        }

        private void DeleteDraft(DraftData draftDataTarget)
        {
            draftDataTarget.m_roomStarter.ResetRoomVentilation();
            draftDataTarget.m_roonEnder.ResetRoomVentilation();

            foreach (RoomConnection item in draftDataTarget.m_roomConnection)
            {
                item.teleporter.GetDoorGeneral().DeleteDraft();
            }
        }
    }



  

}
