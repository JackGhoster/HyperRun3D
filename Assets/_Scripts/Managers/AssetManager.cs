using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
    public class AssetManager : MonoBehaviour
    {
        public Dictionary<AssetReference, GameObject> PreloadedAssets { get; } =
            new Dictionary<AssetReference, GameObject>();

        private List<AsyncOperationHandle<GameObject>> _handles = new List<AsyncOperationHandle<GameObject>>();

        // public static AssetManager Instance { get; private set; } = null;
        public bool Initialized { get; set; } = false;

        // private void Awake()
        // {
        //     TryInitializeSingleton();
        // }
        //
        // private bool TryInitializeSingleton()
        // {
        //     if (AssetManager.Instance != null)
        //     {
        //         Destroy(this.gameObject);
        //         return false;
        //     }
        //
        //     AssetManager.Instance = this;
        //     this.Initialized = true;
        //     // DontDestroyOnLoad(this.gameObject);
        //     return true;
        // }


        public IEnumerator HandleAssetLoading(AssetReferenceGameObject assetRef)
        {
            var op = assetRef.LoadAssetAsync();
            _handles.Add(op);

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

        public void CleanUp()
        {
            foreach (var op in _handles)
            {
                Addressables.Release(op);
            }
        }
    }
}