using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class CameraInput : MonoBehaviour
    {
        public InputActionAsset inputActions;
        public Vector2 mousePosition;

        private InputActionMap m_playerMap;
        private InputAction m_lookAction;

        private void Awake()
        {
            m_playerMap = inputActions.FindActionMap("Player");
            m_lookAction = m_playerMap.FindAction("Look");
        }

        private void OnEnable() => m_playerMap.Enable();
        private void OnDisable() => m_playerMap.Disable();

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
