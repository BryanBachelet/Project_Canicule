using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Scriptable Objects/CameraData")]
    public class CameraData : ScriptableObject
    {
        [Header("Player Camera")]
        public AnimationCurve cameraZoomOverExhaustion;
        public AnimationCurve characterTargetMaxDistanceOverExhaustion;
        public AnimationCurve cameraLerpSpeedOverExhaustion;
        public float targetEquilibrium = 2f;
        public float zoomLerpSpeed = 1f;
        public float targetLerpSpeed = 1f;
    }
}
