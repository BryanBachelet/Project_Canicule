using System;
using System.Collections;
using UnityEngine;

namespace Canicule
{
    public class ItemEffectManager : MonoBehaviour
    {

        private float m_roomTemperatureModifierTimer = 0f;
        private bool b_isTemperatureTimerRunning = false;
        private float m_roomTemperatureDeltaBeforeTimer = 0f;

        private void Update()
        {
            // Room Temperature Timer
            RunRoomTemperatureTimer();
        }


        // Hunger
        public void ModifyHunger(float amount)
        {
            GameManager.GetExhaustionManager().ModifyHungerExhaustion(amount);
            Debug.Log("[Item Effect Manager] Hunger modified by " + amount + " !");
        }

        // Thirst
        public void ModifyThirst(float amount)
        {
            GameManager.GetExhaustionManager().ModifyThirstExhaustion(amount);
            Debug.Log("[Item Effect Manager] Thirst modified by " + amount + " !");
        }

        // Body Temperature
        public void ModifyRoomTemperatureModifier(float amount)
        {
            GameManager.GetExhaustionManager().ModifyRoomTemperatureModifier(amount);
            Debug.Log("[Item Effect Manager] Room Temperature Modifier modified by " + amount + " !");
        }

        public void ModifyRoomTemperatureModifierForXSeconds(float amount, float duration)
        {
            if (!b_isTemperatureTimerRunning)
            {
                ModifyRoomTemperatureModifier(amount);
                m_roomTemperatureDeltaBeforeTimer = amount;
            }

            m_roomTemperatureModifierTimer = duration;
            b_isTemperatureTimerRunning = true;
        }

        public void RunRoomTemperatureTimer()
        {
            if (b_isTemperatureTimerRunning)
            {
                m_roomTemperatureModifierTimer -= Time.deltaTime;

                if (m_roomTemperatureModifierTimer < 0f)
                {
                    ModifyRoomTemperatureModifier(-1 * m_roomTemperatureDeltaBeforeTimer);
                    Debug.Log("[Item Effect Manager] Room Temperature Timer ended !");
                    b_isTemperatureTimerRunning = false;
                }
            }
        }


        IEnumerator WaitAndRun(float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);

            onComplete?.Invoke();
        }

        
    }
}
