using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class TimerObject 
    {
        private float m_duration = 0.0f;
        private float m_timer = 0.0f;
        public UnityEvent OnTimerCycleFinish;
        public float ratio { get { return m_timer / m_duration; } }
        public TimerObject(float duration)
        {
            m_duration = duration;
        }

        ~TimerObject()
        {
            OnTimerCycleFinish.RemoveAllListeners();
        }

        public bool UpdateTimer()
        {
            if(m_timer > m_duration)
            {
                OnTimerCycleFinish?.Invoke();
                m_timer = 0.0f;
                return true;
            }
            else
            {
                m_timer += Time.deltaTime;
                return false;
            }
        }
        
        public bool UpdateTimer(float deltaTime)
        {
            if (m_timer > m_duration)
            {
                OnTimerCycleFinish?.Invoke();
                m_timer = 0.0f;
                return true;
            }
            else
            {
                m_timer += deltaTime;
                return false;
            }
        }

        
    }
}
