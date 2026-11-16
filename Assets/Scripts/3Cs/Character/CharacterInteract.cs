using UnityEngine;

namespace Canicule
{
    public class CharacterInteract : MonoBehaviour
    {
        [Header("Cursor Variables")]
        [SerializeField] private Texture2D m_baseCursorTex;
        [SerializeField] private Texture2D m_interactCursorTex;
        [SerializeField] private Texture2D m_cancelInteractionCursor;
        [SerializeField] private float m_cancelDuration = 2.0f;
        private bool m_cancelCursorActive;
        private float m_cancelTimer = 0.0f;


        private CharacterInput m_characterInput;

        private void Awake()
        {
            m_characterInput = GetComponent<CharacterInput>();
        }

        public void Update()
        {
            UpdateCursor();
        }


        private void UpdateCursor()
        {
            if(m_cancelCursorActive)
            {
                if (m_cancelTimer > m_cancelDuration)
                {
                    m_cancelCursorActive = false;
                }
                else
                {
                    m_cancelTimer += Time.deltaTime;
                    return;
                }

            }

            bool bIsInteracting = CheckMouseOver();
            if (bIsInteracting)
            {
                Cursor.SetCursor(m_interactCursorTex, Vector2.zero, CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.SetCursor(m_baseCursorTex, Vector2.zero, CursorMode.ForceSoftware);
            } 

        }

        private void SetCancelCursor()
        {
            m_cancelCursorActive = true;
            m_cancelTimer = 0.0f;
            Cursor.SetCursor(m_cancelInteractionCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        public bool CheckMouseOver()
        {
            Collider2D hit = Physics2D.OverlapPoint(m_characterInput.GetMousePosition(), LayerMask.GetMask("Interactible"));

            if (hit != null)
            {
                InteractibleObject interactibleObject = hit.GetComponent<InteractibleObject>();
                if (interactibleObject != null)
                {
                    return true;
                }
            }

            return false;
        }

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
                    RaycastHit2D playerInteractibleRay = Physics2D.Raycast(playerPos, normalizedDirection, m_characterInput.characterData.interactionRange, LayerMask.GetMask("Interactible"));

                    if (interactibleObject.CanInteract() && playerInteractibleRay)
                    {
                        interactibleObject.LaunchInteraction(this.transform, Interactor.PLAYER);
                    }
                    else
                    {
                        SetCancelCursor();
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
