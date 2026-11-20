using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    [System.Serializable] 
    
    public class TimerObject 
    {
        public float duration = 0.0f;
        private float m_timer = 0.0f;
        [Space]
        public UnityEvent OnTimerCycleFinish;
        public float ratio { get { return m_timer / duration; } }
        public TimerObject(float duration)
        {
            this.duration = duration;
        }

        ~TimerObject()
        {
            OnTimerCycleFinish.RemoveAllListeners();
        }

        public bool UpdateTimer()
        {
            if(m_timer > duration)
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
            if (m_timer > duration)
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
