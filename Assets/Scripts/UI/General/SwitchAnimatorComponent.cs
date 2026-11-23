using UnityEngine;

namespace Canicule
{
    public class SwitchAnimatorComponent : MonoBehaviour
    {
        private Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void EnableAnimator()
        {
            if (m_animator != null)
            {
                m_animator.enabled = true;
            }
        }

        public void DisableAnimator()
        {
            if (m_animator != null)
            {
                m_animator .enabled = false;
            }
        }
    }
}
