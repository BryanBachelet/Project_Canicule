using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using static UnityEditor.Progress;

namespace Canicule
{
    public class InventoryDataManager : MonoBehaviour
    {
        [SerializeField] private float m_playerBalance;
        [SerializeField] private InventoryUIManager m_inventoryUIManager;
        private int m_indexSelector = 0;

        private Dictionary<ItemData, int> m_playerItems = new Dictionary<ItemData, int>();
        private List<ItemData> m_displayedInventory = new List<ItemData>();

        public event Action<ItemData, int> OnAddingItem; // Used to update the owned amount in the shop
        public event Action OnDecreasingItemAmount;
        public event Action OnRemovingItem;
        public event Action OnAddingFinished;
        public event Action OnSelectorChanged;

        private UnityEvent<InventoryRequestResult, ItemData> m_OnSelectionRequestAnser = new UnityEvent<InventoryRequestResult, ItemData>();

        private StationInterface m_currentStationInterface;
        private const string m_resultIndexValidString = "Index selection isn't valid";
        private const string m_resultItemValidString = "Item isn't valid";

        [SerializeField] private OutsideWorkUIPhase m_OutsideUIPhase;

        private bool m_isInventoryOpen = false;
        private InventoryDisplayMode m_displayMode;

        public void OpenInventory()
        {

            if (m_isInventoryOpen)
            {
                _CloseInventory();
            }
            else
            {
                _OpenInventory();
            }

        }

        private void _OpenInventory()
        {
            // Manage player states
            Character character = GameManager.GetPlayer().GetComponent<Character>();
            CharacterState currentCharacterState = character.GetCurrentState();

            m_displayMode = InventoryDisplayMode.FREE;

            if (!m_isInventoryOpen && currentCharacterState != CharacterState.INVENTORY)
            {
                character.ChangeState(CharacterState.INVENTORY);
                m_isInventoryOpen = true;
                return;
            }

        }

        private void _CloseInventory()
        {
            if (m_isInventoryOpen)
            {
                Character character = GameManager.GetPlayer().GetComponent<Character>();
                CharacterState currentCharacterState = character.GetCurrentState();
                character.ChangeState(CharacterState.IDLE);

                m_inventoryUIManager.CloseInventoryScreen();
            }
            if (m_displayMode == InventoryDisplayMode.SELECTION)
            {
                CharacterInteract interactCmp = GameManager.GetPlayer().GetComponent<CharacterInteract>();
                interactCmp.CancelInteraction();
            }

        }

        public void RequestSelectionItem(UnityAction<InventoryRequestResult, ItemData> func, StationInterface stationInterface)
        {
            m_inventoryUIManager.OpenInventoryScreen(InventoryDisplayMode.SELECTION);
            m_displayMode = InventoryDisplayMode.SELECTION;
            m_OnSelectionRequestAnser.AddListener(func);
            m_currentStationInterface = stationInterface;
            m_isInventoryOpen = true;

        }

        public bool AnswerSelectionRequest(InventoryRequestResult result, out string resultMessage)
        {
            resultMessage = "";
            if (m_indexSelector >= m_displayedInventory.Count)
            {
                resultMessage = m_resultIndexValidString;
                return false;
            }

            if (!m_currentStationInterface.IsItemValid(m_displayedInventory[m_indexSelector]))
            {
                resultMessage = m_resultItemValidString;
                return false;
            }


            m_OnSelectionRequestAnser?.Invoke(result, m_displayedInventory[m_indexSelector]);
            m_OnSelectionRequestAnser.RemoveAllListeners();
            m_currentStationInterface = null;
            OpenInventory();
            return true;
        }


        #region Inventory Data Management Methods
        public void UseItem()
        {
            if (m_indexSelector >= m_displayedInventory.Count)
            {
                return;
            }

            ItemData item = m_displayedInventory[m_indexSelector];

            Debug.Log("[Inventory Data] Used " + item.itemName);

            if (item.itemType == ItemType.Consumable)
            {
                RemoveItem(item);
            }
            else if (item.itemType == ItemType.Equippable)
            {
                Debug.Log("[Inventory Data] Switch Equip State");
            }
            else if (item.itemType == ItemType.Interactable)
            {
                Debug.Log("[Inventory Data] Hold Interact Item");
                Item.CharacterHoldItem characterHoldItem = GameManager.GetPlayer().GetComponent<Item.CharacterHoldItem>();
                characterHoldItem.SetHoldItem(item);
            }

            item.UseItem();

        }

