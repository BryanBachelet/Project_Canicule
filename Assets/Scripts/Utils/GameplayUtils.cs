using UnityEngine;
using UnityEngine.AI;

namespace Canicule
{
    public static class GameplayUtils
    {
        public static Vector3 GetSamplePosition(Vector3 source,float maxDistance = 1000,int areaMaskInt= NavMesh.AllAreas)
        {
            NavMeshHit hit = new NavMeshHit();
            if(NavMesh.SamplePosition(source,out hit,maxDistance, areaMaskInt))
            {
                return hit.position;
            }
            else
            {
                Debug.LogWarning("[Navigation] Coundn't find a sample position for the request");
                return source;
            }
        }
    }
}
