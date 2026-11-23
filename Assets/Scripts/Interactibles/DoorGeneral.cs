using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class DoorGeneral : MonoBehaviour
    {
        [HideInInspector] public Door m_doorCmpt;
        [HideInInspector] public Teleporter m_teleporterCmpt;
        [HideInInspector] public DoorGeneral m_doorLink;

        [HideInInspector] public UnityEvent OnDoorClosed;
        [HideInInspector] public UnityEvent OnDoorOpened;
        private void Awake()
        {
            m_doorCmpt = GetComponentInChildren<Door>();
            m_teleporterCmpt = GetComponentInChildren<Teleporter>();
            m_doorLink = m_teleporterCmpt.GetDoorTransformLink().GetComponent<DoorGeneral>();
        }

        public void OpenLinkDoor()
        {
            m_doorLink.m_doorCmpt.SetDoorOpen();
        }

        public bool HasDraft()
        {
            return m_doorCmpt.IsDraftActive();
        }

        public bool IsDoorOpen() { return m_doorCmpt.IsOpen(); }
        public void ActiveTP()
        {
            m_teleporterCmpt.ActiveTeleportCollider();
        }

        public void DeactiveTP()
        {
            m_teleporterCmpt.DeactiveTeleportCollider();
        }

        public void DeleteDraft()
        {
            m_doorCmpt.DeleteDraft();
        }

    }
}
