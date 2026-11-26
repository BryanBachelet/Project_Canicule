using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Canicule
{
    public enum ShapeType
    {
        Square = 0,
        Round = 1,
        Triangle = 2,
    }

    public class ShapeBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform m_rectTransform;
        [SerializeField] private Canvas m_canvas;
        private Vector2 m_startPosition;
        private Vector2 m_previousPosition;
        private Image m_image;
        
        [SerializeField] private WorkUIManager m_workUIManager;
        [SerializeField] private RectTransform m_workAppRect;
        [SerializeField] private ShapeType m_shapeType;

        #region Unity Methods
        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_canvas = GetComponentInParent<Canvas>();
            m_image = GetComponent<Image>();
        }

        private void Start()
        {
            m_startPosition = m_rectTransform.anchoredPosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_previousPosition = m_rectTransform.anchoredPosition;
            m_image.raycastTarget = false;
            m_workUIManager.SetShapeCurrentlyHold(this.gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_rectTransform.anchoredPosition += eventData.delta / m_canvas.scaleFactor;

            if(m_rectTransform.anchoredPosition.x < 0 
                || m_rectTransform.anchoredPosition.y < 0 
                || m_rectTransform.anchoredPosition.x > m_workAppRect.rect.width
                || m_rectTransform.anchoredPosition.y > m_workAppRect.rect.height)
            {
                eventData.pointerDrag = null;
                m_image.raycastTarget = true;
                m_rectTransform.anchoredPosition = m_startPosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_image.raycastTarget = true;
        }
        #endregion

        public void ResetShapePosition()
        {
            m_rectTransform.anchoredPosition = m_previousPosition;
        }

        public void InitializeShape(WorkUIManager workUIManager, Canvas canvas, Vector2 startPos, RectTransform workAppRect)
        {
            m_workUIManager = workUIManager;
            m_canvas = canvas;
            m_startPosition = startPos;
            m_workAppRect = workAppRect;

            m_rectTransform.anchoredPosition = startPos;

            m_rectTransform.anchorMin = Vector2.zero;
            m_rectTransform.anchorMax = Vector2.zero;
        }

        #region Getter
        public ShapeType GetShapeType()
        {
            return m_shapeType;
        }

        public Image GetImage()
        {
            return m_image;
        }

        #endregion
    }
}
