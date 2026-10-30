using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "ColorsData", menuName = "Scriptable Objects/ColorsData")]
    public class ColorsData : ScriptableObject
    {
        public Color sleepColor;
        public Color hungerColor;
        public Color thirstColor;
        public Color bodyTemperatureColor;
    }
}
