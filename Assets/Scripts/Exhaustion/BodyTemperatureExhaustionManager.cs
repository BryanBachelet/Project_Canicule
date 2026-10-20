using UnityEngine;

namespace Canicule
{
    public class BodyTemperatureExhaustionManager : ExhaustionElement
    {
        public float ModifyExhaustionOverTime(float exhaustion, float target, float upSpeed, float downSpeed)
        {
            if(exhaustion <= target)
            {
                exhaustion += upSpeed * Time.deltaTime;
            }
            else
            {
                exhaustion -= downSpeed * Time.deltaTime;
            }

            return exhaustion;
        }
    }
}
