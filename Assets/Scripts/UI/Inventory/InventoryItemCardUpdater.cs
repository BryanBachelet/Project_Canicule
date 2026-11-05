using System;
using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class InventoryItemCardUpdater : MonoBehaviour
    {
        [SerializeField] private GameObject m_selectorArrows;
        [SerializeField] private float m_lerpSpeed = 15f;

        private InventoryDataManager m_inventoryDataManager;
        private InventoryUIManager m_inventoryUIManager;
        private RectTransform m_rectTransform;
        private int m_rank;
        private Vector2 m_targetPosition = Vector2.zero;

        private void Update()
        {
            m_rectTransform.anchoredPosition = Vector2.Lerp(m_rectTransform.anchoredPosition, m_targetPosition, m_lerpSpeed * Time.deltaTime);
        }

        public void UpdatePosition()
        {
            m_targetPosition = m_rectTransform.anchoredPosition;

            m_targetPosition.x =
                m_inventoryUIManager.GetInitialItemDistance()
                + m_rank * (m_inventoryUIManager.GetItemWidth() + m_inventoryUIManager.GetItemSpacing())
                - m_inventoryDataManager.GetIndexSelector() * (m_inventoryUIManager.GetItemWidth() + m_inventoryUIManager.GetItemSpacing());
        }

        public void UpdateStatus()
        {
            if(m_rank == m_inventoryDataManager.GetIndexSelector())
            {
                m_selectorArrows.SetActive(true);
            }
            else
            {
                m_selectorArrows.SetActive(false);
            }
        }

        #region Getter & Setter
        public int GetRank()
        {
            return m_rank;
        }

        public void SetRank(int rank)
        {
            m_rank = rank;
        }

        public void Initializer(RectTransform rectTransform, InventoryUIManager inventoryUIManager, InventoryDataManager inventoryDataManager)
        {
            m_rectTransform = rectTransform;
            m_inventoryDataManager = inventoryDataManager;
            m_inventoryUIManager = inventoryUIManager;
        }

        public void SetSprite(Sprite sprite)
        {
            GetComponent<Image>().sprite = sprite;
        }
        #endregion

    }
}
