using Canicule.Rooms;
using UnityEngine;

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

        public override void Start()
        {
            base.Start();
            m_animator = GetComponent<Animator>();
            m_doorGeneral = GetComponentInParent<DoorGeneral>();
            SetAnimatorBools();
        }

        protected override void Interact()
        {
            m_doorGeneral.OpenLinkDoor();
            OpenDoor();
            CreateDraft();


            base.Interact();
        }

        private void OpenDoor()
        {
            m_bIsDoorOpen = !m_bIsDoorOpen;
            UpdateAnimatorDoor();
        }

        public void SetDoorOpen()
        {
            OpenDoor();
        }

        public override void Update()
        {
            base.Update();

            if (!m_bIsDoorOpen && !m_roomOwner.gameObject.activeSelf)
            {
                return;
            }

            CreateDraft();
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

        public void CreateDraft()
        {
            Room linkRoom = m_doorGeneral.m_doorLink.m_doorCmpt.m_roomOwner;
            Door linkDoor = m_doorGeneral.m_doorLink.m_doorCmpt;
            if (!m_bIsDoorOpen || !m_roomOwner.IsAWindowOpen() || !linkRoom.IsAWindowOpen() || m_draftBehavior)
            {
                return;
            }

            GameObject draftInstance = GameObject.Instantiate(m_draftPrefab, m_doorGeneral.transform);
            DraftBehavior draftBehavior = draftInstance.GetComponent<DraftBehavior>();

            DraftBehavior.DraftData draftDataA = new DraftBehavior.DraftData();
            draftDataA.door = this;
            draftDataA.room = m_roomOwner;

            DraftBehavior.DraftData draftDataB = new DraftBehavior.DraftData();
            draftDataB.door = linkDoor;
            draftDataB.room = linkRoom;

            draftBehavior.InitDraft(draftDataA, draftDataB);
            m_draftBehavior = draftBehavior;
            m_draftBehavior.OnDraftDestroy.AddListener(RemoveActiveDraft);
            m_draftBehavior.OnDraftDestroy.AddListener(linkDoor.RemoveActiveDraft);
        }

        public void RemoveActiveDraft()
        {
            m_draftBehavior = null;
        }


        public bool IsOpen() { return m_bIsDoorOpen; }

    }
}

