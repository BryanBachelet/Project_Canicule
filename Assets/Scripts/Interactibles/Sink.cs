using UnityEngine;

namespace Canicule
{
    public class Sink : InteractibleObject, StationInterface
    {
        private ItemData interactItem;


        public override void LaunchInteraction(Transform actor, Interactor interactActorType)
        {
            base.LaunchInteraction(actor, interactActorType);
            if(m_interactor == Interactor.PLAYER)
            {
                CharacterOpenInventory charaOpenInventory = actor.GetComponent<CharacterOpenInventory>();
                charaOpenInventory.RequestInventorySelection(this);
            }
        }
        public bool IsItemValid(ItemData itemTest)
        {
            if (itemTest.GetItemEffect<WaterBowlEffect>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected override void UpdateInteraction()
        {
            // Empty
        }

        public void ValidateInteraction(ItemData itemData)
        {
            interactItem = itemData;
            Interact();
        }

        protected override void Interact()
        {
            WaterBowlEffect bowlEffect = interactItem.GetItemEffect<WaterBowlEffect>();
            bowlEffect.m_isFill = true;
            base.Interact();
            interactItem = null;
        }

    }
}
