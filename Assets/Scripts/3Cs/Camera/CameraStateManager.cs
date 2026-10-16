using UnityEngine;

namespace Canicule
{
    public class CameraStateManager : MonoBehaviour
    {
        private CameraState m_currentState;

        private void Awake()
        {
            ChangeState(CameraState.FollowingPlayerTarget);
        }

        // Change the current camera state
        public void ChangeState(CameraState state)
        {
            m_currentState = state;
        }


        // Return the value of the current camera state
        public CameraState GetCurrentCameraState()
        {
            return m_currentState;
        }
    }
}
