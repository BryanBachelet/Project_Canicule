using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class CharacterInput : MonoBehaviour
    {

        public InputActionAsset inputActions;
        public Vector2 moveDirection;

        [SerializeField] private CameraInput m_CameraInput;
        private CharacterInteract m_characterInteract;

        private InputActionMap m_playerMap;
        private InputAction m_moveAction;
        private InputAction m_interactAction;


        private void Awake()
        {
            m_characterInteract = GetComponent<CharacterInteract>();

            m_playerMap = inputActions.FindActionMap("Player", true);
            m_moveAction = m_playerMap.FindAction("Move", true);
            m_interactAction = m_playerMap.FindAction("Interact", true);
        }

        private void OnEnable()
        {
            m_playerMap.Enable();
            m_interactAction.performed += OnInteract;
        }
        private void OnDisable()
        {
            m_playerMap.Disable();
            m_interactAction.performed -= OnInteract;
        }

        private void Update()
        {
            OnMove();
        }

        // Update moveDirection to the current value of the Vector2 move
        public void OnMove()
        {
            moveDirection = m_moveAction.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext ctx)
        {
            m_characterInteract.Interact(m_CameraInput.mousePosition);
        }

    }
}