        // This method is used to add the cart items from the shopDataManager to the player items
        public void AddNewItems(Dictionary<ItemData, int> itemCart)
        {

            foreach (KeyValuePair<ItemData, int> item in itemCart)
            {
                bool b_hasKey = m_playerItems.ContainsKey(item.Key);

                if (b_hasKey)
                {
                    m_playerItems[item.Key] += item.Value;
                    OnAddingItem?.Invoke(item.Key, m_playerItems[item.Key]);
                }
                else
                {
                    m_playerItems.Add(item.Key, item.Value);
                    OnAddingItem?.Invoke(item.Key, item.Value);

                    // If it's an upgrade, use it right away
                    if (item.Key.itemType == ItemType.Upgrade)
                    {
                        item.Key.UseItem();
                    }
                }
            }

            OnAddingFinished?.Invoke();
        }

        public void AddNewItem(KeyValuePair<ItemData, int> item)
        {
            bool b_hasKey = m_playerItems.ContainsKey(item.Key);

            if (b_hasKey)
            {
                m_playerItems[item.Key] += item.Value;
            }
            else
            {
                m_playerItems.Add(item.Key, item.Value);
                OnAddingItem?.Invoke(item.Key, item.Value);
            }

            OnAddingFinished?.Invoke();
        }

        public void RemoveItem(ItemData item)
        {
            m_playerItems[item] -= 1;

            if (m_playerItems[item] == 0)
            {
                m_playerItems.Remove(item);
                m_displayedInventory.Remove(item);

                OnAddingItem?.Invoke(item, 0);
                OnRemovingItem?.Invoke();
                Debug.Log("An item has been removed!");
            }
            else
            {
                OnAddingItem?.Invoke(item, m_playerItems[item]);
                OnDecreasingItemAmount?.Invoke();
            }

        }
        #endregion

        #region Work Function

        public void ActivateWorkSession()
        {
            GameManager.RequestToSwitchGamePhase();
            GameManager.GetPlayer().GetComponent<Character>().ChangeState(CharacterState.NONE);
            m_OutsideUIPhase.LaunchOutsidePhase();
            m_OutsideUIPhase.OnEndOutsidePhase.AddListener(EndWorkSession);
        }

        public void EndWorkSession()
        {
            if (GameManager.GetCurrentGamePhase() != GamePhase.WORK)
                return;

            m_OutsideUIPhase.OnEndOutsidePhase.RemoveListener(EndWorkSession);
            GameManager.RequestToSwitchGamePhase();
            GameManager.GetPlayer().GetComponent<Character>().ChangeState(CharacterState.IDLE);
        }

        #endregion

        #region Getter & Setter
        public float GetPlayerBalance()
        {
            return m_playerBalance;
        }

        public void SetPlayerBalance(float newBalance)
        {
            m_playerBalance = newBalance;
        }

        public int GetItemCount(ItemData item)
        {
            if (m_playerItems.ContainsKey(item))
            {
                return m_playerItems[item];
            }
            else
            {
                return 0;
            }
        }

        public Dictionary<ItemData, int> GetPlayerItems()
        {
            return m_playerItems;
        }

        public int GetIndexSelector()
        {
            return m_indexSelector;
        }

        public void IncreaseIndexSelector()
        {
            if (m_indexSelector < m_displayedInventory.Count - 1)
            {
                m_indexSelector++;
                OnSelectorChanged?.Invoke();
            }
        }

        public void DecreaseIndexSelector()
        {
            if (m_indexSelector > 0)
            {
                m_indexSelector--;
                OnSelectorChanged?.Invoke();
            }
        }

        public void SetIndexSelector(int index)
        {
            m_indexSelector = index;
        }

        public List<ItemData> GetDisplayedInventory()
        {
            return m_displayedInventory;
        }

        public void AddItemInDisplayedInventory(ItemData item)
        {
            m_displayedInventory.Add(item);
        }

        public void ClearItemsFromDisplayedInventory()
        {
            m_displayedInventory.Clear();
        }
        #endregion
    }
}
