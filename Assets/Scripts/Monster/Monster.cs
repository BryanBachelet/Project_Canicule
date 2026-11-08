using UnityEngine;
using UnityEngine.AI;

using NavMeshPlus.Components;
using NavMeshPlus.Extensions;

using System;
using System.Collections.Generic;

using Canicule.Rooms;
using UnityEngine.UIElements;
using UnityEngine.LowLevel;

namespace Canicule
{
    public enum MonsterState
    {
        NONE = 0,
        APPEAR = 1,
        DISAPPEAR = 2,
        MOVE = 3,
        PHASE = 4,
        CATCH = 5,
        SEARCH = 6,
    }


    [RequireComponent(typeof(NavMeshAgent))]
    public class Monster : MonoBehaviour, IPauseableObject
    {
        private MonsterState m_currentState;

        // Components 
        [Header("Components")]
        [SerializeField] private Animator m_monsterAnimator;
        private NavMeshAgent m_navMeshAgent;
        private Rooms.RoomManager roomManager;


        [Header("Move Parameters")]
        [SerializeField] private float m_speed;
        [SerializeField] private float m_speedMalus;
        [SerializeField] private Transform m_target;
        private const float m_cooldownRefreshTimer = .5f;
        private float m_moveRefreshTimer = 0.0f;

        [Header("Appear Parameters")]
        [SerializeField] private float m_appearDuration;
        [SerializeField] private AnimationCurve m_appearSpeedMalus;
        private float m_appearTimer;

        [Header("Disappear Parameters")]
        [SerializeField] private float m_disappearDuration;
        [SerializeField] private AnimationCurve m_disappearSpeedMalus;
        private float m_disappearTimer;

        [Header("Catch Parameter")]
        [SerializeField] private Collider2D m_catchCollider;
        [SerializeField] private float m_catchDistance;

        [Header("Search Parameters")]
        private List<Rooms.RoomConnection> m_roomNavList = new List<RoomConnection>();
        private Room m_lastPlayerRoom = null;
        private Teleporter m_currentDoorTarget = null;
        private const float m_doorDetectionDistance = 0.5f;

        private bool m_onPause= false;

        #region Unity Functions

        public void Awake()
        {
            GameManager.RegisterObject(this);

            m_currentState = MonsterState.NONE;

            m_navMeshAgent = GetComponent<NavMeshAgent>();
            m_catchCollider.enabled = false;

        }

        public void Start()
        {
            roomManager = GameManager.GetRoomManager();
            ChangeState(MonsterState.APPEAR);
        }

        public void Update()
        {
            if (m_onPause) return;

            Process_State();
        }

        #endregion

        public void ChangeState(MonsterState state)
        {
            EndState(state);
            Debug.Log("[Monster] Change monster state from " + m_currentState + " to " + state);
            m_currentState = state;
            StartState(state);

            m_monsterAnimator.SetInteger("MonsterState", (int)m_currentState);
        }

        private void EndState(MonsterState state)
        {
            switch (state)
            {
                case MonsterState.NONE:
                    break;
                case MonsterState.APPEAR:
                    break;
                case MonsterState.DISAPPEAR:
                    m_navMeshAgent.isStopped = true;
                    break;
                case MonsterState.MOVE:
                    break;
                case MonsterState.PHASE:
                    break;
                case MonsterState.CATCH:
                    break;
                default:
                    break;
            }
        }

        private void StartState(MonsterState state)
        {
            switch (state)
            {
                case MonsterState.NONE:
                    break;
                case MonsterState.APPEAR:
                    gameObject.SetActive(true);
                    m_navMeshAgent.isStopped = false;
                    m_appearTimer = m_appearDuration;
                    m_navMeshAgent.SetDestination(m_target.position);
                    break;

                case MonsterState.DISAPPEAR:
                    m_disappearTimer = m_disappearDuration;
                    m_navMeshAgent.isStopped = false;
                    m_navMeshAgent.SetDestination(m_target.position);
                    break;

                case MonsterState.MOVE:
                    m_moveRefreshTimer = m_cooldownRefreshTimer;
                    m_navMeshAgent.isStopped = false;
                    m_navMeshAgent.SetDestination(m_target.position);
                    break;
                case MonsterState.PHASE:
                    StartProcess_Phase();
                    break;
                case MonsterState.CATCH:
                    StartCatch();
                    break;
                case MonsterState.SEARCH:
                    StartState_Search();
                    break;
                default:
                    break;
            }
        }
        public void Teleport(Vector3 position)
        {
            NavMeshHit hit = new NavMeshHit();
            const float m_distanceSamplePoint = 10.0f;
            if (NavMesh.SamplePosition(position, out hit, m_distanceSamplePoint, NavMesh.AllAreas))
            {
                m_navMeshAgent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning($"[Monster] Couldn't teleport because monster navmesh can't find a point on the navmesh point");
            }
        }

        #region Process Functions
        private void Process_State()
        {
            switch (m_currentState)
            {
                case MonsterState.APPEAR:
                    Process_Appear();
                    break;
                case MonsterState.DISAPPEAR:
                    Process_Disappear();
                    break;
                case MonsterState.MOVE:
                    Process_Move();
                    break;
                case MonsterState.CATCH:
                    Process_Catch();
                    break;
                case MonsterState.SEARCH:
                    Process_Search();
                    break;
                default:
                    break;
            }
        }

       
        private void StartState_Search()
        {
            Room playerRoom = roomManager.GetCurrentRoomPlayer();
            if (m_roomNavList.Count == 0 || m_lastPlayerRoom != playerRoom)
            {
                m_roomNavList = roomManager.ComputeRoomNavigation(
                   roomManager.GetCurrentRoomPlayer(),
                   roomManager.GetCurrentRoomMonster()
                   );

                m_lastPlayerRoom = playerRoom;
            }

            Vector3 doorPosition = m_roomNavList[0].teleporter.transform.position;
            m_currentDoorTarget = m_roomNavList[0].teleporter;

            NavMeshHit hit = new NavMeshHit();
            if (NavMesh.SamplePosition(doorPosition, out hit, 10000.0f, NavMesh.AllAreas))
            {
                m_navMeshAgent.SetDestination(hit.position);
                m_roomNavList.RemoveAt(0);
            }
            else
            {
                Debug.Log("[Navigation] Couldn't find a point close from the door");
            }

        }

