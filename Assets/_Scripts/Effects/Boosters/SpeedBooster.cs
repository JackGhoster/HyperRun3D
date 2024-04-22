using PlayerLogic;
using UnityEngine;

namespace Game.Effects
{
    public class SpeedBooster : Booster
    {
        [field: SerializeField] private SpeedBoosterConfig _boosterConfig;
        public override void ApplyEffect(Player player)
        {
            
            player.Stats += new PlayerStatsBuilder().WithSpeed(_boosterConfig.AddedSpeed).Build();
            base.ApplyEffect(player);
        }
        
        
    }
}