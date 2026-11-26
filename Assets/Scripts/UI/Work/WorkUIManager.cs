using System.Linq;
using TMPro;
using UnityEngine;

namespace Canicule
{
    public class WorkUIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_balanceText;
        [SerializeField] private string m_currency;
        [SerializeField] private GameObject m_moneyAppOnlineState;
        [SerializeField] private GameObject m_moneyAppOfflineState;
        [SerializeField] private GameObject[] m_shapesPool;
        [SerializeField] private GameObject shapeHolder;
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private RectTransform m_spawnZoneRect;
        [SerializeField] private RectTransform m_workAppRect;
        private float m_offset = 120f;

        private WorkDataManager m_workDataManager;
        private NightDayManager m_nightDayManager;

        private GameObject m_shapeCurrentlyHold;

        #region Unity Methods
        private void Awake()
        {
            m_workDataManager = GameManager.GetWorkDataManager();
            m_nightDayManager = GameManager.GetNightDayManager();
        }

        private void OnEnable()
        {
            m_workDataManager.OnMoneyGained += UpdateBalance;

            UpdateBalance();

            if (m_nightDayManager.GetIsNightActive())
            {
                MakeAppOnline();
            }
            else
            {
                MakeAppOffline();
            }
        }

        private void OnDisable()
        {
            m_workDataManager.OnMoneyGained -= UpdateBalance;
        }
        #endregion

        #region UI Management
        public void UpdateBalance()
        {
            m_balanceText.text = m_workDataManager.GetPlayerBalance().ToString("F2") + m_currency;
        }

        private void MakeAppOnline()
        {
            m_moneyAppOnlineState.SetActive(true);
            m_moneyAppOfflineState.SetActive(false);
        }

        private void MakeAppOffline()
        {
            m_moneyAppOnlineState.SetActive(false);
            m_moneyAppOfflineState.SetActive(true);
        }

        public void SpawnShape()
        {
            GameObject newShape = GameObject.Instantiate(m_shapesPool[Random.Range(0, m_shapesPool.Count())], shapeHolder.transform);

            Vector2 randomVector = new Vector2(
                Random.Range(-1 * (m_spawnZoneRect.rect.width / 2) + m_offset, m_spawnZoneRect.rect.width / 2 - m_offset),
                Random.Range(-1 * (m_spawnZoneRect.rect.height / 2) + m_offset, m_spawnZoneRect.rect.height / 2 - m_offset));

            newShape.GetComponent<ShapeBehavior>().InitializeShape(this, m_canvas, m_spawnZoneRect.anchoredPosition + randomVector, m_workAppRect);
        }
        #endregion

        #region Getter & Setter
        public void SetShapeCurrentlyHold(GameObject shape)
        {
            m_shapeCurrentlyHold = shape;
        }

        public GameObject GetShapeCurrentlyHold()
        {
            return m_shapeCurrentlyHold;
        }
        #endregion
    }
}
