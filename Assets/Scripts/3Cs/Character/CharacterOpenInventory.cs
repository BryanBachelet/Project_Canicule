using UnityEngine;

namespace Canicule
{
    public enum InventoryRequestResult
    {
        CANCEL = 0,
        SELECTED = 1,
    }

    public class CharacterOpenInventory : MonoBehaviour
    {
        private InventoryDataManager m_inventoryDataManager;
        private StationInterface m_requestStationObject = null;

        private void Start()
        {
            m_inventoryDataManager = GameManager.GetInventoryDataManager();
        }
        public void OpenInventory()
        {
            m_inventoryDataManager.OpenInventory();
        }

        public void RequestInventorySelection(StationInterface interactObject)
        {
            m_requestStationObject = interactObject;
            m_inventoryDataManager.RequestSelectionItem(OnRequestAnswer, interactObject);
        }

        public void OnRequestAnswer(InventoryRequestResult requestResult,ItemData itemData)
        {
            if(requestResult == InventoryRequestResult.CANCEL)
            {
                CancelRequest();
            }

            ValidRequest(itemData);
        }

        private void CancelRequest()
        {
            InteractibleObject interactObject = (InteractibleObject)m_requestStationObject;
            interactObject.CancelInteration();
            m_requestStationObject = null;
        }

        private void ValidRequest(ItemData itemData)
        {
            m_requestStationObject.ValidateInteraction(itemData);
            m_requestStationObject = null;
        }
    }
}
