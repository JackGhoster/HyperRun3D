using UnityEngine;

namespace Game.Effects
{
    
    [CreateAssetMenu(menuName = "Configs/Boosters/" + nameof(HealthBoosterConfig), fileName = nameof(HealthBoosterConfig))]
    public class HealthBoosterConfig : ScriptableObject
    {
        [field: SerializeField] public float AddHealth { get; private set; }
    }
}