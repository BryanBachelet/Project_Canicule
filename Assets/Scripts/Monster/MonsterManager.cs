using BorsalinoTools;
using System;
using UnityEngine;

namespace Canicule
{
    public class MonsterManager : MonoBehaviour, IPauseableObject
    {
        [SerializeField] private bool m_IsMonsterCannotAppear = false;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject m_monsterPrefab;

        [Header("Appearence")]
        [SerializeField] private float m_appearenceTemperature = 40.0f;

        private Monster m_monsterCmpt;

        private bool isPaused = false;

#if UNITY_EDITOR || UNITY_DEVELOPEMENT
        private const string m_spawnMonsterCommandStr = "SpawnMonster";
#endif

        #region Unity Functions
        public void OnEnable()
        {
            GameManager.RegisterObject(this);
#if UNITY_EDITOR || UNITY_DEVELOPEMENT
            CommandsConsole.OnCommandCall += OnCommandEnter;
            CommandsConsole.RegisterCommand(m_spawnMonsterCommandStr);
#endif
        }

        public void OnDisable()
        {
            GameManager.UnregisterObject(this);
#if UNITY_EDITOR || UNITY_DEVELOPEMENT
            CommandsConsole.OnCommandCall -= OnCommandEnter;
#endif
        }

        public void Update()
        {
            if (isPaused || m_IsMonsterCannotAppear)
                return;

            UpdateMonsterApparence();
        }
        #endregion

        public void InitiateManager()
        {
            GameManager.RegisterObject(this);
            SpawnMonster();
        }

        private void UpdateMonsterApparence()
        {
            if (m_monsterCmpt.IsAppear())
            {
                if (GameManager.GetCurrentGamePhase() != GamePhase.NIGHT)
                {
                    m_monsterCmpt.ChangeState(MonsterState.DISAPPEAR);
                    return;
                }

                return;
            }



            Rooms.Room currentRoom = GameManager.GetRoomManager().GetCurrentRoomPlayer();
            if (currentRoom.GetRoomDegree() < m_appearenceTemperature)
            {
                return;
            }

            LaunchMonsterAppearence(currentRoom);
        }


        /// <summary>
        /// Spawn the monster object in the game;
        /// </summary>
        public void SpawnMonster()
        {
            if (m_monsterCmpt)
                return;

            GameObject instance = GameObject.Instantiate(m_monsterPrefab, Vector3.zero, Quaternion.identity);
            m_monsterCmpt = instance.GetComponent<Monster>();
            m_monsterCmpt.Awake();
            m_monsterCmpt.gameObject.SetActive(false);
            m_monsterCmpt.ChangeState(MonsterState.NONE);

            GameObject playerInstance = GameManager.GetPlayer();
            m_monsterCmpt.SetTarget(playerInstance.transform);
        }

        /// <summary>
        /// Launch the monster appearence (visible for the player)
        /// </summary>
        /// <param name="room">Room where the monster will appear </param>
        public void LaunchMonsterAppearence(Rooms.Room room)
        {
            Vector3 playerInstancePosition = GameManager.GetPlayer().transform.position;

            Vector3[] points = room.GetAppearencePoint();
            float maxDistance = 0.0f;
            int validIndex = -1;

            for (int i = 0; i < points.Length; i++)
            {
                float distance = Vector2.Distance(playerInstancePosition, points[i]);
                if (distance > maxDistance)
                {
                    validIndex = i;
                }
            }

            Vector3 teleportPosition = room.transform.position + points[validIndex];  
            m_monsterCmpt.Teleport(teleportPosition);
            m_monsterCmpt.ChangeState(MonsterState.APPEAR);
            GameManager.GetRoomManager().SetCurrentRoomMonster(room);
        }

        #region Pauseable Functions
        public void OnPause()
        {
            isPaused = true;
        }

        public void OnPlay()
        {
            isPaused = false;
        }

        public void OnRegister()
        {

        }

        public Monster GetMonster()
        {
           return  m_monsterCmpt ;
        }
        #endregion

#if UNITY_EDITOR || UNITY_DEVELOPEMENT
        public void OnCommandEnter(string commandStr, params object[] args)
        {
            if(commandStr == m_spawnMonsterCommandStr)
            {
                SpawnMonsterCommand();
            }    
        }

        private void SpawnMonsterCommand()
        {
            LaunchMonsterAppearence(GameManager.GetRoomManager().GetCurrentRoomPlayer());
        }
#endif
    }
}
