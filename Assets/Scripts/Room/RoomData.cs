using UnityEngine;

namespace Canicule.Rooms
{
    [CreateAssetMenu(fileName = "S_RoomData",menuName ="Room/RoomData",order =0)]
    public class RoomData : ScriptableObject
    {
        public float roomSize = 1;
        
        // Correspond to the resistance of room to increase the degree in it
        public float roomHeatResistance = 1;
    }
}
