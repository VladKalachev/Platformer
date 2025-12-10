using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        [Serializable]
        public class InventoryItemData
        {
            public string Id;
            public int Value;
        }
    }
}