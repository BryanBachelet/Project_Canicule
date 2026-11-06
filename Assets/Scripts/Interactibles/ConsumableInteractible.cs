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
            m_exhaustionManager = GameManager.GetExhaustionManager();
        }

        public override void LaunchInteraction(Transform actor, Interactor interactActorType)
        {
            base.LaunchInteraction(actor, interactActorType);
            m_exhaustionManager.ModifyHungerExhaustion(m_hungerModifierAmount);
            m_exhaustionManager.ModifyThirstExhaustion(m_thirstModifierAmount);

            gameObject.SetActive(false);
        }

    }
}
