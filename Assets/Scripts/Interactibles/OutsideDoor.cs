using UnityEngine;

namespace Canicule
{
    public class OutsideDoor : InteractibleObject
    {

        protected override void Interact()
        {
            Debug.Log(GameManager.GetNightDayManager().GetIsNightActive());

            if (!GameManager.GetNightDayManager().GetIsNightActive())
            {
                GameManager.GetInventoryDataManager().ActivateWorkSession();
                base.Interact();
            }
            else
            {
                CancelInteration();
                Debug.Log("Nope");
                // Should turn the change the cursor to cancel state
            }
        }
    }
}
