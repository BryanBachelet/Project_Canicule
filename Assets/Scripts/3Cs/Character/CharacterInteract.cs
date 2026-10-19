using UnityEngine;

namespace Canicule
{
    public class CharacterInteract : MonoBehaviour
    {
        [SerializeField] private float m_interactionDistance = 2f;

        // Shoots a point at the mouse's position, and if it's an interactible in the player's range, plays the Interact() function.
        public void Interact(Vector2 mousePosWorld)
        {
            Collider2D hit = Physics2D.OverlapPoint(mousePosWorld, LayerMask.GetMask("Interactible"));

            if (hit != null)
            {
                InteractibleObject interactibleObject = hit.GetComponent<InteractibleObject>();

                if (interactibleObject != null)
                {
                    // Check distance between player and object
                    Vector2 playerPos;
                    playerPos.x = transform.position.x;
                    playerPos.y = transform.position.y;
                    Vector2 direction = mousePosWorld - playerPos;
                    Vector2 normalizedDirection = direction.normalized;
                    RaycastHit2D playerInteractibleRay = Physics2D.Raycast(playerPos, normalizedDirection, m_interactionDistance, LayerMask.GetMask("Interactible"));
                    
                    if (interactibleObject.CanInteract() && playerInteractibleRay)
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
