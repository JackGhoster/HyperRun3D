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

        public Dictionary<AssetReference, GameObject> PreloadedAssets { get; } =
            new Dictionary<AssetReference, GameObject>();


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
                yield return HandleAssetLoading(assetRef);
            }
        }

        public IEnumerator GenerateFloors()
        {
            yield return GenerateSafeFloors();
            yield return GenerateRestFloors();
        }

        private IEnumerator GenerateSafeFloors()
        {
            int safeCount = (int)FloorConfig.StartingSafeFloorsCount;
            for (int i = 0; i < safeCount; i++)
            {
                var floorAsset = FloorConfig.GetDefaultFloorAsset();

                if (!PreloadedAssets.TryGetValue(floorAsset, out var go))
                {
                    yield return HandleAssetLoading(floorAsset);
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

                if (!PreloadedAssets.TryGetValue(floorAsset, out var go))
                {
                    yield return HandleAssetLoading(floorAsset);
                }

                InstantiateFloor(go);
            }

            finished = true;

            yield return new WaitUntil(() => finished);

            var winAsset = FloorConfig.GetWinFloorAsset();
            if (!PreloadedAssets.TryGetValue(winAsset, out var winGo))
            {
                yield return HandleAssetLoading(winAsset);
            }

            InstantiateFloor(winGo);

        }

        private IEnumerator HandleAssetLoading(AssetReferenceGameObject assetRef)
        {
            var op = assetRef.LoadAssetAsync();

            yield return op;

            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                PreloadedAssets.Add(assetRef, op.Result);
            }
            else
            {
                print($"Error while loading asset: {assetRef.Asset.name}");
            }
        }

        private void InstantiateFloor(GameObject floorGO)
        {
            var floor = Instantiate(floorGO, Vector3.zero, Quaternion.identity).GetComponent<Floor>();
            RepositionFloor(floor);
            GeneratedFloors.Add(floor);
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

    }
}