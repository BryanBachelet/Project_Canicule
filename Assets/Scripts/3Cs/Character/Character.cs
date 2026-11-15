using UnityEngine;

namespace Canicule
{
    public enum CharacterState
    {
        NONE = 0,
        IDLE = 1,
        WALKING = 2,
        SLEEPING = 3,
        INTERACTION = 4,
        INVENTORY = 5,
    }

    public class Character : MonoBehaviour
    {
        private CharacterState m_currentState;
        [SerializeField] private bool b_isDebugActive;

        private void Start()
        {
            m_currentState = CharacterState.NONE;
        }

        public void ChangeState(CharacterState state)
        {
            if (b_isDebugActive)
            {
                Debug.Log("[Character] Change character state from " + m_currentState + " to " + state);
            }

            m_currentState = state;
        }

        public CharacterState GetCurrentState()
        {
            return m_currentState;
        }
    }
}
