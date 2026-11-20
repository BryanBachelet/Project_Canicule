using UnityEngine;

namespace Canicule
{
    public class CharacterInteract : MonoBehaviour
    {
        private bool m_isInteracting = false;
        private Character m_characterCmpt = null;
        private InteractibleObject m_lastInteractibleObject = null;
        private PlayerUIManager m_playerUIManager;

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
            m_characterCmpt = GetComponent<Character>();
            m_playerUIManager = GetComponentInChildren<PlayerUIManager>();
        }

        public void Update()
        {
            UpdateCursor();
            UpdateInteraction();
        }

        private void UpdateInteraction()
        {
            if(!m_isInteracting)
            {
                return;
            }

            m_playerUIManager.SetInteractionBar(m_lastInteractibleObject.GetRatioInteraction());
        }


        private void UpdateCursor()
        {
            if (m_cancelCursorActive)
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
                Debug.Log(hit.gameObject.name);

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
                        m_lastInteractibleObject = interactibleObject;
                        m_lastInteractibleObject.LaunchInteraction(this.transform, Interactor.PLAYER);
                        m_lastInteractibleObject.OnInteractEnd.AddListener(OnInteractionEnd);
                        m_isInteracting = true;
                        m_characterCmpt.ChangeState(CharacterState.INTERACTION);

                        m_playerUIManager.ActiveInteractionBar();
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

        public void CancelInteraction()
        {
            m_isInteracting = true;
            m_lastInteractibleObject.CancelInteration();
            OnInteractionEnd();
        }
       
        private void OnInteractionEnd()
        {
            m_isInteracting = false;
            m_characterCmpt.ChangeState(CharacterState.IDLE);
            m_lastInteractibleObject.OnInteractEnd.RemoveListener(OnInteractionEnd);
            m_playerUIManager.DeactivateInteractionBar();
        }

        public bool IsInteracting()
        {
            return m_isInteracting;
        }
    }
}
