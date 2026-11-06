using UnityEngine;
using UnityEngine.UI;

namespace Canicule
{
    public class BuyAllButton : MonoBehaviour
    {
        private ShopDataManager m_shopDataManager;
        private Button m_button;

        private void Awake()
        {
            m_shopDataManager = GameManager.GetShopDataManager();
            m_button = GetComponent<Button>();
        }
        private void Start()
        {
            m_button.onClick.AddListener(FireMoveCartItemsToInventory);
        }

        private void OnDestroy()
        {
            if(m_button != null)
            {
                m_button.onClick.RemoveListener(FireMoveCartItemsToInventory);
            }
        }

        private void FireMoveCartItemsToInventory()
        {
            m_shopDataManager.MoveCartItemsToInventory();
        }
    }
}
