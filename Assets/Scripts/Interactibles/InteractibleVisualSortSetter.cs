using UnityEngine;

namespace Canicule
{
    public class InteractibleVisualSortSetter : MonoBehaviour
    {
        [SerializeField] private GameObject m_movementCollider;

        void Start()
        {
            // Get the 2D Colliders
            Collider2D sourceCollider = m_movementCollider.GetComponent<Collider2D>();
            Collider2D targetCollider = gameObject.AddComponent(sourceCollider.GetType()) as Collider2D;

            // Set the target values to the source values
            if (targetCollider is BoxCollider2D targetBox && sourceCollider is BoxCollider2D sourceBox)
            {
                targetBox.size = sourceBox.size;
                targetBox.offset = sourceBox.offset;
            }
            else if (targetCollider is PolygonCollider2D targetPolygon && sourceCollider is PolygonCollider2D sourcePolygon)
            {
                targetPolygon.points = sourcePolygon.points;
            }
        }
    }
}
