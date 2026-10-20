using UnityEngine;

namespace Canicule
{
    public class ConsumableInteractible : InteractibleObject
    {
        private ExhaustionManager m_exhaustionManager;
        [SerializeField] private float m_hungerDecreaseAmount;
        [SerializeField] private float m_thirstDecreaseAmount;

        private void Awake()
        {
            m_exhaustionManager = Object.FindAnyObjectByType<ExhaustionManager>();
        }

        public override void Interact()
        {
            base.Interact();
            m_exhaustionManager.DecreaseHungerExhaustion(m_hungerDecreaseAmount);
            m_exhaustionManager.DecreaseThirstExhaustion(m_thirstDecreaseAmount);

            gameObject.SetActive(false);
        }

    }
}
