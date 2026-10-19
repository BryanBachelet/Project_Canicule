using UnityEngine;

namespace Canicule
{
    public class CameraTargetMovement : MonoBehaviour
    {

        [SerializeField] private CameraInput m_cameraInput;
        [SerializeField] private GameObject m_player;
        [SerializeField] private float m_targetMaxDistance = 3f;
        [SerializeField] private float m_targetEquilibrium = 2f;
        private Vector2 mousePosition;

        private void Update()
        {
            mousePosition = m_cameraInput.mousePosition;
            Look(ComputesTargetPosition(m_player.transform.position, mousePosition));

            GetTargetDistanceRatio(m_player.transform.position, 
                ComputesTargetPosition(m_player.transform.position, mousePosition));
        }


        public Vector2 ComputesTargetPosition(Vector2 playerPos, Vector2 mousePos)
        {
            Vector2 m_playerMouseVector = mousePos - playerPos;
            Vector2 m_targetPos = playerPos +
                Vector2.ClampMagnitude(m_playerMouseVector / m_targetEquilibrium, m_targetMaxDistance);
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
    }
}
