using UnityEngine;

namespace Canicule
{
    public class OutsideDoor : InteractibleObject
    {

        protected override void Interact()
        {
            if (!GameManager.GetNightDayManager().GetIsNightActive())
            {
                GameManager.GetInventoryDataManager().ActivateWorkSession();
                base.Interact();
            }
            else
            {
                CancelInteration();
                // Should turn the change the cursor to cancel state
            }
        }
    }
}
