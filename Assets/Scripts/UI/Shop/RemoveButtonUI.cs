using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class RemoveButtonUI : MonoBehaviour
    {
        [SerializeField] private CartCardUpdater m_cartCardUpdater;
        private ShopDataManager m_shopManager;
        private Button m_button;

        #region Unity Methods
        private void Awake()
        {
            m_shopManager = FindAnyObjectByType<ShopDataManager>();
            m_button = GetComponent<Button>();
        }

        void Start()
        {
            m_button.onClick.AddListener(RemoveItemToCart);
        }

        private void OnDestroy()
        {
            m_button.onClick.RemoveListener(RemoveItemToCart);
        }
        #endregion

        public void RemoveItemToCart()
        {
            m_shopManager.RemoveItemFromCart(m_cartCardUpdater.GetStoredItem());
        }
    }
}
