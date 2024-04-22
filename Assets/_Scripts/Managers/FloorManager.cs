using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Floor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
    public class FloorManager : MonoBehaviour
    {
        [field: SerializeField] public FloorConfig FloorConfig { get; private set; }

        public List<Floor> GeneratedFloors { get; private set; } = new List<Floor>();
        public AssetManager AssetManager => GameManager.Instance.AssetManager;

        public bool FinishedGenerating { get; private set; } = false;


        public IEnumerator Init()
        {
            yield return PreloadFloors();

            yield return GenerateFloors();
        }

        private IEnumerator PreloadFloors()
        {
            bool finished = false;

            var assetRefs = FloorConfig.GetAllFloorAssets();

            foreach (var assetRef in assetRefs)
            {
                yield return AssetManager.HandleAssetLoading(assetRef);
            }
        }

        public IEnumerator GenerateFloors()
        {
            FinishedGenerating = false;
            
            yield return GenerateSafeFloors();
            yield return GenerateRestFloors();

            FinishedGenerating = true;
        }

        private IEnumerator GenerateSafeFloors()
        {
            int safeCount = (int)FloorConfig.StartingSafeFloorsCount;
            for (int i = 0; i < safeCount; i++)
            {
                var floorAsset = FloorConfig.GetDefaultFloorAsset();

                if (!AssetManager.PreloadedAssets.TryGetValue(floorAsset, out var go))
                {
                    yield return AssetManager.HandleAssetLoading(floorAsset);
                }

                InstantiateFloor(go);
            }
        }

        private IEnumerator GenerateRestFloors()
        {
            int count = (int)FloorConfig.GenerateCount;
            bool finished = false;

            //generate except win
            for (int i = 0; i < count - 1; i++)
            {
                var last = GeneratedFloors.LastOrDefault();

                var floorAsset = last != null && FloorConfig.IsDangerous(last.FloorType) ||
                                 FloorConfig.IsTurn(last.FloorType)
                    ? FloorConfig.GetDefaultFloorAsset()
                    : FloorConfig.GetRandomFloorData();

                if (!AssetManager.PreloadedAssets.TryGetValue(floorAsset, out var go))
                {
                    yield return AssetManager.HandleAssetLoading(floorAsset);
                }

                InstantiateFloor(go);
            }

            finished = true;

            yield return new WaitUntil(() => finished);

            var winAsset = FloorConfig.GetWinFloorAsset();
            if (!AssetManager.PreloadedAssets.TryGetValue(winAsset, out var winGo))
            {
                yield return AssetManager.HandleAssetLoading(winAsset);
            }

            InstantiateFloor(winGo);

        }

        private void InstantiateFloor(GameObject floorGO)
        {
            var floor = Instantiate(floorGO, Vector3.zero, Quaternion.identity).GetComponent<Floor>();
            RepositionFloor(floor);
            GeneratedFloors.Add(floor);
            floor.SetIndex((uint)GeneratedFloors.Count - 1);
            
            var effectManager = GameManager.Instance.EffectManager;
            if(floor.FloorIndex > FloorConfig.StartingSafeFloorsCount - 1) 
                effectManager.StartCoroutine(effectManager.TryToSpawnEffect(floor));
        }

        private void RepositionFloor(Floor floor)
        {
            var last = GeneratedFloors.LastOrDefault();
            if (last == null) return;

            if (last.IsTurn)
            {
                floor.gameObject.transform.rotation = Quaternion.Euler(last.transform.localRotation.eulerAngles + last.EndPoint.localRotation.eulerAngles);
                floor.gameObject.transform.position = last.EndPoint.transform.position;
                return;
            }
            
            var length = FloorConfig.GetLengthBy(last.FloorType);
            var rotation = last.gameObject.transform.rotation;
            var lastPosition = last.gameObject.transform.position;
            var offset = last.transform.right * length;
 
            floor.gameObject.transform.position = lastPosition + offset;
            floor.gameObject.transform.rotation = rotation;
        }


        public void SetFailedToCurrentFloor()
        {
            var curr = GetCurrentFloor();
            if(curr != null) curr.FailedToPass = true;
        }

        public Floor GetCurrentFloor()
        {
           return GeneratedFloors.Find((floor) => floor.FloorIndex == GameManager.Instance.PlayerManager.CurrentFloorIndex);
        }

        public Floor GetNextFloor()
        {
            var floorIndex = GameManager.Instance.PlayerManager.CurrentFloorIndex + 1;
            if (floorIndex == GeneratedFloors.Count) return GeneratedFloors.LastOrDefault();
            return GeneratedFloors.Find((floor) =>
                floor.FloorIndex == floorIndex);
        }


        public List<Floor> GetDangerousFloors()
        {
            List<Floor> floors = GeneratedFloors.FindAll((floor) => FloorConfig.IsDangerous(floor.FloorType)).ToList();

            return floors;
        }

        public List<FloorType> GetDangerousFloorTypes() => FloorConfig.GetDangerousTypes();

        public int GetCountOfFailedByType(FloorType type)
        {
            List<Floor> floors = GeneratedFloors.FindAll((floor) => floor.FloorType == type && floor.FailedToPass).ToList();

            return floors.Count;
        }

        public int GetCountOfPassedByType(FloorType type)
        {
            List<Floor> floors = GeneratedFloors.FindAll((floor) =>
                    floor.FloorType == type &&
                    !floor.FailedToPass &&
                    floor.FloorIndex <= GameManager.Instance.PlayerManager.CurrentFloorIndex).ToList();
               

            return floors.Count;
        }
        
    }
}