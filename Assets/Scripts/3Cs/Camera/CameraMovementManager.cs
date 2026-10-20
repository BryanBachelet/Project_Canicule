using UnityEngine;

namespace Canicule
{
    public class CameraMovementManager : MonoBehaviour
    {
        public CameraData cameraData;
        [HideInInspector()]
        public ExhaustionManager m_exhaustionManager;
        [SerializeField] private Camera m_camera;
        [SerializeField] private GameObject m_target;
        [SerializeField] private float m_cameraLerpSpeed = 8f;
        private CameraStateManager m_cameraStateManager;



        private void Awake()
        {
            m_exhaustionManager = Object.FindAnyObjectByType<ExhaustionManager>();
            m_cameraStateManager = GetComponent<CameraStateManager>();
        }


        private void Update()
        {
            UpdateCameraZoom();
            UpdateCameraLerpSpeed();
        }

        private void LateUpdate()
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

        // Updates camera zoom function of exhaustion ratio
        private void UpdateCameraZoom()
        {
            float start = m_camera.orthographicSize;
            float target = cameraData.cameraZoomOverExhaustion.Evaluate(m_exhaustionManager.exhaustionRatio);
            m_camera.orthographicSize = Mathf.Lerp(start, target, cameraData.zoomLerpSpeed * Time.deltaTime);
        }

        private void UpdateCameraLerpSpeed()
        {
            m_cameraLerpSpeed = cameraData.cameraLerpSpeedOverExhaustion.Evaluate(m_exhaustionManager.exhaustionRatio);
        }


    }
}
