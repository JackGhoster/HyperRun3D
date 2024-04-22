using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(EffectsConfig), fileName = nameof(EffectsConfig))]
    public class EffectsConfig : ScriptableObject
    {
        [field: SerializeField] public float Percentage { get; private set; } = 0.5f;
        [field: SerializeField] public List<AssetReferenceGameObject> EffectAssets { get; private set; }

        public AssetReferenceGameObject GetRandomAsset()
        {
            return EffectAssets[Random.Range(0, EffectAssets.Count)];
        }
    }
}