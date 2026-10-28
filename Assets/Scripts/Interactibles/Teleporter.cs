using UnityEngine;

namespace Canicule
{
    public class Teleporter : InteractibleObject
    {
        [SerializeField] private Rooms.Room m_roomLink;
        [Space]
        [SerializeField] private Transform m_teleportPoint;
        [SerializeField] private Vector3 m_offsetTeleportPoint= Vector3.zero;

        [Header("Debugs Options")]
        [SerializeField] private bool m_showTeleportPoint;
        private static readonly Color m_debugSphereColorStr = Color.rebeccaPurple;
        private const float m_debugSphereSize = .4f;

        public override void Start()
        {
            base.Start();

            if(m_roomLink == null)
            {
                Debug.LogError("[Door] Door don't have the linked room");
                return;
            }

            Rooms.RoomConnection roomConnection = new Rooms.RoomConnection(m_roomLink, this);
            m_roomOwner.LinkRoomConnection(roomConnection);
        }

        protected override void Interact()
        {
            m_roomLink.ActivateRoom();
            m_roomOwner.DeactivateRoom();
            actorTransform.position = m_teleportPoint.position + m_offsetTeleportPoint;
            
            base.Interact();
        }

#if UNITY_DEVELOPMENT || UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!m_showTeleportPoint) return;

            Gizmos.color = m_debugSphereColorStr;
            Gizmos.DrawLine(transform.position, m_teleportPoint.position + m_offsetTeleportPoint);
            Gizmos.DrawSphere(m_teleportPoint.position + m_offsetTeleportPoint, m_debugSphereSize);
        }
#endif
    }
}
