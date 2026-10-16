using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class CharacterInput : MonoBehaviour
    {

        public InputActionAsset inputActions;
        public Vector2 moveDirection;

        private InputActionMap m_playerMap;
        private InputAction m_moveAction;


        private void Awake()
        {
            m_playerMap = inputActions.FindActionMap("Player", true);
            m_moveAction = m_playerMap.FindAction("Move", true);
        }

        private void OnEnable() => m_playerMap.Enable();
        private void OnDisable() => m_playerMap.Disable();

        private void Update()
        {
            OnMove();
        }

        // Update moveDirection to the current value of the Vector2 move
        public void OnMove()
        {
            moveDirection = m_moveAction.ReadValue<Vector2>();
        }

    }
}
