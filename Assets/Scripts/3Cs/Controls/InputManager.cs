using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Canicule
{
    public class InputManager : MonoBehaviour
    {

        private const string m_playerInputMapName = "Player";
        private const string m_UIInputMapName = "UI";

        private void Start()
        {
            DisablePlayerMap();
            DisableUIMap();

            EnablePlayerMap();
        }

        public void EnablePlayerMap()
        {
            InputSystem.actions.FindActionMap(m_playerInputMapName).Enable();
        }

        public void DisablePlayerMap()
        {
            InputSystem.actions.FindActionMap(m_playerInputMapName).Disable();
        }

        public void EnableUIMap()
        {
            InputSystem.actions.FindActionMap(m_UIInputMapName).Enable();
        }

        public void DisableUIMap()
        {
            InputSystem.actions.FindActionMap(m_UIInputMapName).Disable();
        }

    }
}
