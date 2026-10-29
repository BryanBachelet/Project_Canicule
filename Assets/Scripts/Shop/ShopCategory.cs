using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Canicule
{
    [System.Serializable]
    public class ShopCategory
    {
        public ExhaustionType type;
        public List<ItemData> items;
    }
}
