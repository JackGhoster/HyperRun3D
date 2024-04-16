using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Floor
{
    [CreateAssetMenu(menuName = "Configs/FloorConfig", fileName = nameof(FloorConfig))]
    public class FloorConfig : ScriptableObject
    {
        [field: SerializeField] public uint GenerateCount { get; private set; } = 10;
        [field: SerializeField] public List<FloorData> FloorDatas { get; private set; } = new List<FloorData>();
        
        public bool TryGetFloorBy(FloorType type, out FloorData data)
        {
            data = null;
            if (type == FloorType.None) return false;
            
            data = FloorDatas.Find((floorData => floorData.floorType == type));
            return true;
        }
    }

    [Serializable]
    public class FloorData
    {
        public FloorType floorType = FloorType.None;
        public float length = 4;
        public bool isDangerous = false;
        public FloorAssetReference assetReference;
    }

    [Serializable]
    public class FloorAssetReference : AssetReferenceT<Floor>
    {
        public FloorAssetReference(string guid) : base(guid)
        {
        }
    }
}