using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Effects
{
    [RequireComponent(typeof(Collider))]
    public class Booster : MonoBehaviour, IPlayerEffect
    {
        public void ApplyEffect()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            ApplyEffect();
        }
    }
}