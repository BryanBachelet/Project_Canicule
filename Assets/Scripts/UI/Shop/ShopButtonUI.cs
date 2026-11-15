using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class ShopButtonUI : MonoBehaviour
    {
        private ShopDataManager m_shopManager;
        private Button m_button;

        #region Unity Methods
        private void Awake()
        {
            m_shopManager = GameManager.GetShopDataManager();
            m_button = GetComponent<Button>();
        }

        void Start()
        {
            m_button.onClick.AddListener(ShopOpened);
        }

        private void OnDestroy()
        {
            m_button.onClick.RemoveListener(ShopOpened);
        }
        #endregion

        public void ShopOpened()
        {
            m_shopManager.InvokeOnShopOpened();
        }
    }
}
