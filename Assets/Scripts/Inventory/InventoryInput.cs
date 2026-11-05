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
        private const string m_navigateActionName = "NavigateInventory";

        #region Unity Methods
        private void Awake()
        {
            m_inputManager = FindAnyObjectByType<InputManager>();
            m_inventoryUIManager = GetComponent<InventoryUIManager>();
            m_inventoryDataManager = GetComponent<InventoryDataManager>();

            m_navigateAction = InputSystem.actions.FindAction(m_navigateActionName, true);
        }

        private void OnEnable()
        {
            m_inventoryUIManager.OnInventoryOpen += SwitchToInventoryMap;
            m_inventoryUIManager.OnInventoryClose += SwitchToPlayerMap;

            m_navigateAction.performed += ModifyIndexSelector;
        }

        private void OnDisable()
        {
            m_inventoryUIManager.OnInventoryOpen -= SwitchToInventoryMap;
            m_inventoryUIManager.OnInventoryClose -= SwitchToPlayerMap;

            m_navigateAction.performed -= ModifyIndexSelector;
        }
        #endregion

        #region Input Methods
        private void ModifyIndexSelector(InputAction.CallbackContext ctx)
        {
            if(ctx.ReadValue<Vector2>().x > 0)
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

        private void SwitchToInventoryMap()
        {
            m_inputManager.DisablePlayerMap();
            m_inputManager.EnableInventoryMap();
        }

        private void SwitchToPlayerMap()
        {
            m_inputManager.EnablePlayerMap();
            m_inputManager.DisableInventoryMap();
        }
        #endregion
    }
}
