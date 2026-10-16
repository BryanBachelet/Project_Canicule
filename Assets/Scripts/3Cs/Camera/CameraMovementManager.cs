using UnityEngine;

namespace Canicule
{
    public class CameraMovementManager : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;
        [SerializeField] private GameObject m_target;
        [SerializeField] private float m_cameraLerpSpeed = 8f;
        private CameraStateManager m_cameraStateManager;


        private void Start()
        {
            m_cameraStateManager = GetComponent<CameraStateManager>();
        }
        // Update is called once per frame
        void LateUpdate()
        {
            // The camera follows the target only if the camera state is FollowingPlayerTarget
            if(m_cameraStateManager.GetCurrentCameraState() == CameraState.FollowingPlayerTarget)
            {
                CameraFollowsTarget();
            }
        }

        // The camera's position is set to the target's position with a lerp
        private void CameraFollowsTarget()
        {
            Vector3 targetPos = new Vector3(m_target.transform.position.x,
                m_target.transform.position.y,
                m_camera.transform.position.z);

            m_camera.gameObject.transform.position = Vector3.Lerp(m_camera.gameObject.transform.position, 
                targetPos, m_cameraLerpSpeed * Time.deltaTime);
        }


    }
}
