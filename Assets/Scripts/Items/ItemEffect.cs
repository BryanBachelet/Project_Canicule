using System;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Canicule
{
    // Defining the concept of ItemEffect
    [System.Serializable]
    public abstract class ItemEffect
    {
        public abstract void UseItemEffect();
    }

    [System.Serializable]
    public class InteractableEffect : ItemEffect
    {
        public GameObject m_interactableObjectPrefab;
        public override void UseItemEffect()
        {
            Debug.Log("[Item Effect] Interactable Effect has been used!");
        }
    }

    #region Sleep Effects
    // Sleep Chance effect
    [System.Serializable]
    public class ModifySleepChance : ItemEffect
    {
        [SerializeField] private float m_amount;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifySleepChance(m_amount);
        }
    }

    // Sleep Chance associated with pills
    [System.Serializable]
    public class ModifySleepChancePills : ItemEffect
    {
        [SerializeField] private float m_amount;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifySleepChancePills(m_amount);
        }
    }

    
    // Sleep Chance Without Pills effect
    [System.Serializable]
    public class ModifySleepChanceWithoutPills : ItemEffect
    {
        [SerializeField] private float m_amount;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifySleepChanceWithoutPills(m_amount);
        }
    }

    [System.Serializable]
    public class ModifySleepChanceEnergyDrink : ItemEffect
    {
        [SerializeField] private float m_amount;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifySleepChanceEnergyDrink(m_amount);
        }
    }

    // Sleep Quality effect
    [System.Serializable]
    public class ModifySleepQuality : ItemEffect
    {
        [SerializeField] private float m_amount;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifySleepQuality(m_amount);
        }
    }

    // Sleep Amount After X seconds effect
    [System.Serializable]
    public class ModifySleepAmountAfterXSeconds : ItemEffect
    {
        [SerializeField] private float m_amount;
        [SerializeField] private float m_delay;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifySleepExhaustionValueAfterXSeconds(m_amount, m_delay);
        }
    }
    #endregion

    #region Hunger Effects
    // Hunger effect
    [System.Serializable]
    public class ModifyHunger : ItemEffect
    {
        [SerializeField] private float m_hungerModifierAmount;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifyHunger(m_hungerModifierAmount);
        }

        public float GetHungerModifierAmount()
        {
            return m_hungerModifierAmount;
        }
    }
    #endregion

    #region Thirst Effects
    // Thirst effect
    [System.Serializable]
    public class ModifyThirst : ItemEffect
    {
        [SerializeField] private float m_thirstModifierAmount;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifyThirst(m_thirstModifierAmount);
        }

        public float GetThirstModifierAmount()
        {
            return m_thirstModifierAmount;
        }
    }
    #endregion

    #region Temperature Effects
    // Room Temperature effect
    [System.Serializable]
    public class ModifyRoomTemperature : ItemEffect
    {
        [SerializeField] private float m_roomTemperatureModifierAmount;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifyRoomTemperatureModifier(m_roomTemperatureModifierAmount);
        }
    }

    // Body Temperature During X Seconds effect
    [System.Serializable]
    public class ModifyBodyTemperatureDuringXSeconds : ItemEffect
    {
        [SerializeField] private float m_amount;
        [SerializeField] private float m_duration;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifyRoomTemperatureModifierForXSeconds(m_amount, m_duration);
        }
    }

    [System.Serializable]
    public class WaterBowlEffect : InteractableEffect
    {
        public bool m_isFill;
        [HideInInspector] public float m_fillDuration;
        public override void UseItemEffect()
        {
            base.UseItemEffect();
            Debug.Log("[Item Effect] Interactable Effect has been used!");
        }
    }
    #endregion

    #region Money Effects
    // Money Generated During X Seconds effect
    [System.Serializable]
    public class ModifyMoneyGeneratedDuringXSeconds : ItemEffect
    {
        [SerializeField] private float m_amount;
        [SerializeField] private float m_duration;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ModifyGainModifierForXSeconds(m_amount, m_duration);
        }
    }
    #endregion

    #region Special Effects
    // Install Survival Blanket effect
    [System.Serializable]
    public class InstallSurvivalBlanket : ItemEffect
    {
        [SerializeField] private float m_reduction;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().InstallSurvivalBlankets(m_reduction);
        }
    }

    // Activate Portable Fan effect
    [System.Serializable]
    public class ActivatePortableFan : ItemEffect
    {
        public override void UseItemEffect()
        {
            Debug.Log("[Item Effect] Activate Portable Fan Effect Used!");
        }
    }

    // Activate Standing Fan effect
    [System.Serializable]
    public class ActivateStandingFan : ItemEffect
    {
        [SerializeField] private float m_sleepChanceIncrease;
        [SerializeField] private float m_sleepQualityDecrease;

        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ActivateStandingFan(m_sleepChanceIncrease, m_sleepQualityDecrease);
        }
    }

    // Active Towel effect
    [System.Serializable]
    public class ActivateTowel : ItemEffect
    {
        public override void UseItemEffect()
        {
            Debug.Log("[Item Effect] Activate Towel Effect Used!");
        }
    }

    [System.Serializable]
    public class ActivateThermometer : ItemEffect
    {
        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ActivateThermoter();
        }
    }

    [System.Serializable]
    public class ActivateWatch : ItemEffect
    {
        public override void UseItemEffect()
        {
            GameManager.GetItemEffectManager().ActivateWatch();
        }
    }

    // End Game effect
    [System.Serializable]
    public class EndGame : ItemEffect
    {
        public override void UseItemEffect()
        {
            Debug.Log("[Item Effect] End Game Effect Used!");
        }
    }
    #endregion

}
