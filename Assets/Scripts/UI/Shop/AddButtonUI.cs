using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class AddButtonUI : MonoBehaviour
    {
        [SerializeField] private ItemCardUpdater m_itemCardUpdater;
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
            m_button.onClick.AddListener(AddItemToCart);
        }

        private void OnDestroy()
        {
            m_button.onClick.RemoveListener(AddItemToCart);
        }
        #endregion

        public void AddItemToCart()
        {
            m_shopManager.AddItemInCart(m_itemCardUpdater.GetStoredItem());
        }

    }
}
