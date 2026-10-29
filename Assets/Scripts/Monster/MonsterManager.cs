using UnityEngine;

namespace Canicule
{
    public class MonsterManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject m_monsterPrefab;

        private Monster m_monsterCmpt;

        /// <summary>
        /// Spawn the monster object in the game;
        /// </summary>
        public void SpawnMonster()
        {
            GameObject instance = GameObject.Instantiate(m_monsterPrefab, Vector3.zero, Quaternion.identity);
            m_monsterCmpt = instance.GetComponent<Monster>();
            m_monsterCmpt.gameObject.SetActive(false);
            m_monsterCmpt.ChangeState(MonsterState.NONE);
        }

        /// <summary>
        /// Launch the monster appearence (visible for the player)
        /// </summary>
        /// <param name="room">Room where the monster will appear </param>
        public void LaunchMonsterAppearence(Rooms.Room room)
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }
}
