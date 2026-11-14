using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
namespace Canicule.Item
{
    public class CharacterHoldItem : MonoBehaviour
    {
        private ItemData m_itemHoldData;

        [HideInInspector] public UnityEvent<ItemData> OnHoldItem;
        public void SetHoldItem(ItemData itemToHold)
        {
            m_itemHoldData = itemToHold;
            OnHoldItem?.Invoke(m_itemHoldData);
        }

        public void UseHoldItem()
        {
            if (!m_itemHoldData) return;

            GameObject instance = SpawnItem();
            if (instance == null)
                return;
            PutItem(instance);

            GameManager.GetInventoryDataManager().RemoveItem(m_itemHoldData);
            m_itemHoldData = null;
        }

        private GameObject SpawnItem()
        {
            Rooms.Room currentRoom = GameManager.GetRoomManager().GetCurrentRoomPlayer();
            InteractableEffect interactableEffect = m_itemHoldData.GetItemEffect<InteractableEffect>();
            if (interactableEffect == null) return null;

            GameObject instance = GameObject.Instantiate(interactableEffect.m_interactableObjectPrefab, transform.position, Quaternion.identity, currentRoom.transform);

            InteractibleObject interactableObject = instance.GetComponent<InteractibleObject>();
            if (interactableObject == null)
            {
                Destroy(instance);
                return null;
            }

            interactableObject.CreateObject(m_itemHoldData);
            return instance;

        }

        private void PutItem(GameObject instance)
        {
            NavMeshHit hit = new NavMeshHit();
            bool samplePositionResult = NavMesh.SamplePosition(instance.transform.position, out hit, 10.0f, NavMesh.AllAreas);
            if (samplePositionResult)
            {
                instance.transform.position = hit.position;
            }
        }
    }
}
