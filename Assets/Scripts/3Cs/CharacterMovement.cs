using UnityEngine;

namespace Canicule
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float m_characterMovementSpeed = 2f;
        private CharacterInput m_characterInput;
        private Vector2 m_moveDirection;
        private Rigidbody2D m_playerRigidbody2D;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_characterInput = GetComponent<CharacterInput>();
            m_playerRigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            m_moveDirection = m_characterInput.moveDirection.normalized * m_characterMovementSpeed;

            Move(m_moveDirection);
        }

        // Moves the character using its rigidbody2D
        public void Move(Vector2 dir)
        {
            m_playerRigidbody2D.linearVelocity = dir;
        }
    }
}
