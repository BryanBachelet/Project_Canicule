using System.Collections.Generic;
using UnityEngine;

namespace Canicule
{
    [CreateAssetMenu(fileName = "ShopContent", menuName = "Scriptable Objects/ShopContent")]
    public class ShopContent : ScriptableObject
    {
        public List<ShopCategory> shopCategories;
    }
}
