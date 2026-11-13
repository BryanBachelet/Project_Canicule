using System;
using System.Collections.Generic;
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
        protected Rooms.Room m_roomOwner;
        [Space]
        [SerializeField] protected float interactionRange = 1;
        [SerializeField] protected float interactionDuration = 5;
        protected float interactionTimer = 0.0f;
        
        protected Transform actorTransform;

        protected bool bIsInteracting = false;

        protected bool bIsOnPause = false;

        protected Interactor m_interactor;

        [HideInInspector] public UnityEvent OnInteractStart;
        [HideInInspector] public UnityEvent OnInteractEnd;

        [SerializeField] protected ItemData m_itemData;

        #region Unity Function

        public virtual void Start()
        {
            GameManager.RegisterObject(this);
            m_roomOwner = GetComponentInParent<Rooms.Room>();
        }

        public virtual void OnDestroy()
        {
            GameManager.UnregisterObject(this);
        }
        // Update is called once per frame
        public virtual void Update()
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

            m_interactor = interactActorType;
        }

        protected virtual void Interact()
        {
            bIsInteracting = false;
            actorTransform = null;
            m_interactor = Interactor.NONE;
            OnInteractEnd?.Invoke();
        }

        public bool CanInteract()
        {
            return !bIsInteracting;
        }

        public virtual void CreateObject(ItemData data)
        {

        }

        public void RetrieveObject()
        {
            if (!m_itemData) return;

            KeyValuePair<ItemData, int> pair = new KeyValuePair<ItemData, int>(m_itemData, 1);
            GameManager.GetInventoryDataManager().AddNewItem(pair);
            DestroyInteractible();
        }

        protected virtual void DestroyInteractible()
        {
            OnInteractEnd.RemoveAllListeners();
            OnInteractStart.RemoveAllListeners();
            Destroy(gameObject);
        }


        #region Getter Function
        public bool IsInteracting() 
        {
            return bIsInteracting; 
        }

     
        #endregion
    }
}
