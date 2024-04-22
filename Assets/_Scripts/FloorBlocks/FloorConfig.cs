using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Game.Floor
{
    [CreateAssetMenu(menuName = "Configs/FloorConfig", fileName = nameof(FloorConfig))]
    public class FloorConfig : ScriptableObject
    {
        
        [field: SerializeField, Title("Enter amount of safe floors at the start of level")] public uint StartingSafeFloorsCount { get; private set; } = 3;
        [field: SerializeField, Title("Enter amount of floors to be generated")] public uint GenerateCount { get; private set; } = 10;
        [field: SerializeField] public List<FloorData> FloorDatas { get; private set; } = new List<FloorData>();

        private void OnValidate()
        {
            GenerateCount = (uint)Mathf.Max(StartingSafeFloorsCount, GenerateCount);
        }

        public AssetReferenceGameObject GetRandomSafeFloorAsset()
        {
            var safeDatas = FloorDatas.FindAll((data) => !data.isDangerous && data.floorType != FloorType.Win);
            var randomData = safeDatas[Random.Range(0, safeDatas.Count)];
            return randomData.assetRef;
        }

        public AssetReferenceGameObject GetDefaultFloorAsset()
        {
            TryGetFloorBy(FloorType.Default, out var data);
            return data.assetRef;
        }
        
        public AssetReferenceGameObject GetWinFloorAsset()
        {
            TryGetFloorBy(FloorType.Win, out var data);
            return data.assetRef;
        }
        
        public AssetReferenceGameObject GetRandomDangerousFloorAsset()
        {
            var dangerousDatas
                = FloorDatas.FindAll((data) => data.isDangerous && data.floorType != FloorType.Win);
            var randomData = dangerousDatas[Random.Range(0, dangerousDatas.Count)];
            return randomData.assetRef;
        }
        
        public AssetReferenceGameObject GetRandomFloorData()
        {
            var datas = FloorDatas.FindAll((data) => data.floorType != FloorType.Win);
            var randomData = datas[Random.Range(0, datas.Count)];
            return randomData.assetRef;
        }
        
        public bool TryGetFloorBy(FloorType type, out FloorData data)
        {
            data = null;
            if (type == FloorType.None) return false;
            
            data = FloorDatas.Find((floorData => floorData.floorType == type));
            return true;
        }

        public bool IsDangerous(FloorType floorType)
        {
            if (TryGetFloorBy(floorType, out var floorData))
            {
                return floorData.isDangerous;
            }

            return false;
        }

        public List<FloorType> GetDangerousTypes()
        {
            var dangerousTypes = new List<FloorType>();
            
            foreach (var floorData in FloorDatas)
            {
                if(floorData.isDangerous) dangerousTypes.Add(floorData.floorType);
                
            }

            return dangerousTypes;
        }

        public bool IsTurn(FloorType floorType)
        {
            if (TryGetFloorBy(floorType, out var floorData))
            {
                return floorType == FloorType.Left || floorType == FloorType.Right;
            }

            return false;
        }

        public List<AssetReferenceGameObject> GetAllFloorAssets()
        {
            List<AssetReferenceGameObject> floorAssets = new List<AssetReferenceGameObject>();
            foreach (var floorData in FloorDatas)
            {
                floorAssets.Add(floorData.assetRef);
            }

            return floorAssets;
        }

        public float GetLengthBy(FloorType floorType) => 
             FloorDatas.Find((floorData) => floorData.floorType == floorType)?.length ?? 0;
    }

    [Serializable]
    public class FloorData
    {
        [Title("Enter Information about the floor")]
        public FloorType floorType = FloorType.None;
        public float length = 4;
        public bool isDangerous = false;
        public AssetReferenceGameObject assetRef;
    }
}