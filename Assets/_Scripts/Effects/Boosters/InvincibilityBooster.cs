using PlayerLogic;
using UnityEngine;

namespace Game.Effects
{
    public class InvincibilityBooster : Booster
    {
        [SerializeField] private InvincibilityBoosterConfig _boosterConfig;
        public override void ApplyEffect(Player player)
        {
            player.StartCoroutine(player.TemporaryInvincibility(_boosterConfig.Time));
            base.ApplyEffect(player);
        }
    }
}