        private void Process_Search()
        {
            if (roomManager.GetCurrentRoomMonster() == roomManager.GetCurrentRoomPlayer())
            {
                ChangeState(MonsterState.MOVE);
            }

            Room playerRoom = roomManager.GetCurrentRoomPlayer();
            if (m_lastPlayerRoom != playerRoom)
            {
                StartState_Search();
            }

            m_navMeshAgent.speed = m_speed * m_speedMalus;

            if (m_navMeshAgent.remainingDistance < m_doorDetectionDistance)
            {
                ChangeState(MonsterState.PHASE);
            }
        }

        private void StartProcess_Phase()
        {
            m_currentDoorTarget.LaunchInteraction(transform, Interactor.MONSTER);
            m_currentDoorTarget.OnInteractEnd.AddListener(OnPhaseFinish);
        }

        private void OnPhaseFinish()
        {
            m_currentDoorTarget.OnInteractEnd.RemoveListener(OnPhaseFinish);

            if (roomManager.GetCurrentRoomMonster() != roomManager.GetCurrentRoomPlayer())
            {
                ChangeState(MonsterState.SEARCH);
            }
            else
            {
                ChangeState(MonsterState.MOVE);
            }
        }

        private void Process_Move()
        {
            if (roomManager.GetCurrentRoomMonster() != roomManager.GetCurrentRoomPlayer())
            {
                ChangeState(MonsterState.SEARCH);
            }

            m_navMeshAgent.speed = m_speed * m_speedMalus;

            RefreshDestination();

            float distance = Vector3.Distance(transform.position, m_target.position);
            if (distance < m_catchDistance)
            {
                ChangeState(MonsterState.CATCH);
            }
        }

        private void RefreshDestination()
        {
            m_moveRefreshTimer -= Time.deltaTime;
            if (m_moveRefreshTimer >= 0.0f) return;

            m_moveRefreshTimer = m_cooldownRefreshTimer;
            m_navMeshAgent.SetDestination(m_target.position);

        }

        private void Process_Catch()
        {

        }

        private void Process_Appear()
        {
            float ratio = (m_appearDuration - m_appearTimer) / m_appearDuration;
            m_speedMalus = m_appearSpeedMalus.Evaluate(ratio);
            m_navMeshAgent.speed = m_speed * m_speedMalus;

            m_appearTimer -= Time.deltaTime;
            if (m_appearTimer > 0.0f) return;

            ChangeState(MonsterState.MOVE);
            m_speedMalus = 1.0f;
        }

        private void Process_Disappear()
        {
            float ratio = (m_disappearDuration - m_disappearTimer) / m_disappearDuration;
            m_speedMalus = m_disappearSpeedMalus.Evaluate(ratio);
            m_navMeshAgent.speed = m_speed * m_speedMalus;

            m_disappearTimer -= Time.deltaTime;
            if (m_disappearTimer > 0.0f) return;

            ChangeState(MonsterState.NONE);
            m_speedMalus = 1.0f;
        }


        #endregion

        #region Collision Functions
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnHit(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            OnHit(collision);
        }
        #endregion

        #region Catch Function
        public void StartCatch()
        {
            m_navMeshAgent.isStopped = true;
        }

        public void ActiveCatchCollider()
        {
            if (m_currentState != MonsterState.CATCH) return;

            m_catchCollider.enabled = true;
        }

        public void DeactiveActiveCatchCollider()
        {
            if (m_currentState != MonsterState.CATCH) return;

            m_catchCollider.enabled = false;
        }

        public void FinishCatchAnimation()
        {
            ChangeState(MonsterState.MOVE);
        }

        private void OnHit(Collider2D collision)
        {
            if ( m_currentState != MonsterState.CATCH || collision.transform.tag != "Player")
            {
                return;
            }
            GameManager.LaunchLose();
            GameManager.GetGameManager().PauseGame();

            Debug.Log("Player is catch");
        }
        #endregion

        public MonsterState GetState() { return m_currentState; }
        public bool IsAppear() { return m_currentState != MonsterState.NONE; }

        #region Pauseable Functions
        public void OnPause()
        {
            m_onPause = true;
        }

        public void OnPlay()
        {
            m_onPause = false;
        }

        public void OnRegister()
        {

        }

        #endregion

        public void SetTarget(Transform target)
        {
            m_target = target;
        }

        #region Debugs Functions

        public void OnDrawGizmos()
        {

            if (m_roomNavList.Count == 0)
                return;

            for (int i = 0; i < m_roomNavList.Count; i++)
            {
                Gizmos.DrawSphere(m_roomNavList[i].teleporter.transform.position, .4f);
                Gizmos.DrawSphere(m_roomNavList[i].roomLink.transform.position, .4f);

                Gizmos.DrawLine(m_roomNavList[i].teleporter.transform.position,
                m_roomNavList[i].roomLink.transform.position);

            }
        }

        #endregion
    }
}
