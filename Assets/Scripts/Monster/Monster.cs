using UnityEngine;
using UnityEngine.AI;
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
    }


    [RequireComponent(typeof(NavMeshAgent))]
    public class Monster : MonoBehaviour, IPauseableObject
    {
        private MonsterState m_currentState;

        // Components 
        [Header("Components")]
        [SerializeField] private Animator m_monsterAnimator;
        private NavMeshAgent m_navMeshAgent;


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
            ChangeState(MonsterState.APPEAR);
        }

        public void Update()
        {
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
                    break;
                case MonsterState.CATCH:
                    StartCatch();
                    break;
                default:
                    break;
            }
        }


        #region Process Functions
        private void Process_State()
        {
            switch (m_currentState)
            {
                case MonsterState.NONE:
                    
                    break;
                case MonsterState.APPEAR:
                    Process_Appear();
                    break;
                case MonsterState.DISAPPEAR:
                    Process_Disappear();
                    break;
                case MonsterState.MOVE:
                    Process_Move();
                    break;
                case MonsterState.PHASE:
                    break;
                case MonsterState.CATCH:
                    Process_Catch();
                    break;
                default:
                    break;
            }
        }

        private void Process_Move()
        {
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
            if (m_currentState != MonsterState.CATCH || collision.transform.tag != "Player") return;

            //TODO: Set the catch system on the player.

            Debug.Log("Player is catch");

        }
        #endregion
    }
}
