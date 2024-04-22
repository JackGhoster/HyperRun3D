using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerLogic
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(PlayerConfig), fileName = nameof(PlayerConfig))]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject PlayerAsset { get; private set; }
        [field: SerializeField] public float DefaultPlayerSpeed { get; private set; } = 5;
        [field: SerializeField] public float DefaultPlayerHealth { get; private set; } = 2;
        [field: SerializeField] public int DefaultJumpCount { get; private set; }
        [field: SerializeField] public float DefaultJumpHeight { get; private set; }
        [field: SerializeField] public float DefaultGravity { get; private set; }
    }
}