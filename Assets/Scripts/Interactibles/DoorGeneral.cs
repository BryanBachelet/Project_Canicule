using UnityEngine;

namespace Canicule
{
    public class DoorGeneral : MonoBehaviour
    {
        [HideInInspector] public Door m_doorCmpt;
        [HideInInspector] public Teleporter m_teleporterCmpt;
        [HideInInspector] public DoorGeneral m_doorLink;

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

    }
}
