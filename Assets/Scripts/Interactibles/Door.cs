using UnityEngine;

namespace Canicule
{
    public class Door : InteractibleObject
    {
        private bool m_bIsDoorOpen = false;
        private Animator m_animator;

        private const string openStateNameAnimator = "Open";
        private const string closeStateNameAnimator = "Close";

        public override void Start()
        {
            base.Start();
            m_animator = GetComponent<Animator>();
        }

        protected override void Interact()
        {
            OpenDoor();

            base.Interact();
        }

        private void OpenDoor()
        {
            if (!m_bIsDoorOpen)
            {
                m_animator.SetBool(openStateNameAnimator, true);
                m_animator.SetBool(closeStateNameAnimator, false);
                m_bIsDoorOpen = true;
            }
            else
            {
                m_animator.SetBool(closeStateNameAnimator, true);
                m_animator.SetBool(openStateNameAnimator, false);
                m_bIsDoorOpen = false;
            }
        }

    }
}

