using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "WorkData", menuName = "Scriptable Objects/WorkData")]
    public class WorkData : ScriptableObject
    {
        public Vector2 moneyGainFromClick;
    }
}
