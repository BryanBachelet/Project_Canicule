using UnityEngine;

namespace Canicule
{
    public class CameraTargetMovement : MonoBehaviour
    {
        [SerializeField] private CameraInput m_cameraInput;
        [SerializeField] private CameraData m_cameraData;
        [SerializeField] private GameObject m_player;
        private ExhaustionManager m_exhaustionManager;
        private float m_targetMaxDistance;
        private Vector2 mousePosition;
        


        private void Awake()
        {
            m_exhaustionManager = Object.FindAnyObjectByType<ExhaustionManager>();
        }

        private void Update()
        {
            mousePosition = m_cameraInput.mousePosition;
            Look(ComputesTargetPosition(m_player.transform.position, mousePosition));

            GetTargetDistanceRatio(m_player.transform.position, 
                ComputesTargetPosition(m_player.transform.position, mousePosition));

            ComputeTargetMaxDistance();
        }


        public Vector2 ComputesTargetPosition(Vector2 playerPos, Vector2 mousePos)
        {
            Vector2 m_playerMouseVector = mousePos - playerPos;
            Vector2 m_targetPos = playerPos +
                Vector2.ClampMagnitude(m_playerMouseVector / m_cameraData.targetEquilibrium, m_targetMaxDistance);
            return m_targetPos;
        }
        // Update the position of the camera target
        public void Look(Vector2 targetPos)
        {
            gameObject.transform.position = targetPos;
        }

        // Outputs the ratio between target max distance and player mouse vector cut
        public void GetTargetDistanceRatio(Vector2 playerPos, Vector2 targetPos)
        {
            Vector2 m_playerTargetVector = targetPos - playerPos;
            //Debug.Log(m_playerTargetVector.magnitude / m_targetMaxDistance);
        }

        // Compute the value of target max distance against exhaustion ratio
        private void ComputeTargetMaxDistance()
        {
            float start = m_targetMaxDistance;
            float target = m_cameraData.characterTargetMaxDistanceOverExhaustion.Evaluate(m_exhaustionManager.exhaustionRatio);
            m_targetMaxDistance = Mathf.Lerp(start, target, m_cameraData.targetLerpSpeed);
        }
    }
}
