using System;
using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class InteractibleObject : MonoBehaviour, IPauseableObject
    {
        [SerializeField] protected float p_interactionRange = 1;
        [SerializeField] protected float p_interactionDuration = 5;
        protected float p_interactionTimer = 0.0f;
        
        protected Transform p_actorTransform;

        protected bool p_bIsInteracting = false;

        protected bool p_bIsOnPause = false;

        [HideInInspector] public UnityEvent OnInteractStart;

        #region Unity Function

        public void Start()
        {
            GameManager.RegisterObject(this);
        }

        public void OnDestroy()
        {
            GameManager.UnregisterObject(this);
        }
        // Update is called once per frame
        void Update()
        {
            UpdateInteraction();
        }

        #endregion

        #region Pause Functions
        public  void OnPause()
        {
            p_bIsOnPause = true;
        }
        public void OnPlay()
        {
            p_bIsOnPause = false;
        }
        #endregion

        /// <summary>
        /// Update during the interaction;
        /// </summary>
        protected virtual void UpdateInteraction()
        {
            if (!p_bIsInteracting || p_bIsOnPause) return;

            if(p_interactionTimer < p_interactionDuration)
            {
                p_interactionTimer += Time.deltaTime;
            }
            else
            {
                Interact();
            }
        }

        /// <summary>
        /// Launch the interaction and can be call only one time per interaction
        /// </summary>
        public virtual void LaunchInteraction(Transform actor)
        {
            if (p_bIsInteracting) return;

            p_bIsInteracting = true;

            p_interactionTimer = 0.0f;
            p_actorTransform = actor;

            OnInteractStart?.Invoke();
        }

        protected virtual void Interact()
        {
            p_bIsInteracting = false;
            p_actorTransform = null;
        }

        public bool CanInteract()
        {
            return !p_bIsInteracting;
        }

        #region Getter Function
        public bool IsInteracting() 
        {
            return p_bIsInteracting; 
        }
        #endregion
    }
}
