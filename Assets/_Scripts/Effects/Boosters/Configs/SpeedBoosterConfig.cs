using UnityEngine;

namespace Game.Effects
{
    
    [CreateAssetMenu(menuName = "Configs/Boosters/" + nameof(SpeedBoosterConfig), fileName = nameof(SpeedBoosterConfig))]
    public class SpeedBoosterConfig : ScriptableObject
    {
        [field: SerializeField] public float AddedSpeed { get; private set; }
    }
}