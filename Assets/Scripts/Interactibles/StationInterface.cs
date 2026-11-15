using UnityEngine;

namespace Canicule
{
    public interface StationInterface
    {
        public bool IsItemValid(ItemData itemTest);

        public void ValidateInteraction(ItemData itemData);

    }
}
