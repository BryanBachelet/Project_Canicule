using Canicule.Item;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class CharacterInput : MonoBehaviour
    {
        public CharacterData characterData;
        public InputActionAsset inputActions;
        public Vector2 moveDirection;

        [SerializeField] private CameraInput m_CameraInput;
        private CharacterInteract m_characterInteract;
        private CharacterOpenInventory m_characterOpenInventory;
        private CharacterHoldItem m_characterHoldItem;

        private InputAction m_moveAction;
        private InputAction m_interactAction;
        private InputAction m_openInventoryAction;
        private InputAction m_closeInventoryAction;
        private InputAction m_useHoldItem;

        private const string m_interactActionName = "Interact";
        private const string m_moveActionName = "Move";
        private const string m_openInventoryActionName = "OpenInventory";
        private const string m_closeInventoryActionName = "CloseInventory";
        private const string m_useHoldItemName = "UseHoldItem";


        private void Awake()
        {
            m_characterInteract = GetComponent<CharacterInteract>();
            m_characterOpenInventory = GetComponent<CharacterOpenInventory>();
            m_characterHoldItem = GetComponent<CharacterHoldItem>();

            m_interactAction = InputSystem.actions.FindAction(m_interactActionName, true);

            m_moveAction = InputSystem.actions.FindAction(m_moveActionName, true);
            m_openInventoryAction = InputSystem.actions.FindAction(m_openInventoryActionName, true);
            m_closeInventoryAction = InputSystem.actions.FindAction(m_closeInventoryActionName, true);
            m_useHoldItem = InputSystem.actions.FindAction(m_useHoldItemName, true);
        }

        private void OnEnable()
        {
            m_interactAction.performed += OnInteract;
            m_openInventoryAction.performed += OnOpenInventory;
            m_closeInventoryAction.performed += OnOpenInventory;
            m_useHoldItem.performed += OnUseHoldItem;
        }
        private void OnDisable()
        {
            m_interactAction.performed -= OnInteract;
            m_openInventoryAction.performed -= OnOpenInventory;
            m_closeInventoryAction.performed -= OnOpenInventory;
            m_useHoldItem.performed -= OnUseHoldItem;
        }

        private void Update()
        {
            OnMove();
        }



        #region Inputs Functions
        // Update moveDirection to the current value of the Vector2 move
        public void OnMove()
        {
            moveDirection = m_moveAction.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext ctx)
        {
            m_characterInteract.Interact(m_CameraInput.mousePosition);
        }

        public void OnOpenInventory(InputAction.CallbackContext ctx)
        {
            m_characterOpenInventory.OpenInventory();
        }

        public void OnUseHoldItem(InputAction.CallbackContext ctx)
        {
            m_characterHoldItem.UseHoldItem();
        }
        #endregion
    }
}
