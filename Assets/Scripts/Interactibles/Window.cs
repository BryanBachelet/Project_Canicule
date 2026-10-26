using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class Window : InteractibleObject
    {
        public UnityEvent OnWindowOpen;
        public UnityEvent OnWindowClose;

        private bool m_isWindowClose = true;

        public override void LaunchInteraction(Transform actor)
        {
            base.LaunchInteraction(actor);
            
            m_isWindowClose = !m_isWindowClose; 
            WindowInteraction();
        }

        private void WindowInteraction()
        {
            if(m_isWindowClose)
            {
                OnWindowClose?.Invoke();
            }
            else
            {
                OnWindowOpen?.Invoke();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            Monster monsterComp = other.GetComponent<Monster>();
            if (monsterComp && !m_isWindowClose)
            {
                monsterComp.ChangeState(MonsterState.DISAPPEAR);
            }
        }
    }
}
