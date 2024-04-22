using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(DamageDealerConfig), fileName = nameof(DamageDealerConfig))]
    public class DamageDealerConfig : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; } = 1;
        [field: SerializeField] public float SlowTime { get; private set; } = 2;
        [field: SerializeField] public float SlowPercentage { get; private set; } = 0.5f;
    }
}