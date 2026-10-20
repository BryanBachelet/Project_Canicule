using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public AnimationCurve characterMovementSpeedOverExhaustion;
        public float movementSpeedLerpSpeed = 1f;
        public float interactionRange = 1.5f;
    }
}
