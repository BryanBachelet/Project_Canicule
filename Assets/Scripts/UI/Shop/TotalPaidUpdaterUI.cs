using TMPro;
using UnityEngine;

namespace Canicule
{
    public class TotalPaidUpdaterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_totalPaid;
        [SerializeField] private string m_currency;
        private ShopDataManager m_shopManager;

        #region Unity Methods
        private void Awake()
        {
            m_shopManager = FindAnyObjectByType<ShopDataManager>();
        }

        private void Start()
        {
            UpdateTotalPaid();
        }

        private void OnEnable()
        {
            m_shopManager.OnItemCartAdded += _ => UpdateTotalPaid();
            m_shopManager.OnItemCartRemoved += _ => UpdateTotalPaid();
            m_shopManager.OnItemCartUpdated += (_, _) => UpdateTotalPaid();
            m_shopManager.OnCartItemsMoved += UpdateTotalPaid;
        }

        private void OnDisable()
        {
            m_shopManager.OnItemCartAdded -= _ => UpdateTotalPaid();
            m_shopManager.OnItemCartRemoved -= _ => UpdateTotalPaid();
            m_shopManager.OnItemCartUpdated -= (_, _) => UpdateTotalPaid();
            m_shopManager.OnCartItemsMoved -= UpdateTotalPaid;
        }
        #endregion

        private void UpdateTotalPaid()
        {
            m_totalPaid.text = m_shopManager.ComputeCartTotalPrice().ToString() + m_currency;
        }
    }
}
