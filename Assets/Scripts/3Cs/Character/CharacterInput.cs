using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class CharacterInput : MonoBehaviour
    {
        public CharacterData characterData;
        [HideInInspector()] public ExhaustionManager m_exhaustionManager;
        public InputActionAsset inputActions;
        public Vector2 moveDirection;

        [SerializeField] private CameraInput m_CameraInput;
        private CharacterInteract m_characterInteract;

        private InputAction m_moveAction;
        private InputAction m_interactAction;


        private void Awake()
        {
            m_characterInteract = GetComponent<CharacterInteract>();
            m_exhaustionManager = FindAnyObjectByType<ExhaustionManager>();

            m_interactAction = InputSystem.actions.FindAction("Interact", true);
            m_moveAction = InputSystem.actions.FindAction("Move", true);
        }

        private void OnEnable()
        {
            m_interactAction.performed += OnInteract;
        }
        private void OnDisable()
        {
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
