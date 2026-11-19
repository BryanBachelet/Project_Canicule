using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Canicule
{
    public class ItemEffectManager : MonoBehaviour
    {
        // Body Temp
        private float m_roomTemperatureModifierTimer = 0f;
        private bool b_isTemperatureTimerRunning = false;
        private float m_roomTemperatureDeltaBeforeTimer = 0f;

        // Work
        private float m_gainModifierTimer = 0f;
        private bool b_isGainTimerRunning = false;
        private float m_gainModifierDeltaBeforeTimer = 0f;

        // Survival Blankets
        private Window[] m_windows;

        #region Unity Methods
        private void Start()
        {
            m_windows = FindObjectsByType<Window>(FindObjectsSortMode.None);
        }
        private void Update()
        {
            // Room Temperature Timer
            RunRoomTemperatureTimer();
            RunGainTimer();
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

        public void ModifySleepChanceEnergyDrink(float amount)
        {
            GameManager.GetSleepManager().ModifySleepChanceEnergyDrink(amount);
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

        #region Work
        public void ModifyGainModifier(float amount)
        {
            GameManager.GetWorkDataManager().ModifyGainModifier(amount);
            Debug.Log("[Item Effect Manager] Gain modifier modified by " + amount + " !");
        }

        public void ModifyGainModifierForXSeconds(float amount, float duration)
        {
            if (!b_isGainTimerRunning)
            {
                ModifyGainModifier(amount);
                m_gainModifierDeltaBeforeTimer = amount;
            }

            m_gainModifierTimer = duration;
            b_isGainTimerRunning = true;
        }

        public void RunGainTimer()
        {
            if (b_isGainTimerRunning)
            {
                m_gainModifierTimer -= Time.deltaTime;

                if (m_gainModifierTimer < 0f)
                {
                    ModifyGainModifier(-1 * m_gainModifierDeltaBeforeTimer);
                    Debug.Log("[Item Effect Manager] Gain modifier timer ended !");
                    b_isGainTimerRunning = false;
                }
            }
        }
        #endregion

        #region Special
        public void InstallSurvivalBlankets(float reduction)
        {
            foreach(Window window in m_windows)
            {
                window.InstallSurvivalBlanket(reduction);
                Debug.Log("Survival Blankets installed for " + window.gameObject.name + " !");
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
