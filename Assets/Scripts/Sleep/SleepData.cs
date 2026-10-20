using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "SleepData", menuName = "Scriptable Objects/SleepData")]
    public class SleepData : ScriptableObject
    {
        public AnimationCurve sleepChanceOverRoomTemperature;
        public float sleepCooldown = 2f;

    }
}
