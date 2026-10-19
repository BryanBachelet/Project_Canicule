using UnityEngine;

namespace Canicule
{
    public class CharacterInteract : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Interact(Vector2 mousePosWorld)
        {
            Collider2D hit = Physics2D.OverlapPoint(mousePosWorld, LayerMask.GetMask("Interactible"));
            if (hit != null)
            {
                Debug.Log("Interact");
            }
        }
    }
}
