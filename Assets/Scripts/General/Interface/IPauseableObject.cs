using UnityEngine;

namespace Canicule
{
    public interface IPauseableObject 
    {
        public void OnPause() { }
        public void OnPlay() { }
        public void OnRegister() { }
    }
}
