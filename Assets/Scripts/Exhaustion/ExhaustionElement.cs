using UnityEngine;

namespace Canicule
{
    public class ExhaustionElement : MonoBehaviour
    {
        // Increase exhaustion over time
        public virtual float IncreaseExhaustionOverTime(float exhaustion, float modifier)
        {
            return exhaustion += modifier * Time.deltaTime;
        }

        // Decrease exhaustion by an amount
        public virtual float ModifyExhaustionByAmount(float exhaustion, float modifier)
        {
            exhaustion += modifier;

            if (exhaustion < 0)
            {
                exhaustion = 0;
            }

            Debug.Log("[Exhaustion Element] Exhaustion modified by " + modifier);

            return exhaustion;
        }

    }
}
