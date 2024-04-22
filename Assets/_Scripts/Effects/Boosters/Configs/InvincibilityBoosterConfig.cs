using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(menuName = "Configs/Boosters/" + nameof(InvincibilityBoosterConfig), fileName = nameof(InvincibilityBoosterConfig))]
    public class InvincibilityBoosterConfig : ScriptableObject
    {
        [field: SerializeField] public float Time { get; private set; }
    }
}