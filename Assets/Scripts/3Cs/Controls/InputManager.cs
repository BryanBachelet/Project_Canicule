using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class InputManager : MonoBehaviour
    {
        private void Start()
        {
            DisablePlayerMap();
            DisableUIMap();

            EnablePlayerMap();
        }

        public void EnablePlayerMap()
        {
            InputSystem.actions.FindActionMap("Player").Enable();
        }

        public void DisablePlayerMap()
        {
            InputSystem.actions.FindActionMap("Player").Disable();
        }

        public void EnableUIMap()
        {
            InputSystem.actions.FindActionMap("UI").Enable();
        }

        public void DisableUIMap()
        {
            InputSystem.actions.FindActionMap("UI").Disable();
        }

    }
}
