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

        #region Unity Methods
        private void Update()
        {
            // Room Temperature Timer
            RunRoomTemperatureTimer();
        }
        #endregion

        #region Sleep

        public void ModifySleepExhaustionValue(float amount)
        {
            GameManager.GetExhaustionManager().ModifySleepExhaustionValue(amount);
        }

        public void ModifySleepExhaustionValueAfterXSeconds(float amount, float delay)
        {
            StartCoroutine(WaitAndRun(delay, () => ModifySleepExhaustionValue(amount)));
        }
        public void ModifySleepChance(float amount)
        {
            GameManager.GetSleepManager().ModifySleepChanceModifier(amount);
        }

        public void ModifySleepChancePills(float amount)
        {
            GameManager.GetSleepManager().ModifySleepChancePillsModifier(amount);
            GameManager.GetSleepManager().SetHasTakenPills(true);
        }

        public void ModifySleepChanceWithoutPills(float amount)
        {
            GameManager.GetSleepManager().ModifySleepChanceWithoutPillsModifier(amount);
        }

        public void ModifySleepQuality(float amount)
        {
            GameManager.GetExhaustionManager().ModifySleepQuality(amount);
        }
        #endregion

        #region Hunger
        public void ModifyHunger(float amount)
        {
            GameManager.GetExhaustionManager().ModifyHungerExhaustion(amount);
            Debug.Log("[Item Effect Manager] Hunger modified by " + amount + " !");
        }
        #endregion

        #region Thirst
        public void ModifyThirst(float amount)
        {
            GameManager.GetExhaustionManager().ModifyThirstExhaustion(amount);
            Debug.Log("[Item Effect Manager] Thirst modified by " + amount + " !");
        }
        #endregion

        #region Body Temperature
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
        #endregion


        IEnumerator WaitAndRun(float delay, Action methodToRun)
        {
            yield return new WaitForSeconds(delay);

            methodToRun?.Invoke();
        }

        
    }
}
