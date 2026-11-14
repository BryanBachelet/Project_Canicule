using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class InventoryInput : MonoBehaviour
    {
        private InputManager m_inputManager;
        private InventoryUIManager m_inventoryUIManager;
        private InventoryDataManager m_inventoryDataManager;

        private InputAction m_navigateAction;
        private InputAction m_submitAction;
        private const string m_navigateActionName = "NavigateInventory";
        private const string m_submitActionName = "SubmitInventory";


        private InventoryDisplayMode m_currentInventoryDisplayMode = InventoryDisplayMode.FREE;
        #region Unity Methods
        private void Awake()
        {
            m_inputManager = GameManager.GetInputManager();
            m_inventoryUIManager = GetComponent<InventoryUIManager>();
            m_inventoryDataManager = GameManager.GetInventoryDataManager();

            m_navigateAction = InputSystem.actions.FindAction(m_navigateActionName, true);
            m_submitAction = InputSystem.actions.FindAction(m_submitActionName, true);
        }

        private void OnEnable()
        {
            m_inventoryUIManager.OnInventoryOpen += SwitchToInventoryMap;
            m_inventoryUIManager.OnInventoryClose += SwitchToPlayerMap;

            m_navigateAction.performed += ModifyIndexSelector;
            m_submitAction.performed += InteractWithItem;
        }

        private void OnDisable()
        {
            m_inventoryUIManager.OnInventoryOpen -= SwitchToInventoryMap;
            m_inventoryUIManager.OnInventoryClose -= SwitchToPlayerMap;

            m_navigateAction.performed -= ModifyIndexSelector;
            m_submitAction.performed -= InteractWithItem;
        }
        #endregion

        #region Input Methods
        private void ModifyIndexSelector(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValue<Vector2>().x > 0)
            {
                // Increase index selector
                m_inventoryDataManager.IncreaseIndexSelector();
            }

            else if (ctx.ReadValue<Vector2>().x < 0)
            {
                // Decrease index selector
                m_inventoryDataManager.DecreaseIndexSelector();
            }
        }

        private void InteractWithItem(InputAction.CallbackContext ctx)
        {
            if (m_currentInventoryDisplayMode == InventoryDisplayMode.FREE)
            {
                m_inventoryDataManager.UseItem();
            }
            else
            {
                string resutAnswer = "";
                if(!m_inventoryDataManager.AnswerSelectionRequest(InventoryRequestResult.SELECTED, out resutAnswer))
                {
                    // Do something of feedback error
                }
            }
        }

        private void SwitchToInventoryMap(InventoryDisplayMode mode)
        {
            m_inputManager.DisablePlayerMap();
            m_inputManager.EnableInventoryMap();
            m_currentInventoryDisplayMode = mode;
        }

        private void SwitchToPlayerMap()
        {
            m_inputManager.EnablePlayerMap();
            m_inputManager.DisableInventoryMap();
        }
        #endregion
    }
}
