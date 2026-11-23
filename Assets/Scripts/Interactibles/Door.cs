using Canicule.Rooms;
using UnityEngine;
using FMODUnity;
namespace Canicule
{
    public class Door : InteractibleObject
    {
        private bool m_bIsDoorOpen = false;
        private Animator m_animator;

        private const string openStateNameAnimator = "Open";
        private const string openingStateNameAnimator = "Opening";
        private const string closeStateNameAnimator = "Close";
        private const string closingStateNameAnimator = "Closing";

        private DoorGeneral m_doorGeneral;
        [SerializeField] private GameObject m_draftPrefab = null;
        private DraftBehavior m_draftBehavior = null;
        private bool m_isDraftActive;

        [SerializeField] private TimerObject m_timerDrawDraftCloseDoor;
        [SerializeField] private float m_rateDoorCloseOfDraft = .1f;

        private StudioEventEmitter m_doorSlamEmitter;

        public override void Start()
        {
            base.Start();
            m_animator = GetComponent<Animator>();
            m_doorGeneral = GetComponentInParent<DoorGeneral>();
            SetAnimatorBools();
            m_doorSlamEmitter = GetComponent<StudioEventEmitter>();
        }

        protected override void Interact()
        {
            m_doorGeneral.OpenLinkDoor();
            if (!m_bIsDoorOpen)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }

            base.Interact();
        }

        private void OpenDoor()
        {
            m_bIsDoorOpen = true;
            m_doorGeneral.OnDoorOpened?.Invoke();
            SwitchDoorState();
        }

        private void CloseDoor()
        {
            m_bIsDoorOpen = false;
            m_doorGeneral.OnDoorClosed?.Invoke();
            SwitchDoorState();
        }

    private void SwitchDoorState()
        {
            if (m_bIsDoorOpen)
            {
                m_doorGeneral.ActiveTP();
            }
            else
            {
                m_doorGeneral.DeactiveTP();
            }

            UpdateAnimatorDoor();
        }

        public void SetDoorOpen()
        {
            if (!m_bIsDoorOpen)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }

        public override void Update()
        {
            base.Update();

            if (!m_bIsDoorOpen && !m_roomOwner.gameObject.activeSelf)
            {
                return;
            }
            UpdateDraftDoor();
        }

        public void OnEnable()
        {
            SetAnimatorBools();
        }

        private void UpdateAnimatorDoor()
        {
            if (!m_animator) return;

            m_animator.SetBool(closingStateNameAnimator, !m_bIsDoorOpen);
            m_animator.SetBool(openingStateNameAnimator, m_bIsDoorOpen);
        }
        private void SetAnimatorBools()
        {
            if (!m_animator) return;

            m_animator.SetBool(closeStateNameAnimator, !m_bIsDoorOpen);
            m_animator.SetBool(openStateNameAnimator, m_bIsDoorOpen);
        }

        private void UpdateDraftDoor()
        {
            if (!m_isDraftActive) return;

            if(m_timerDrawDraftCloseDoor.UpdateTimer())
            {
                float draw = Random.Range(0.0f, 1.0f);
                
                if(m_rateDoorCloseOfDraft > draw)
                {
                    SlamDoor();
                }
            }
        }

        private void SlamDoor()
        {
            GameManager.GetDraftManager().OnDoorClosed(m_doorGeneral);
            CloseDoor();
            m_doorGeneral.OpenLinkDoor();
            m_doorSlamEmitter.Play();
        }

        public void CreateDraft(Room starterRoom,Room enderRoom)
        {
            GameObject draftInstance = GameObject.Instantiate(m_draftPrefab, m_doorGeneral.transform);
            DraftBehavior draftBehavior = draftInstance.GetComponent<DraftBehavior>();

            DraftBehavior.DraftData draftDataA = new DraftBehavior.DraftData();
            draftDataA.door = this;
            draftDataA.room = starterRoom;

            DraftBehavior.DraftData draftDataB = new DraftBehavior.DraftData();
            Door linkDoor = m_doorGeneral.m_doorLink.m_doorCmpt;
            draftDataB.door = linkDoor;
            draftDataB.room = enderRoom;

            draftBehavior.InitDraft(draftDataA, draftDataB);
            m_draftBehavior = draftBehavior;
            m_isDraftActive = true;
        }

        public void DeleteDraft()
        {
            m_draftBehavior.DestroyDraft();
            m_draftBehavior = null;
            m_isDraftActive = false;
        }

        public bool IsDraftActive() { return m_isDraftActive; }
        public bool IsOpen() { return m_bIsDoorOpen; }

    }
}

