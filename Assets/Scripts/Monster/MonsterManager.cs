using UnityEngine;

namespace Canicule
{
    public class MonsterManager : MonoBehaviour, IPauseableObject
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject m_monsterPrefab;

        [Header("Appearence")]
        [SerializeField] private float m_appearenceTemperature = 40.0f;

        private Monster m_monsterCmpt;

        private bool isPaused = false;

        #region Unity Functions
        public void OnEnable()
        {
            GameManager.RegisterObject(this);
        }

        public void OnDisable()
        {
            GameManager.UnregisterObject(this);
        }

        public void Update()
        {
            if (isPaused)
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
                return;
            }

            Rooms.Room currentRoom = GameManager.GetRoomManager().GetCurrentRoom();
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
            GameObject playerInstance = GameManager.GetPlayer();

            Vector3[] points = room.GetAppearencePoint();
            float maxDistance = 0.0f;
            int validIndex = -1;
            for (int i = 0; i < points.Length; i++)
            {
                float distance = Vector2.Distance(playerInstance.transform.position, points[i]);
                if (distance > maxDistance)
                {
                    validIndex = i;
                }

            }

            Vector3 teleportPosition = points[validIndex];
            m_monsterCmpt.Teleport(teleportPosition);
            m_monsterCmpt.ChangeState(MonsterState.APPEAR);
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
        #endregion
    }
}
