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
        [SerializeField] protected float interactionDuration = 1;
        protected float interactionTimer = 0.0f;
        
        protected Transform actorTransform;

        protected bool bIsInteracting = false;
        protected bool bIsAvailable = true;

        protected bool bIsOnPause = false;

        protected Interactor m_interactor;

        [HideInInspector] public UnityEvent OnInteractStart;
        [HideInInspector] public UnityEvent OnInteractEnd;

        [SerializeField] protected ItemData m_itemData;

        [Header("Outline Parameters")]
        [SerializeField] private Renderer m_rendererCmpt;
        [SerializeField] private float m_outlineThickness  = 1;
        [Space]
        private const string m_outlineActiveParameterStr = "_ActiveOutline";
        private const string m_outlineThicknecsParameterStr = "_Thickness";

        #region Unity Function

        public virtual void Start()
        {
            GameManager.RegisterObject(this);
            m_roomOwner = GetComponentInParent<Rooms.Room>();
            if (m_rendererCmpt)
            {
                m_rendererCmpt.material = new Material(m_rendererCmpt.material);
                    DeactiveOutline();
            }
            else
            {
                Debug.LogError($"Need to fill the variable m_renderCmpt");
            }

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

        public virtual void CancelInteration()
        {
            bIsInteracting = false;
            actorTransform = null;
            m_interactor = Interactor.NONE;
        }

        public bool CanInteract()
        {
            return !bIsInteracting && bIsAvailable;
        }

        public void SetAvailability(bool availableState)
        {
            bIsAvailable = availableState;
        }

        public virtual void CreateObject(ItemData data)
        {

        }


        public ItemData CreateItemInstance()
        {
            ItemData instance = UnityEngine.Object.Instantiate(m_itemData);
            instance = m_itemData;
            return instance;
        }
        public void RetrieveObject(ItemData instance)
        {
            if (!m_itemData || !instance) return;

            KeyValuePair<ItemData, int> pair = new KeyValuePair<ItemData, int>(instance, 1);
            GameManager.GetInventoryDataManager().AddNewItem(pair);
            DestroyInteractible();
        }

        protected virtual void DestroyInteractible()
        {
            OnInteractEnd.RemoveAllListeners();
            OnInteractStart.RemoveAllListeners();
            Destroy(gameObject);
        }

        #region Visual Functions
        public void ActiveOutline()
        {
            if (!m_rendererCmpt) return;
            
                m_rendererCmpt.material.SetFloat(m_outlineActiveParameterStr, 1);
                m_rendererCmpt.material.SetFloat(m_outlineThicknecsParameterStr, m_outlineThickness);
        }

        public void DeactiveOutline()
        {
            if (!m_rendererCmpt) return;    
            m_rendererCmpt.material.SetFloat(m_outlineActiveParameterStr, 0);
        }

        #endregion


        #region Getter Function
        public bool IsInteracting() 
        {
            return bIsInteracting; 
        }

        public float GetRatioInteraction()
        {
            return interactionTimer / interactionDuration;
        }

     
        #endregion
    }
}
