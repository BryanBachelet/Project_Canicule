using UnityEngine;

namespace Canicule
{
    public class InteractibleObject : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public virtual void Interact()
        {
            Debug.Log("Interact");
        }

        public virtual bool CanInteract()
        {
            return true;
        }
    }
}
