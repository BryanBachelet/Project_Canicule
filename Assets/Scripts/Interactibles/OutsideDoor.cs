using UnityEngine;

namespace Canicule
{
    public class OutsideDoor : InteractibleObject
    {

        protected override void Interact()
        {
            GameManager.GetInventoryDataManager().ActivateWorkSession();
            base.Interact();
        }
    }
}
