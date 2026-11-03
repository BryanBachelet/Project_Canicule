using System;
using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public enum Interactor
    {
        NONE = 0,
        PLAYER = 1,
        MONSTER =2,
    }

    public class InteractibleObject : MonoBehaviour, IPauseableObject
    {
        [SerializeField] protected Rooms.Room m_roomOwner;
        [Space]
        [SerializeField] protected float interactionRange = 1;
        [SerializeField] protected float interactionDuration = 5;
        protected float interactionTimer = 0.0f;
        
        protected Transform actorTransform;

        protected bool bIsInteracting = false;

        protected bool bIsOnPause = false;

        protected Interactor interactor;

        [HideInInspector] public UnityEvent OnInteractStart;

        #region Unity Function

        public virtual void Start()
        {
            GameManager.RegisterObject(this);
            m_roomOwner = GetComponentInParent<Rooms.Room>();
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

        public void OnRegister()
        {
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
        public virtual void LaunchInteraction(Transform actor, Interactor interactActorType)
        {
            if (bIsInteracting) return;

            bIsInteracting = true;

            interactionTimer = 0.0f;
            actorTransform = actor;

            OnInteractStart?.Invoke();

            interactor = interactActorType;
        }

        protected virtual void Interact()
        {
            bIsInteracting = false;
            actorTransform = null;
            interactor = Interactor.NONE;
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
