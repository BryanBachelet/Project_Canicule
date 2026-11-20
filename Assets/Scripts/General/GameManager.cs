using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace Canicule
{
    public enum GamePhase
    {
        NIGHT = 0,
        MORNING = 1,
        WORK = 2,
    }


    [DefaultExecutionOrder(-999)]
    // Ensure game manager runs first
    // To be replaced with an initialization system
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance;
        private List<IPauseableObject> m_pauseableObject = new List<IPauseableObject>();
        private GamePhase m_currentGamePhase = GamePhase.NIGHT;
        public UnityEvent<GamePhase> OnGamePhaseChange;


        public GameObject playerObj;
        // Manager Variables
        private Rooms.RoomManager m_roomManager;
        private MonsterManager m_monsterManager;
        private ExhaustionManager m_exhaustionManager;
        private ShopDataManager m_shopDataManager;
        private InventoryDataManager m_inventoryDataManager;
        private InputManager m_inputManager;
        private SleepManager m_sleepManager;
        private WorkDataManager m_workDataManager;
        private NightDayManager m_nightDayManager;
        private Canicule.Temperature.EnvirommentHeatSource m_environementTemperature;
        private ItemEffectManager m_itemEffectManager;

        public UnityEvent OnPause;
        public UnityEvent OnPlay;

        [HideInInspector] public UnityEvent OnLose;

        [SerializeField] private bool m_showDebug = false;

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
            m_inputManager = GetComponent<InputManager>();
            m_sleepManager = GetComponent<SleepManager>();
            m_workDataManager = GetComponent<WorkDataManager>();
            m_nightDayManager = GetComponent<NightDayManager>();
            m_environementTemperature = GetComponent<Canicule.Temperature.EnvirommentHeatSource>();
            m_itemEffectManager = GetComponent <ItemEffectManager>();
        }

        private void Start()
        {
            m_shopDataManager.InitiateManager();
            m_monsterManager.InitiateManager();
            playerObj.GetComponent<Character>().ChangeState(CharacterState.IDLE); // To be replaced by an initiation of the player manager
        }

        private void Update()
        {
            if(m_showDebug)
            {
                BorsalinoTools.ScreenDebuggerTool.AddMessage($"Game State : {m_currentGamePhase.ToString()}");
            }
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

        public void SwitchNextGamePhase()
        {
            int enumCount = Enum.GetNames(typeof(GamePhase)).Length;
            int currentGamePhaseIndex = (int)m_currentGamePhase;

            currentGamePhaseIndex++;
            if (currentGamePhaseIndex == enumCount)
            {
                currentGamePhaseIndex = 0;
            }

            m_currentGamePhase = (GamePhase)currentGamePhaseIndex;
            OnGamePhaseChange?.Invoke(m_currentGamePhase);

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

        public static bool SubscribeToOnLoseEvent(UnityAction func)
        {
            if (!m_instance)
                return false;

            m_instance.OnLose?.AddListener(func);
            return true;
        }

        public static ExhaustionManager GetExhaustionManager()
        {
            if (m_instance == null || !m_instance.m_exhaustionManager)
                return null;

            return m_instance.m_exhaustionManager;
        }

        public static bool UnsubscribeToOnLoseEvent(UnityAction func)
        {
            if (!m_instance)
                return false;

            m_instance.OnLose?.RemoveListener(func);
            return true;
        }

        public static void LaunchLose()
        {
            if (!m_instance)
                return;

            m_instance.OnLose?.Invoke();
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

        public static InputManager GetInputManager()
        {
            if (m_instance == null || !m_instance.m_inputManager)
                return null;

            return m_instance.m_inputManager;
        }

        public static SleepManager GetSleepManager()
        {
            if (m_instance == null || !m_instance.m_sleepManager)
                return null;

            return m_instance.m_sleepManager;
        }

        public static WorkDataManager GetWorkDataManager()
        {
            if (m_instance == null || !m_instance.m_workDataManager)
                return null;

            return m_instance.m_workDataManager;
        }

        public static NightDayManager GetNightDayManager()
        {
            if (m_instance == null || !m_instance.m_nightDayManager)
                return null;

            return m_instance.m_nightDayManager;
        }

        public static Canicule.Temperature.EnvirommentHeatSource GetEnvirommentHeatSource()
        {
            if (m_instance == null || !m_instance.m_environementTemperature)
                return null;

            return m_instance.m_environementTemperature;
        }
        public static ItemEffectManager GetItemEffectManager()
        {
            if (m_instance == null || !m_instance.m_itemEffectManager)
                return null;

            return m_instance.m_itemEffectManager;
        }

        #region Game Phase Functions
        public static GamePhase GetCurrentGamePhase()
        {
            return m_instance.m_currentGamePhase;
        }

        public static void RequestToSwitchGamePhase()
        {
            m_instance.SwitchNextGamePhase();
        }

        public static void SubcribeToOnGameEventChange(UnityAction<GamePhase> func )
        {
            m_instance.OnGamePhaseChange.AddListener(func);
        }

        public static void UnsubcribeToOnGameEventChange(UnityAction<GamePhase> func)
        {
            m_instance.OnGamePhaseChange.RemoveListener(func);
        }

        #endregion

        #endregion

       

    }
}
