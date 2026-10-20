using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "ExhaustionData", menuName = "Scriptable Objects/ExhaustionData")]
    public class ExhaustionData : ScriptableObject
    {
        [Header("Sleep")]
        public AnimationCurve sleepIncreaseRateOverNightRatio;
        public AnimationCurve sleepDecreaseAmountOverNightRatio;
        [Space(15)]

        [Header("Hunger")]
        public float hungerIncreaseRate;
        [Space(15)]

        [Header("Thirst")]
        public AnimationCurve thirstIncreaseRateOverTemperature;
        [Space(15)]

        [Header("Body Temperature")]
        public AnimationCurve bodyTemperatureOverTargetTemperature;
        public float draftBodyTemperatureReduction;
        public float upBodyTemperatureSpeed;
        public float downBodyTemperatureSpeed;
    }
}
