using UnityEngine;

namespace Canicule
{
    public class CharacterOpenInventory : MonoBehaviour
    {
        private InventoryUIManager m_inventoryUIManager;

        private void Awake()
        {
            m_inventoryUIManager = FindAnyObjectByType<InventoryUIManager>();
        }
        public void OpenInventory()
        {
            m_inventoryUIManager.SwitchInventoryScreen();
        }
    }
}
