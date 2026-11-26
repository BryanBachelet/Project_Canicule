using UnityEngine;
using UnityEngine.EventSystems;

namespace Canicule
{
    public class ShapeReceiver : MonoBehaviour, IDropHandler
    {
        [SerializeField] private WorkUIManager m_workUIManager;
        [SerializeField] private ShapeType m_shapeTypeAccepted;

        private WorkDataManager m_workDataManager;

        private void Awake()
        {
            m_workDataManager = GameManager.GetWorkDataManager();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ShapeBehavior droppedShape = m_workUIManager.GetShapeCurrentlyHold().GetComponent<ShapeBehavior>();

            if (droppedShape.GetShapeType() == m_shapeTypeAccepted)
            {
                Destroy(droppedShape.gameObject);
                m_workDataManager.GainMoney();
                m_workUIManager.SpawnShape();
            }
            else
            {
                droppedShape.ResetShapePosition();
            }
        }
    }
}
