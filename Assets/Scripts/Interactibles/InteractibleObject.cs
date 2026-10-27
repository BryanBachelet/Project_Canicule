using System;
using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class InteractibleObject : MonoBehaviour, IPauseableObject
    {
        [SerializeField] protected float interactionRange = 1;
        [SerializeField] protected float interactionDuration = 5;
        protected float interactionTimer = 0.0f;
        
        protected Transform actorTransform;

        protected bool bIsInteracting = false;

        protected bool bIsOnPause = false;

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
            bIsOnPause = true;
        }
        public void OnPlay()
        {
            bIsOnPause = false;
        }
        #endregion

        /// <summary>
        /// Update during the interaction;
        /// </summary>
        protected virtual void UpdateInteraction()
        {
            if (!bIsInteracting || bIsOnPause) return;

            if(interactionTimer < interactionDuration)
            {
                interactionTimer += Time.deltaTime;
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
            if (bIsInteracting) return;

            bIsInteracting = true;

            interactionTimer = 0.0f;
            actorTransform = actor;

            OnInteractStart?.Invoke();
        }

        protected virtual void Interact()
        {
            bIsInteracting = false;
            actorTransform = null;
        }

        public bool CanInteract()
        {
            return !bIsInteracting;
        }

        #region Getter Function
        public bool IsInteracting() 
        {
            return bIsInteracting; 
        }
        #endregion
    }
}
