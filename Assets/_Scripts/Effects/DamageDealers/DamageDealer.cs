using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using PlayerLogic;
using UnityEngine;

namespace Game.Effects
{
    [RequireComponent(typeof(Collider))]
    public class DamageDealer : MonoBehaviour, IPlayerEffect
    {
        [SerializeField] private DamageDealerConfig _config;
        public void ApplyEffect(Player player)
        {
            player.ApplyDamage(_config.Damage, _config.SlowTime, _config.SlowPercentage);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                GameManager.Instance.FloorManager.SetFailedToCurrentFloor();
                ApplyEffect(player);
            }
        }
    }
}