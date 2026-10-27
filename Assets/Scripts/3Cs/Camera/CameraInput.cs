using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class CameraInput : MonoBehaviour
    {
        public InputActionAsset inputActions;
        public Vector2 mousePosition;

        private InputAction m_lookAction;

        private void Awake()
        {
            m_lookAction = InputSystem.actions.FindAction("Look");
        }

        private void Update()
        {
            OnLook();
        }

        // Update mousePosition to the value of the vector2 look
        private void OnLook()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(m_lookAction.ReadValue<Vector2>());
        }

    }
}
