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

        // Shoots a point at the mouse's position, and if it's an interactible, plays the Interact() function.
        public void Interact(Vector2 mousePosWorld)
        {
            Collider2D hit = Physics2D.OverlapPoint(mousePosWorld, LayerMask.GetMask("Interactible"));

            if (hit != null)
            {
                InteractibleObject interactibleObject = hit.GetComponent<InteractibleObject>();

                if (interactibleObject != null)
                {
                    if (interactibleObject.CanInteract())
                    {
                        interactibleObject.Interact();
                    }
                }
                else
                {
                    Debug.LogError("No Interactible Script was founded on " + interactibleObject.name);
                }
            }
        }
    }
}
