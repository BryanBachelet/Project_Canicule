using System;
using UnityEngine;

namespace Canicule
{
    public class InteractibleObject : MonoBehaviour, IPauseableObject
    {
        [SerializeField] protected float p_interactionRange = 1;
        [SerializeField] protected float p_interactionDuration = 5;

        protected Transform m_playerTransform;
        protected bool p_bIsInteracting = false;

        protected bool p_bIsOnPause = false;

        #region Unity Function

        public void Start()
        {
            GameManager.RegisterObject(this);

           // m_playerTransform =
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

        }

        /// <summary>
        /// Launch the interaction and can be call only one time per interaction
        /// </summary>
        public virtual void Interact()
        {
            if (!p_bIsInteracting) return;

            p_bIsInteracting = true;
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
