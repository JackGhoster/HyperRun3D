using System.Collections;
using Game.Floor;
using UnityEngine;

namespace Managers
{
    public class EffectManager : MonoBehaviour
    {
        [field: SerializeField] public EffectsConfig EffectsConfig { get; private set; }
        public AssetManager AssetManager => GameManager.Instance.AssetManager;
        public bool Inited { get; private set; }
        
        public IEnumerator Init()
        {
            yield return PreloadFloors();
            Inited = true;
        }

        private IEnumerator PreloadFloors()
        {
            bool finished = false;

            var assetRefs = EffectsConfig.EffectAssets;

            foreach (var assetRef in assetRefs)
            {
                yield return AssetManager.HandleAssetLoading(assetRef);
            }
        }
        public IEnumerator TryToSpawnEffect(Floor floor)
        {
            var randomValue = Random.value;
            if (floor.FloorType != FloorType.Default || randomValue < EffectsConfig.Percentage) yield break;
            
            var effectAsset = EffectsConfig.GetRandomAsset();
                
            if (!AssetManager.PreloadedAssets.TryGetValue(effectAsset, out var go))
            {
                yield return AssetManager.HandleAssetLoading(effectAsset);
            }

            InstantiateEffect(floor.transform, go);
        }
        
        
        private void InstantiateEffect(Transform floorTransform, GameObject effectGo)
        {
            var effect = Instantiate(effectGo,
                floorTransform.position + (Vector3.up * 2),
                Quaternion.LookRotation(floorTransform.right,
                    Vector3.up));
        }


    }
}