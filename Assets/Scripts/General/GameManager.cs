using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace Canicule
{
    [DefaultExecutionOrder(-999)] 
    // Ensure game manager runs first
    // To be replaced with an initialization system
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance;
        private List<IPauseableObject> m_pauseableObject = new List<IPauseableObject>();

        public GameObject playerObj;
        // Manager Variables
        private Rooms.RoomManager m_roomManager;
        private MonsterManager m_monsterManager;
        private ExhaustionManager m_exhaustionManager;
        private ShopDataManager m_shopDataManager;
        private InventoryDataManager m_inventoryDataManager;
        private WorkDataManager m_workDataManager;
        private SleepManager m_sleepManager;
        private InputManager m_inputManager;

        public UnityEvent OnPause;
        public UnityEvent OnPlay;

        #region Unity Functions
        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            m_roomManager = GetComponent<Rooms.RoomManager>();
            m_monsterManager = GetComponent<MonsterManager>();
            m_exhaustionManager = GetComponent<ExhaustionManager>();
            m_shopDataManager = GetComponent<ShopDataManager>();
            m_inventoryDataManager = GetComponent<InventoryDataManager>();
        }

        private void Start()
        {
            //m_monsterManager.InitiateManager();
        }
        #endregion

        public void PauseGame()
        {
            for (int i = 0; i < m_pauseableObject.Count; i++)
            {
                m_pauseableObject[i].OnPause();
            }

            OnPause?.Invoke();
        }

        public void PlayGame()
        {
            for (int i = 0; i < m_pauseableObject.Count; i++)
            {
                m_pauseableObject[i].OnPlay();
            }
            OnPlay?.Invoke();
        }


        #region Static Functions
        public static void RegisterObject(IPauseableObject obj)
        {
            if (m_instance == null)
                return;

            m_instance.m_pauseableObject.Add(obj);
            obj.OnRegister();
        }

        public static void UnregisterObject(IPauseableObject obj)
        {
            if (m_instance == null)
                return;

            m_instance.m_pauseableObject.Remove(obj);
        }

        public static GameManager GetGameManager()
        {
            if (m_instance == null)
                return null;

            return m_instance;
        }

        public static Rooms.RoomManager GetRoomManager()
        {
            if (m_instance == null || !m_instance.m_roomManager)
                return null;

            return m_instance.m_roomManager;
        }

        public static GameObject GetPlayer()
        {
            if (!m_instance || !m_instance.playerObj)
                return null;

            return m_instance.playerObj;
        }

        public static ExhaustionManager GetExhaustionManager()
        {
            if (m_instance == null || !m_instance.m_exhaustionManager)
                return null;

            return m_instance.m_exhaustionManager;
        }

        public static ShopDataManager GetShopDataManager()
        {
            if (m_instance == null || !m_instance.m_shopDataManager)
                return null;

            return m_instance.m_shopDataManager;
        }
        public static InventoryDataManager GetInventoryDataManager()
        {
            if (m_instance == null || !m_instance.m_inventoryDataManager)
                return null;

            return m_instance.m_inventoryDataManager;
        }
        #endregion

    }
}
