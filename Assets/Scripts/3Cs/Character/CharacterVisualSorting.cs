using UnityEngine;

namespace Canicule
{
    public class CharacterVisualSorting : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_playerSpriteRenderer;
        private int m_previousPlayerSortOrder;

        private void Start()
        {
            m_previousPlayerSortOrder = m_playerSpriteRenderer.sortingOrder;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpriteRenderer collisionSpriteRenderer = collision.GetComponent<SpriteRenderer>();

            if (collisionSpriteRenderer != null)
            {
                if(m_playerSpriteRenderer.sortingOrder <= collisionSpriteRenderer.sortingOrder)
                {
                    m_previousPlayerSortOrder = m_playerSpriteRenderer.sortingOrder;
                    m_playerSpriteRenderer.sortingOrder = collisionSpriteRenderer.sortingOrder + 1;
                }
            }
            else
            {
                Debug.LogWarning("No sprite renderer was found on " + collision.gameObject.name);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(m_playerSpriteRenderer.sortingOrder != m_previousPlayerSortOrder)
            {
                m_playerSpriteRenderer.sortingOrder = m_previousPlayerSortOrder;
            }
        }
    }
}
