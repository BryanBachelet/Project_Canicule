using UnityEngine;

namespace Canicule
{
    // Defining the concept of ItemEffect
    [System.Serializable]
    public abstract class ItemEffect
    {
        public abstract void UseItemEffect(GameObject reference);
    }

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
            }
        }

        public float GetHungerModifierAmount()
        {
            return m_hungerModifierAmount;
        }
    }

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
        }

        public float GetThirstModifierAmount()
        {
            return m_thirstModifierAmount;
        }
    }
}
