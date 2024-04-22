using PlayerLogic;
using UnityEngine;

namespace Game.Effects
{
    public class HealthBooster : Booster
    {
        [field: SerializeField] private HealthBoosterConfig _boosterConfig;
        public override void ApplyEffect(Player player)
        {
            player.Stats += new PlayerStatsBuilder().WithHealth(_boosterConfig.AddHealth).Build();
            print(player.Stats.health);
            base.ApplyEffect(player);
        }
    }
}