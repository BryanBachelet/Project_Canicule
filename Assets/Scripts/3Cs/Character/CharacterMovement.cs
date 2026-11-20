using UnityEngine;
using UnityEngine.AI;

namespace Canicule
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float m_characterMovementSpeed = 2f;

        [Header("Debug Options")]
        [SerializeField] private bool m_deactiveNavMeshLimiter = false;

        private CharacterInput m_characterInput;
        private Rigidbody2D m_playerRigidbody2D;
        private Character m_character;

        private Vector2 m_moveDirection;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_characterInput = GetComponent<CharacterInput>();
            m_playerRigidbody2D = GetComponent<Rigidbody2D>();
            m_character = GetComponent<Character>();
        }

        // Update is called once per frame
        void Update()
        {
                if (m_character.GetCurrentState() == CharacterState.INVENTORY ||
                m_character.GetCurrentState() == CharacterState.INTERACTION)
            {
                m_playerRigidbody2D.linearVelocity = Vector2.zero;
                return;
            }

            ComputeCharacterMovementSpeed();

            m_moveDirection = m_characterInput.moveDirection.normalized * m_characterMovementSpeed;
            Move(m_moveDirection);
        }

        // Moves the character using its rigidbody2D
        public void Move(Vector2 dir)
        {
            m_playerRigidbody2D.linearVelocity = dir;

            if(dir != Vector2.zero && m_character.GetCurrentState() == CharacterState.IDLE)
            {
                m_character.ChangeState(CharacterState.WALKING);
            }
            else if (dir == Vector2.zero && m_character.GetCurrentState() == CharacterState.WALKING)
            {
                m_character.ChangeState(CharacterState.IDLE);
            }
        }

        private void ComputeCharacterMovementSpeed()
        {
            float start = m_characterMovementSpeed;
            float target = m_characterInput.characterData.characterMovementSpeedOverExhaustion.Evaluate(
                GameManager.GetExhaustionManager().exhaustionRatio);
            m_characterMovementSpeed = Mathf.Lerp(start, target, m_characterInput.characterData.movementSpeedLerpSpeed);
        }
    }
}
