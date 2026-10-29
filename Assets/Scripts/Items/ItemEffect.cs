using System;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Canicule
{
    // Defining the concept of ItemEffect
    [System.Serializable]
    public abstract class ItemEffect
    {
        public abstract void UseItemEffect(GameObject reference);
    }

    #region Sleep Effects
    // Sleep Chance effect
    [System.Serializable]
    public class ModifySleepChance : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Sleep Chance Effect Used!");
        }
    }

    // Sleep Chance Without Pills effect
    [System.Serializable]
    public class ModifySleepChanceWithoutPills : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Sleep Chance Without Pills Effect Used!");
        }
    }

    // Sleep Quality effect
    [System.Serializable]
    public class ModifySleepQuality : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Sleep Quality Effect Used!");
        }
    }

    // Sleep Amount After X seconds effect
    [System.Serializable]
    public class ModifySleepAmountAfterXSeconds : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Sleep Amount Effect Used!");
        }
    }
    #endregion

    #region Hunger Effects
    // Hunger effect
    [System.Serializable]
    public class ModifyHunger : ItemEffect
    {
        [SerializeField] private float m_hungerModifierAmount;

        public override void UseItemEffect(GameObject reference)
        {
            if(reference.TryGetComponent<ExhaustionManager>(out ExhaustionManager exhaustionManager))
            {
                exhaustionManager.ModifyHungerExhaustion(m_hungerModifierAmount);

                Debug.Log("[Item Effect] Hunger modified by " + m_hungerModifierAmount + " through item use!");
            }
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

        public override void UseItemEffect(GameObject reference)
        {
            if(reference.TryGetComponent<ExhaustionManager>(out ExhaustionManager exhaustionManager))
            {
                exhaustionManager.ModifyThirstExhaustion(m_thirstModifierAmount);
            }

            Debug.Log("[Item Effect] Thirst modified by " + m_thirstModifierAmount + " through item use!");
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
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Room Temperature Effect Used!");
        }
    }

    // Body Temperature During X Seconds effect
    [System.Serializable]
    public class ModifyBodyTemperatureDuringXSeconds : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Body Temperature During X Seconds Effect Used!");
        }
    }
    #endregion

    #region Money Effects
    // Money Generated During X Seconds effect
    [System.Serializable]
    public class ModifyMoneyGeneratedDuringXSeconds : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Money Generated Effect Used!");
        }
    }
    #endregion

    #region Special Effects
    // Install Survival Blanket effect
    [System.Serializable]
    public class InstallSurvivalBlanket : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Install Survival Blanket Effect Used!");
        }
    }

    // Activate Portable Fan effect
    [System.Serializable]
    public class ActivatePortableFan : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Activate Portable Fan Effect Used!");
        }
    }

    // Activate Standing Fan effect
    [System.Serializable]
    public class ActivateStandingFan : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Activate Standing Fan Effect Used!");
        }
    }

    // Active Towel effect
    [System.Serializable]
    public class ActivateTowel : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] Activate Towel Effect Used!");
        }
    }

    // End Game effect
    [System.Serializable]
    public class EndGame : ItemEffect
    {
        public override void UseItemEffect(GameObject reference)
        {
            Debug.Log("[Item Effect] End Game Effect Used!");
        }
    }
    #endregion

}
