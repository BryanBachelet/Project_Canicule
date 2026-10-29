using UnityEngine;

namespace Canicule
{
    public class ConsumableInteractible : InteractibleObject
    {
        private ExhaustionManager m_exhaustionManager;
        [SerializeField] private float m_hungerModifierAmount;
        [SerializeField] private float m_thirstModifierAmount;

        private void Awake()
        {
            m_exhaustionManager = Object.FindAnyObjectByType<ExhaustionManager>();
        }

        public override void LaunchInteraction(Transform actor)
        {
            base.LaunchInteraction(actor);
            m_exhaustionManager.ModifyHungerExhaustion(m_hungerModifierAmount);
            m_exhaustionManager.ModifyThirstExhaustion(m_thirstModifierAmount);

            gameObject.SetActive(false);
        }

    }
}
