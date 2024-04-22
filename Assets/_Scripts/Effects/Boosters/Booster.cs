using System;
using System.Collections;
using System.Collections.Generic;
using PlayerLogic;
using UnityEngine;

namespace Game.Effects
{
    [RequireComponent(typeof(Collider))]
    public class Booster : MonoBehaviour, IPlayerEffect
    {
        public virtual void ApplyEffect(Player player)
        {
            Destroy(gameObject, 0.1f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                ApplyEffect(player);
            }
        }
    }
}