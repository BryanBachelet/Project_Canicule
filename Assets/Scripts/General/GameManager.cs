using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Canicule
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance;
        private List<IPauseableObject> m_pauseableObject;

        public UnityEvent OnPause;
        public UnityEvent OnPlay;

        public void PauseGame()
        {
            for (int i = 0; i < m_pauseableObject.Count; i++)
            {
                m_pauseableObject[i].OnPause();
            }

            OnPause?.Invoke();
        }

        public void PlayGame()
        {
            for (int i = 0; i < m_pauseableObject.Count; i++)
            {
                m_pauseableObject[i].OnPlay();
            }
            OnPlay?.Invoke();
        }


        #region Static Functions
        public static void RegisterObject(IPauseableObject obj)
        {
            if (m_instance == null)
                return;

            m_instance.m_pauseableObject.Add(obj);
            obj.OnRegister();
        }

        public static void UnregisterObject(IPauseableObject obj)
        {
            if (m_instance == null)
                return;

            m_instance.m_pauseableObject.Remove(obj);
        }

        public static GameManager GetGameManager()
        {
            if (m_instance == null)
                return null;

            return m_instance;
        }

        #endregion

    }
}
